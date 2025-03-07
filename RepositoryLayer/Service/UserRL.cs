using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly UserDbContext _context;

        public UserRL(UserDbContext context)
        {
            _context = context;
        }

        public bool UserExists(string email) => _context.Users.Any(u => u.Email ==email);

        public UserModel CreateUser(string username, string email, string passwordHash)
        {
            var user = new UserModel { UserName= username, Email = email, PasswordHash = passwordHash };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
        public UserModel GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void UpdateUserPassword(string email, string newPasswordHash)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.PasswordHash = newPasswordHash;
                _context.SaveChanges();
            }
        }
    }
}
