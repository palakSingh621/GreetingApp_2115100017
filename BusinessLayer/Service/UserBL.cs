using ModelLayer.Model;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using RepositoryLayer.Hashing;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRepository;

        public UserBL(IUserRL userRepository)
        {
            _userRepository = userRepository;
        }

        public UserModel RegisterUser(RegisterRequest model)
        {
            if (_userRepository.UserExists(model.Email))
                return null;
            string hashedPassword = HashingHelper.HashPassword(model.Password);
            var user=_userRepository.CreateUser(model.UserName, model.Email, hashedPassword);
            return user;
        }

        public UserModel LoginUser(LoginRequest model)
        {
            var user = _userRepository.GetUserByEmail(model.Email);
            if (user == null || !HashingHelper.VerifyPassword(model.Password, user.PasswordHash))
                return null;
            return user;
        }

        public string GenerateResetToken(int userId, string email)
        {
            return _userRepository.GenerateResetToken(userId, email);
        }
        public UserModel GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }
        public UserModel ResetPassword(string token,ResetPasswordDTO model)
        {
            int userId=_userRepository.ResetPassword(token, model);
            var user = _userRepository.GetUserById(userId);
            if (user != null)
            {
                string hashedPassword = HashingHelper.HashPassword(model.NewPassword);
                user.PasswordHash = hashedPassword;
                if (_userRepository.UpdateUserPassword(user.Email, user.PasswordHash))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
