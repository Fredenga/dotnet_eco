using Microsoft.AspNetCore.Http;

namespace SharedLib.Middleware
{
    public class ListenToOnlyApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Extract specific header from the request
            var signedHeader = context.Request.Headers["Api-Gateway"];

            // NULL -> request is not coming from the API gateway
            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service is unavailable");
            } else
            {
                await next(context);
            }
        }
    }
}
