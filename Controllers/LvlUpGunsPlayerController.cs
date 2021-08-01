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
    public class LvlUpGunsPlayerController : ControllerBase
    {
        private readonly ILogger<LvlUpGunsPlayerController> _logger;

        public LvlUpGunsPlayerController(ILogger<LvlUpGunsPlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<LvlUpGunsPlayer>> Get()
        {
            return  new LvlUpGunsPlayer() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<LvlUpGunsPlayer>> Get(string name)
        {
            return new LvlUpGunsPlayer() { Name = name};
        }


    }
}
