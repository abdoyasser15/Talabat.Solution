using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talbat.Core.Services.Contract;

namespace Talabat.APIs.Helpers
{
    public class CashedAttribute : Attribute, IAsyncActionFilter
    {

        public CashedAttribute()
        {
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ResponseCashService = context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>(); // Ask Clr to inject IResponseCashService Explicitly

            var cacheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);

            var Response = await ResponseCashService.GetCashedResonseAsync(cacheKey);

            if(!string.IsNullOrEmpty(Response))// Response Is Not Cashed 
            {
                var result = new ContentResult
                {
                    Content = Response,
                    ContentType = "application/json",
                    StatusCode = 200 // OK
                };
                context.Result = result; // Set the result to the cached response
                return;
            }

            var executedActionContext =  await next.Invoke(); // Will Execute the next action Filter Or Action Itself 

            if(executedActionContext.Result is OkObjectResult okobjectResult && okobjectResult.Value is not null)
            {
                await ResponseCashService.CashResponseAsync(cacheKey, okobjectResult.Value, TimeSpan.FromMinutes(5));
            }
        }

        private string GenerateCasheKeyFromRequest(HttpRequest request)
        {
            // {{url}}/api/products?pageIndex=1&pageSize=10&sort=name
            var KeyBuilder = new StringBuilder();

            KeyBuilder.Append($"{request.Path}"); // /api/products

            foreach(var (key, value) in request.Query) // pageIndex=1&pageSize=10&sort=name
            {
                KeyBuilder.Append($"|{key}-{value}"); // /api/products|pageIndex-1|pageSize-10|sort-name
            }
            return KeyBuilder.ToString();
        }
    }
}
