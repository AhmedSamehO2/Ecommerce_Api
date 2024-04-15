using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregat;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Talabat.Core.Specification;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            // Secrit Key
            StripeConfiguration.ApiKey = _configuration["StripeKey:Secretkey"];
            var Basket = await _basketRepository.getBasketAsync(BasketId);
            if(Basket is null) return null;
            var ShippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = deliveryMethod.Cost;
            }
            if(Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var subTotal = Basket.Items.Sum(Item => Item.Price * Item.Quantity);

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)ShippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.clientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)ShippingPrice * 100,
                };
                paymentIntent = await service.UpdateAsync(Basket.PaymentIntentId, Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.clientSecret = paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }

        public async Task<Order> UpdatePaymentIntentSuccssedOrFailed(string paymentIntent, bool flag)
        {
            var spec = new OrderWithPaymentSpec(paymentIntent);
         var order  = await _unitOfWork.Repository<Order>().GetByIdwithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;

            }
             _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompliteAsync();
            return order;
        }
    }
}
