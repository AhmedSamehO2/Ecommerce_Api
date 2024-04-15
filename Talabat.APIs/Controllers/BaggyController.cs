using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class BaggyController : APIBaseController
    {
        private readonly StoreContext _storeContext;

        public BaggyController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        [HttpGet("NotFound/{id}")]
        public IActionResult GetNotFound(int id)
        {
            var product = _storeContext.products.Where(P => P.Id == id).FirstOrDefault();
            if(product is null) { return NotFound(); }
            return Ok(product);
        }
        [HttpGet("ServerError")]
        public IActionResult GetServerError()
        {
            var product = _storeContext.products.Find(100);
           var ProductToReturn = product.ToString();
            return Ok(ProductToReturn);
        }
    }
}
