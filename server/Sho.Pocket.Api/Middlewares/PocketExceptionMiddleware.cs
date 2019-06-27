using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sho.Pocket.Api.Models;
using Sho.Pocket.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Sho.Pocket.Api.Middlewares
{
    public class PocketExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public PocketExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if (exception is BasePocketException pocketException)
            {
                context.Response.StatusCode = (int)pocketException.StatusCode;

                List<ResponseError> errors = new List<ResponseError>
                {
                    new ResponseError(pocketException.Code, pocketException.Message)
                };

                return context.Response.WriteAsync(JsonConvert.SerializeObject(errors));
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return context.Response.WriteAsync("Error occured. Please contact system administrator");
            }
        }
    }
}
