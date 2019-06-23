using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace WebApiLambda.Infrastructure
{
    internal static class ExceptionHandler
    {
        public static RequestDelegate JsonResponeExceptionHandler = async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                var env = context.RequestServices.GetRequiredService<IHostingEnvironment>();

                if (env.IsDevelopment())
                {
                    await WriteResponseBodyAsync(context, contextFeature.Error);
                }
                else
                {
                    await WriteResponseBodyAsync(context);
                }
            }
        };

        private static async Task WriteResponseBodyAsync(HttpContext context)
        {
            var mvcJsonOptions = context.RequestServices.GetRequiredService<IOptions<MvcJsonOptions>>().Value;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new
                {
                    context.Response.StatusCode,
                    Message = "Internal Server Error."
                }, mvcJsonOptions.SerializerSettings));
        }

        private static async Task WriteResponseBodyAsync(HttpContext context, Exception error)
        {
            var mvcJsonOptions = context.RequestServices.GetRequiredService<IOptions<MvcJsonOptions>>().Value;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new
                {
                    context.Response.StatusCode,
                    error.Message,
                    Error = error.ToString()
                }, mvcJsonOptions.SerializerSettings));
        }
    }
}
