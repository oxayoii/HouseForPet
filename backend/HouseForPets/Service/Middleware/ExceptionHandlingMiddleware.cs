using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Service.Middleware.CustomException;

namespace Service.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ExceptionHandlingMiddleware(RequestDelegate _next)
        {
            next = _next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case NotFoundException notFound:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFound.Message;
                    break;
                case BadRequestException badRequest:
                    statusCode = HttpStatusCode.BadRequest;
                    message = badRequest.Message;
                    break;
                case ForbiddenException forbidden:
                    statusCode = HttpStatusCode.Forbidden;
                    message = forbidden.Message;
                    break;
                case UnauthorizedException unauthorized:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = unauthorized.Message;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = exception.ToString();
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new { message }));
        }
    }
}
