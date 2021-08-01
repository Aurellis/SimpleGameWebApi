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
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(ILogger<RegisterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Register>> Get()
        {
            return  new Register() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Register>> Get(string name)
        {
            return new Register() { Name = name};
        }


    }
}
