using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.interfaces.AuthInterfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _option;
        public JwtProvider(IOptions<JwtOptions> option)
        {
            _option = option.Value;
        }
        public Guid GenerateRefreshToken()
        {
            var newToken = Guid.NewGuid();
            return newToken;
        }
        public string GenerateAccessToken(User user)
        {
            Claim[] claims =
         [
             new("userId", user.Id.ToString()),
             new("timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
         ];

            var signingCreditials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_option.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCreditials,
                expires: DateTime.UtcNow.AddMinutes(_option.ExpiresMinutes)
                );

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}
