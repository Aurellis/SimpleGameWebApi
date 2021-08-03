using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestTaskGame.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost("register")]
        public IActionResult Register(AutentificateRequest request)
        {
            var respose = _playerService.Register(request);

            if (respose == null)
            {
                return BadRequest(new { message = "Registration failed" });
            }

            return new JsonResult(new { message = "Registration complete" }) { StatusCode = StatusCodes.Status201Created};
        }

        [HttpPost("autentificate")]
        public IActionResult Autentificate(AutentificateRequest request)
        {
            var respose = _playerService.Autentificate(request);

            if (respose == null)
            {
                return BadRequest(new { message = "Incorrect username or password" });
            }
            return Ok(respose);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetPlayer()
        {
            Player player = (Player)HttpContext.Items["Player"];
            return Ok(_playerService.GetByName(player.Name));
        }

        [Authorize]
        [HttpGet("allguns")]
        public IActionResult AllGuns()
        {
            Player player = (Player)HttpContext.Items["Player"];
            return Ok(_playerService.AllGuns(player.Name));
        }

        [Authorize]
        [HttpGet("allcharacters")]
        public IActionResult AllCharacters()
        {
            Player player = (Player)HttpContext.Items["Player"];
            return Ok(_playerService.AllCharacters(player.Name));
        }

        [Authorize]
        [HttpGet("mycharacters")]
        public IActionResult MyCharacters()
        {
            Player player = (Player)HttpContext.Items["Player"];
            return Ok(_playerService.AllCharacters(player.Name).Where(c => c.Unlocked == true));
        }

        [Authorize]
        [HttpGet("myguns")]
        public IActionResult MyGuns()
        {
            Player player = (Player)HttpContext.Items["Player"];
            return Ok(_playerService.AllGuns(player.Name).Where(c => c.Unlocked == true));
        }

        [Authorize]
        [HttpGet("paygun/{gunname}")]
        public IActionResult PayGun( string gunname)
        {
            Player player = (Player)HttpContext.Items["Player"];
            //return Ok(_playerService.AllGuns(player.Name).Where(c => c.Unlocked == true));
            return Ok(new { message = "Template for pay gun " + gunname});
        }

        [Authorize]
        [HttpGet("lvlupgun/{gunname}")]
        public IActionResult LvlUpGum(string gunname)
        {
            Player player = (Player)HttpContext.Items["Player"];
            //return Ok(_playerService.AllGuns(player.Name).Where(c => c.Unlocked == true));
            return Ok(new { message = "Template for level up gun " + gunname });
        }

        [Authorize]
        [HttpGet("paycharacter/{charactername}")]
        public IActionResult PayCharacter(string charactername)
        {
            Player player = (Player)HttpContext.Items["Player"];
            //return Ok(_playerService.AllGuns(player.Name).Where(c => c.Unlocked == true));
            return Ok(new { message = "Template for pay character " + charactername });
        }


    }
}
