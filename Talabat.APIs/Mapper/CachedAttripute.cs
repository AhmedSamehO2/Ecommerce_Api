using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core;

namespace Talabat.APIs.Mapper
{
    public class CachedAttripute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSecond;

        public CachedAttripute(int ExpireTimeInSecond)
        {
            _expireTimeInSecond = ExpireTimeInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var cashService =  context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>();
            var CashedKey = GenerateCashKeyFromRequest(context.HttpContext.Request);
           var CashedResponse =  await cashService.GetCashedResponse(CashedKey);
            if(!string.IsNullOrEmpty(CashedResponse))
            {
                var contentResult = new ContentResult()
                {
                  Content = CashedResponse,
                  ContentType = "application/json",
                  StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
          var ExcutedEndPointContext = await next.Invoke();
            if(ExcutedEndPointContext.Result is OkObjectResult result)
            {
                await cashService.CashResponseAsync(CashedKey, result.Value, TimeSpan.FromSeconds(_expireTimeInSecond));
            }
        }

        private string GenerateCashKeyFromRequest(HttpRequest request)
        {
           var KeyBuilder = new StringBuilder();

            KeyBuilder.Append(request.Path);
            foreach (var (key,value) in request.Query.OrderBy(X=>X.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
