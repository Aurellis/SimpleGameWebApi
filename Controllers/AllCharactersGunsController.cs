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
    public class AllCharactersGunsController : ControllerBase
    {
        private readonly ILogger<AllCharactersGunsController> _logger;

        public AllCharactersGunsController(ILogger<AllCharactersGunsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<AllCharactersGuns>> Get()
        {
            return new AllCharactersGuns() { Name = "OK" }; ;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<AllCharactersGuns>> Get(string name)
        {
            return new AllCharactersGuns() { Name = name};
        }


    }
}
