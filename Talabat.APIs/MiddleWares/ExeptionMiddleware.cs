using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
    // By Convention 
    public class ExeptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExeptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExeptionMiddleware(RequestDelegate next , ILogger<ExeptionMiddleware> logger , IHostEnvironment env) // nex is  Delegate
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context); // Call the next middleware in the pipeline
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message); // Development Logging
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError; // 500 Internal Server Error
                var response = env.IsDevelopment() 
                    ?new ApiExceptionResponse(context.Response.StatusCode, ex.Message, env.IsDevelopment() ? ex.StackTrace?.ToString() : null)
                    : new ApiExceptionResponse(context.Response.StatusCode);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Use camelCase for JSON properties
                    WriteIndented = true // Format the JSON output
                };
                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json); // Write the response to the client
            }
        }
    }
}
