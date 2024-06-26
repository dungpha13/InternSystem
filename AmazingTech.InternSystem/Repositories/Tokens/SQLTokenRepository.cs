﻿using AmazingTech.InternSystem.Data;
using AmazingTech.InternSystem.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace swp391_be.API.Repositories.Tokens
{
    public class SQLTokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext dbContext;

        public SQLTokenRepository(IConfiguration _configuration, AppDbContext dbContext, UserManager<User> _userManager)
        {
            this._configuration = _configuration;
            this.dbContext = dbContext;
            this._userManager = _userManager;
        }

        public string CreateJwtToken(User user, List<string> roles)
        {
            //var claims = new List<Claim>();
            //claims.Add(new Claim(ClaimTypes.Sid, user.Id));
            //claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var token = new JwtSecurityToken(
            //    _configuration["Jwt:Issuer"],
            //    _configuration["Jwt:Audience"],
            //    claims,
            //    null,
            //    DateTime.Now.AddMinutes(15),
            //    signingCredentials: credentials);

            //return new JwtSecurityTokenHandler().WriteToken(token);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("de455d3d7f83bf393eea5aef43f474f4aac57e3e8d75f9118e60d526453002dc");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("username", user.UserName),
                    new Claim(ClaimTypes.Role, roles[0]),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<InvalidToken> InvalidTokens = new List<InvalidToken>();

        public static void InvalidateToken(string token)
        {
            InvalidTokens.Add(new InvalidToken { Token = token, InvalidatedAt = DateTime.UtcNow });
        }

        public static bool IsTokenValid(string token)
        {
            // Clear expired tokens before checking validity
            ClearExpiredTokens();

            return !InvalidTokens.Any(t => t.Token == token);
        }

        private static void ClearExpiredTokens()
        {
            var expirationThreshold = DateTime.UtcNow.AddHours(-1); // Set your expiration duration

            InvalidTokens.RemoveAll(t => t.InvalidatedAt < expirationThreshold);
        }

        private class InvalidToken
        {
            public string Token { get; set; }
            public DateTime InvalidatedAt { get; set; }
        }
    }
}
