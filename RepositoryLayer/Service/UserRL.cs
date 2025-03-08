using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly IConfiguration _config;
        private readonly GreetingAppContext _context;

        public UserRL(IConfiguration config, GreetingAppContext context)
        {
            _config = config;
            _context = context;
        }

        public bool UserExists(string email) => _context.Users.Any(u => u.Email ==email);

        public UserEntity CreateUser(string username, string email, string passwordHash)
        {
            var user = new UserEntity { UserName= username, Email = email, PasswordHash = passwordHash };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        public UserEntity GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public UserEntity GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(user => user.Id == userId);
        }
        public bool UpdateUserPassword(string email, string newPasswordHash)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public string GenerateResetToken(int userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:ResetSecret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("userId", userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: "GreetingApp",
                audience: "GreetingAppUser",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token expires in 1 hour
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public int ResetPassword(string token,ResetPasswordDTO model)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:ResetSecret"]));
            var handler = new JwtSecurityTokenHandler();
            var claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "GreetingApp",
                ValidAudience = "GreetingAppUser",
                IssuerSigningKey = key
            }, 
            out SecurityToken validatedToken);
            // Extract userId from claims
            var userIdClaim = claimsPrincipal.FindFirst("userId")?.Value;

            if (userIdClaim == null)
            {
                throw new SecurityTokenException("Invalid token: userId claim missing.");
            }
            return int.Parse(userIdClaim);
        }
    }
}
