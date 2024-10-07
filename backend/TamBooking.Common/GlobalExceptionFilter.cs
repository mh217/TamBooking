using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Net;
using System.Net.Mail;

namespace TamBooking.Common
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NpgsqlException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = "Internal Server Error",
                };
            }
            else if (context.Exception is SecurityTokenExpiredException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = "Token has expired.",
                };
            }
            else if (context.Exception is SecurityTokenException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Content = "Token is not valid.",
                };
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = "You do not have permission to access this resource.",
                };
            }
            else if (context.Exception is InvalidOperationException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = context.Exception.Message,
                };
            }
            else if (context.Exception is SmtpException)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = "Email sending error.",
                };
            }
            else if (context.Exception is not null)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = "An unexpected error occurred. Please try again later.",
                };
            }
            context.ExceptionHandled = true;
        }
    }
}