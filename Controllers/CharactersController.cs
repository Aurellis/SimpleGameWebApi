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
    public class CharactersController : ControllerBase
    {
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(ILogger<CharactersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Characters>> Get()
        {
            return  new Characters() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Characters>> Get(string name)
        {
            return new Characters() { Name = name};
        }


    }
}
