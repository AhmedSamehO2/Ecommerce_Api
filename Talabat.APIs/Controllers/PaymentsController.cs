using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.DTOS;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
   
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService,IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }


        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{BasketId}")]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
          var customerBasket = await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            if (customerBasket is null) return BadRequest(new ApiResponse(400,"There is Proble In Your Basket "));
            var MappedBasket = _mapper.Map<CustomerBasket,CustomerBasketDto>(customerBasket);
            return Ok(MappedBasket);
        }


        const string endpointSecret = "whsec_4f9895d99163d76e36e25e55e9d11e813201659c055dd041db9097d32a1d9425";
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    _paymentService.UpdatePaymentIntentSuccssedOrFailed(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    _paymentService.UpdatePaymentIntentSuccssedOrFailed(paymentIntent.Id, true);

                }
                // ... handle other event types
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
