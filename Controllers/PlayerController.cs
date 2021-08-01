using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskGame.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;

        public PlayerController(ILogger<PlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Player>> Get()
        {
            return  new Player() { Name = "Is Name" };
        }

        
        [HttpGet("{name}")]
        public async Task<ActionResult<Player>> Get(string name)
        {
            return new Player() { Name = name};
        }


    }
}
