using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOS;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repository;

namespace Talabat.APIs.Controllers
{

    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BasketController(IBasketRepository basketRepo,IMapper mapper,IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
           _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        // GET or ReCreate
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string BasketId)
        {
            var basket = await _basketRepo.getBasketAsync(BasketId);
            return basket is null ? new CustomerBasket(BasketId) : basket;
        }

        //Update
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var result = _mapper.Map<CustomerBasket>(basket);   
            var CreateOrUpdate = await _basketRepo.UpdateBasketAsync(result);
            if (CreateOrUpdate is null)
                return BadRequest(new ApiResponse(400));
            return Ok(CreateOrUpdate);
        }
        //Delete
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepo.DeleteBasketAsync(BasketId);
        }
    }
}
