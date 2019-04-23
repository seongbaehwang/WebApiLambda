using System.Linq;
using System.Security.Claims;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;

namespace WebApiLambda
{
    public class LambdaFunction : APIGatewayProxyFunction<Startup>
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        // Lambda logs console output to CloudWatch
                        .WriteTo.Console(new JsonFormatter(renderMessage: true));
                });
        }

        protected override void PostCreateContext(HostingApplication.Context context, APIGatewayProxyRequest lambdaRequest, ILambdaContext lambdaContext)
        {
            base.PostCreateContext(context, lambdaRequest, lambdaContext);

            AddNameClaim(context.HttpContext.User.Identity as ClaimsIdentity);
        }

        private void AddNameClaim(ClaimsIdentity identity)
        {
            if (identity == null) return;

            if (!identity.HasClaim(c => c.Type == ClaimTypes.Name) && identity.HasClaim(c => c.Type == "cognito:username"))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, identity.Claims.First(c => c.Type == "cognito:username").Value));
            }
        }
    }
}
