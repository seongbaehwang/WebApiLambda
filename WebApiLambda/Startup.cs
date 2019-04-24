using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiLambda.Extensions;
using WebApiLambda.Infrastructure;

namespace WebApiLambda
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // TODO: This is for testing purpose
            services.AddSingleton(services);

            services.Configure<ExceptionHandlerOptions>(options =>
                options.ExceptionHandler = ExceptionHandler.JsonResponeExceptionHandler);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder.AllowAnyOrigin());
            });

            services.AddSwaggerDocumentation();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    // Use DefaultContractResolver, if Pascal casing needs to be used
                    // options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            app.UseExceptionHandler();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseSwaggerDocumentation(env, configuration);

            app.UseMvc();
        }
    }
}
