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
using Talabat.Core.Specification.OrderSpec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository , IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, OrderAddress address)
        {
            // 1.Get Basket From Basket Repo
            var Basket = await _basketRepository.getBasketAsync(BasketId);
            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrder = new ProductItemOrder(Product.Name, Product.Id, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrder, Product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }
            //3.Calculate SubTotal
            var SubTotal = OrderItems.Sum(Item => Item.Price * Item.Qunatity);
            //4.Get Delivery Method From DeliveryMethod Repo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            //5.Create Order
            var spec = new OrderWithPaymentSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetByIdwithSpecAsync(spec);
            if (ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }
            var Order = new Order(BuyerEmail,address, DeliveryMethod,OrderItems,SubTotal,Basket.PaymentIntentId);
            //6.Add Order Locally
           await _unitOfWork.Repository<Order>().AddAsync(Order);
            //7.Save Order To Database[ToDo]
          var result =  await _unitOfWork.CompliteAsync();
            if (result <= 0) return null;
            return Order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
           var deliveryMethod = _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethod;
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
        {   
            var spec = new OrderSpecification(BuyerEmail, OrderId);
            var Orders = await _unitOfWork.Repository<Order>().GetByIdwithSpecAsync(spec);
            return Orders;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecification(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return Orders;
        }
    }
}
