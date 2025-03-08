using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        UserEntity RegisterUser(RegisterRequest model);
        UserEntity LoginUser(LoginRequest model);
        string GenerateResetToken(int userId, string email);
        UserEntity GetUserByEmail(string email);
        UserEntity ResetPassword(string token, ResetPasswordDTO model);
    }
}
