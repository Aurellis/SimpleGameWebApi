using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace TestTaskGame
{
    public class JwtMid
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMid(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IPlayerService playerService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                attachPlayerToContext(context, playerService, token);
            }

            await _next(context);
        }

        private void attachPlayerToContext (HttpContext context, IPlayerService playerService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var playerName = jwtToken.Claims.First(x => x.Type == "name").Value;

                context.Items["Player"] = playerService.GetByName(playerName);
            }
            catch 
            {
            }
        }
    }
}