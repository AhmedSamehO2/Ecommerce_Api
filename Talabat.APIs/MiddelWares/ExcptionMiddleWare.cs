using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddelWares
{
    public class ExcptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExcptionMiddleWare(RequestDelegate Next,ILogger<ExcptionMiddleWare> logger,IHostEnvironment env)
        {
            _next = Next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
               await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                //if (_env.IsDevelopment())
                //{
                //    var Response = new ApiExcptionRespons(500,ex.Message,ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExcptionRespons(500);
                //}

                var Response = _env.IsDevelopment()? new ApiExcptionRespons(500, ex.Message, ex.StackTrace.ToString())
                    : new ApiExcptionRespons(500, ex.Message, ex.StackTrace.ToString());
                var Options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var JsonRespnse = JsonSerializer.Serialize(Response,Options);
                httpContext.Response.WriteAsync(JsonRespnse);
            }
        }
    }
}
