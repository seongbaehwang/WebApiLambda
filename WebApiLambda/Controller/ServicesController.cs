using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiLambda.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceCollection _services;

        public ServicesController(IServiceCollection services)
        {
            _services = services;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _services.Select(sd => $"{sd.ServiceType} - {sd.ImplementationType}");
        }
    }
}