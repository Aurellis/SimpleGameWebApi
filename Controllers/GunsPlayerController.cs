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
    public class GunsPlayerController : ControllerBase
    {
        private readonly ILogger<GunsPlayerController> _logger;

        public GunsPlayerController(ILogger<GunsPlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<GunsPlayer>> Get()
        {
            return  new GunsPlayer() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<GunsPlayer>> Get(string name)
        {
            return new GunsPlayer() { Name = name};
        }


    }
}
