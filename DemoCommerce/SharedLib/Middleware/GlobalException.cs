using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Logs;
using System.Net;

namespace SharedLib.Middleware
{
    public class GlobalException(RequestDelegate next)
    {
        string message = "";
        int statusCode = 0;
        string title = "";
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
                if(context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    message = "sorry, internal server error occured, kindly try again";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    title = "Error";
                    await ModifyHeader(context, title, message, statusCode);
                }
                // check if the exception is too many requests -> 429
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    title = "Warning";
                    message = "Too many requests made";
                    statusCode = (int)StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, title, message, statusCode);
                }
                //if Response is unauthorized -> 401
                if(context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    title = "Alert";
                    message = "You are not authorized to access";
                    statusCode = (int)StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, title, message, statusCode);
                }

                //if Response is forbidden -> 403
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    title = "Forbidden Access";
                    message = "You are not allowed to access";
                    statusCode = (int)StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, title, message, statusCode);
                }

            }
            catch(Exception ex)
            {
                //Log original exceptions -> file, debugger, console
                LogException.LogError(ex);

                //check if exception is timeout
                if (ex is TaskCanceledException || ex is TimeoutException)
                {
                    title = "Out of time";
                    message = "Request timeout. Please try again";
                    statusCode = (int)StatusCodes.Status408RequestTimeout;
                }

                //if none of the exceptions then do the default
                await ModifyHeader(context, title, message, statusCode);

            }
        }
        private static async Task ModifyHeader(HttpContext context, string title, string message, int StatusCode)
        {
            // display minified message to user
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = StatusCode,
                Title = title
            }), CancellationToken.None);
        }
    }

   
}
