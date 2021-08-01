using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GunsController : ControllerBase
    {
        private readonly ILogger<GunsController> _logger;

        public GunsController(ILogger<GunsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Guns>> Get()
        {
            return  new Guns() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Guns>> Get(string name)
        {
            return new Guns() { Name = name};
        }


    }
}
