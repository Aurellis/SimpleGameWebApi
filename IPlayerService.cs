using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestTaskGame
{
    public interface IPlayerService
    {
        JsonResult Register(AutentificateRequest request);
        AutentificateResponse Autentificate(AutentificateRequest request);        
        Player GetByName(string name);
        List<Gun> AllGuns(string name);
        List<Character> AllCharacters(string name);
        bool PayItem(string playerName, string itemName);
        bool UpGun(string playerName, string itemName);
    }

    public class PlayerService : IPlayerService
    {  
        private readonly AppSettings _appSettings;
        private readonly ISQLService _sqlService;

        public PlayerService(IOptions<AppSettings> appSettings, ISQLService sqlService)
        {
            _appSettings = appSettings.Value;
            _sqlService = sqlService;
        }

        public JsonResult Register(AutentificateRequest request)
        {            
            if (!_sqlService.Register(new Player() { Name = request.Name, Password = request.Password }))
            {
                return null;
            }

            return new JsonResult(new { message = "Register complete" });

        }

        public AutentificateResponse Autentificate(AutentificateRequest request)
        {
            Player player = _sqlService.GetPlayer( new  Player() {Name = request.Name, Password = request.Password });

            if (player == null)
            {
                return null;
            }
            var token = GenToken(player);

            return new AutentificateResponse(player, token);
        }

        public Player GetByName(string name)
        {
            return _sqlService.GetPlayer(new Player() { Name = name});
        }

        public List<Gun> AllGuns(string name)
        {
            return _sqlService.GetGuns(name);
        }

        public List<Character> AllCharacters(string name)
        {
            return _sqlService.GetCharacters(name);
        }

        public bool PayItem(string playerName, string itemName)
        {
            return _sqlService.PayItem(playerName, itemName);
        }

        public bool UpGun(string playerName, string itemName)
        {
            return _sqlService.UpItem(playerName, itemName);
        }

        private string GenToken(Player player)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("name", player.Name) }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }        
    }
}