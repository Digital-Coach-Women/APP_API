using System;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Coaching.Helper
{
    public static class TokenHelper
    {
        public static string GenerateJwtToken(string id)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("ABCDEFGHIJKLMOPQRSTUVXYZ1234567890"); //_jwtConfig.Secret

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", id),
                    //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMonths(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        public static int GetUserId(string token)
        {
            //decrypt token
            var tokenHandler = new JwtSecurityTokenHandler();

            //read
            var tokenVal = tokenHandler.ReadJwtToken(token);
            var idFromToken = tokenVal.Claims.FirstOrDefault(x => x.Type == "Id").Value;

            if (idFromToken is null)
                return 0;

            _ = int.TryParse(idFromToken, out var userId);
            return userId;
        }
    }
}
