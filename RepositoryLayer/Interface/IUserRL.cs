using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        bool UserExists(string email);
        UserEntity CreateUser(string username, string email, string passwordHash);
        UserEntity GetUserByEmail(string email);
        UserEntity GetUserById(int userId);
        bool UpdateUserPassword(string email, string newPasswordHash);
        string GenerateResetToken(int userId, string email);
        int ResetPassword(string token, ResetPasswordDTO model);
    }
}
