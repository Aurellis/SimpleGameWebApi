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
    public class PayCharactersPlayerController : ControllerBase
    {
        private readonly ILogger<PayCharactersPlayerController> _logger;

        public PayCharactersPlayerController(ILogger<PayCharactersPlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PayCharactersPlayer>> Get()
        {
            return  new PayCharactersPlayer() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<PayCharactersPlayer>> Get(string name)
        {
            return new PayCharactersPlayer() { Name = name};
        }


    }
}
