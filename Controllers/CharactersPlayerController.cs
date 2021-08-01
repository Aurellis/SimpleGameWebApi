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
    public class CharactersPlayerController : ControllerBase
    {
        private readonly ILogger<CharactersPlayerController> _logger;

        public CharactersPlayerController(ILogger<CharactersPlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CharactersPlayer>> Get()
        {
            return  new CharactersPlayer() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<CharactersPlayer>> Get(string name)
        {
            return new CharactersPlayer() { Name = name};
        }


    }
}
