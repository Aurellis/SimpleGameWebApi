using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Linq;

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
        [HttpGet("pay/gun/{gunname}")]
        public IActionResult PayGun( string gunname)
        {
            Player player = (Player)HttpContext.Items["Player"];

            Player playerTek = _playerService.GetByName(player.Name);

            if (_playerService.AllGuns(player.Name).Where(c => c.Name == gunname  && c.Unlocked == false && c.Price < playerTek.Coins).Count() != 0 )
            {
                if (_playerService.PayItem(player.Name, gunname))
                {
                    return Ok(new { message = "Payed" });
                }  
            }
            return Ok(new { message = "Failed pay" });            
        }

        [Authorize]
        [HttpGet("pay/gun/up/{gunname}")]
        public IActionResult LvlUpGum(string gunname)
        {
            Player player = (Player)HttpContext.Items["Player"];
            Player playerTek = _playerService.GetByName(player.Name);

            if (_playerService.AllGuns(player.Name).Where(c => c.Name == gunname && c.Unlocked == true && c.Price < playerTek.Coins ).Count() != 0)
            {
                if (_playerService.UpGun(playerTek.Name, gunname))
                {
                    return Ok(new { message = "Level Up" });
                }                
            }

            return Ok(new { message = "Failed Level Up" });

        }

        [Authorize]
        [HttpGet("pay/character/{charactername}")]
        public IActionResult PayCharacter(string charactername)
        {
            Player player = (Player)HttpContext.Items["Player"];

            Player playerTek = _playerService.GetByName(player.Name);

            if (_playerService.AllCharacters(player.Name).Where(c => c.Name == charactername && c.Unlocked == false && c.Price < playerTek.Coins).Count() != 0)
            {
                if (_playerService.PayItem(player.Name, charactername))
                {
                    return Ok(new { message = "Payed" });
                }
            }

            return Ok(new { message = "Failed pay" });
        }

    }
}
