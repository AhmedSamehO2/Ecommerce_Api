using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Mapper;
using Talabat.Core;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extension
{
    public static class ApplicationSerivecsExtension
    {
        public static IServiceCollection AddAplicationServics(this IServiceCollection Services)
        {
            Services.AddSingleton<IResponseCashService, ResponseCashService>();
            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
         //   Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(DomainProfile));
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var Error = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                           .SelectMany(P => P.Value.Errors)
                                                           .Select(E => E.ErrorMessage).ToArray();
                    var ErrorValidationResponse = new ApiValidationError()
                    {
                        Errors = Error
                    };
                    return new BadRequestObjectResult(ErrorValidationResponse);
                };
            });
            return Services;
        }
    }
}
