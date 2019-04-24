using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApiLambda.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ILogger<ServicesController> _logger;
        private readonly IServiceCollection _services;

        public ServicesController(IServiceCollection services, ILogger<ServicesController> logger)
        {
            _services = services;
            _logger = logger;
        }

        /// <summary>
        /// Sample method to return all the registered services
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")] // for Swagger
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public IEnumerable<string> Get(bool? throwException)
        {
            _logger.LogDebug("{@LambdaContext}", Request.HttpContext.Items[AbstractAspNetCoreFunction.LAMBDA_CONTEXT]);
            _logger.LogDebug("{@LambdaRequestObject}", Request.HttpContext.Items[AbstractAspNetCoreFunction.LAMBDA_REQUEST_OBJECT]);

            if (throwException != null && throwException.Value)
            {
                throw new Exception("Exception test");
            }

            return _services.Select(sd => $"{sd.ServiceType} - {sd.ImplementationType}");
        }
    }
}