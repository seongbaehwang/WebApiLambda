using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;

namespace WebApiLambda
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .WriteTo.Console(new JsonFormatter(renderMessage: true));
                })
                .UseStartup<Startup>();
    }
}
