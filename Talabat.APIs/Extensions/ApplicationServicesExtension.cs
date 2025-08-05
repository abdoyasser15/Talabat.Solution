using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talbat.Core;
using Talbat.Core.Repositories.Contract;
using Talbat.Core.Services.Contract;
using Talbat.Repository;
using Talbat.Service;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IResponseCashService),typeof(ResponseCashService));

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService)); 

            services.AddScoped(typeof(IProductService),typeof(ProductService));

            services.AddScoped(typeof(IUntiOfWork), typeof(UnitOfWork));
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddScoped(typeof(IBasketRepository),typeof(BasketRepository));

            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}
