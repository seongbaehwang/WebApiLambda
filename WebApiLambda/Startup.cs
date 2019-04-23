using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
