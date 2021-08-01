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
    public class PayGunsPlayerController : ControllerBase
    {
        private readonly ILogger<PayGunsPlayerController> _logger;

        public PayGunsPlayerController(ILogger<PayGunsPlayerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PayGunsPlayer>> Get()
        {
            return  new PayGunsPlayer() { Name = "Is Name" };
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<PayGunsPlayer>> Get(string name)
        {
            return new PayGunsPlayer() { Name = name};
        }


    }
}
