using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using RepositoryLayer.Entity; 
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingAppContext _context;

        public GreetingRL(GreetingAppContext context)
        {
            _context = context;
        }

        public string GetGreetingsRL(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return $"Hello {firstName} {lastName}!";
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                return $"Hello {firstName}!";
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                return $"Hello {lastName}!";
            }
            else
            {
                return "Hello, World!";
            }
        }

        public GreetingMessageEntity GetGreetingById(int userId, int id)
        {
            return _context.GreetingMessages.FirstOrDefault(g => g.UserId == userId && g.Id == id);
        }

        public GreetingMessageEntity SaveGreeting(int userId, string message)
        {
            //// Validate user existence
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found. Please log in.");
            }
            var greeting = new GreetingMessageEntity
            {
                UserId = userId,
                Message = message
            };
            _context.GreetingMessages.Add(greeting);
            _context.SaveChanges();
            return greeting;
        }

        public List<GreetingMessageEntity> GetAllGreetings(int userId)
        {
            return _context.GreetingMessages.Where(g => g.UserId == userId).ToList();
        }

        public bool UpdateGreeting(int userId, int id, string newMessage)
        {
            var greeting = _context.GreetingMessages.FirstOrDefault(g => g.Id == id && g.UserId == userId);
            if(greeting ==null)
            {
                return false;
            }
            greeting.Message = newMessage;
            _context.SaveChanges();
            return true;
        }
        public bool DeleteGreeting(int userId,int id)
        {
            var greeting = _context.GreetingMessages.FirstOrDefault(g => g.Id == id && g.UserId == userId);
            if (greeting != null)
            {
                _context.GreetingMessages.Remove(greeting);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
