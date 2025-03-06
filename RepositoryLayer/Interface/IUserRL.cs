using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        bool UserExists(string email);
        void CreateUser(string username, string email, string passwordHash);
        UserModel GetUserByEmail(string email);
        void UpdateUserPassword(string email, string newPasswordHash);
    }
}
