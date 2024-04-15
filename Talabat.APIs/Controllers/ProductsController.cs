using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOS;
using Talabat.APIs.Errors;
using Talabat.APIs.Mapper;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specification;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController( IMapper mapper,IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
           _unitOfWork = unitOfWork;
        }
        [CachedAttripute(300)]
        [HttpGet]
        public async Task<ActionResult<Pagination<IReadOnlyList<ProductToReturnDto>>>> GetProduct([FromQuery]ProductSpecParams Params)
        {
            var spec = new ProductWithPrandTypeSpec(Params);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var result = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            var CountSpec = new ProductWithFiltirationforCount(Params);
                var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex,Params.PageSize,result,Count));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> getProductById(int id)
        {
            var spec = new ProductWithPrandTypeSpec(id);
            var product = await _unitOfWork.Repository<Product>().GetByIdwithSpecAsync(spec);
            if (product is null)
                return NotFound(new ApiResponse(404));
            var result = _mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(result);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypeProduct()
        {
            var productType = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(productType);
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductPrand()
        {
            var productprand = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(productprand);
        }

    }
}
