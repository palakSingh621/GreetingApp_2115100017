using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using RepositoryLayer.Entity; 
using RepositoryLayer.Context;
using RepositoryLayer.Interface;

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

        public GreetingMessageEntity GetGreetingById(int id)
        {
            return _context.GreetingMessages.FirstOrDefault(g => g.Id == id);
        }

        public void SaveGreeting(string message)
        {
            var greeting = new GreetingMessageEntity { Message = message };
            _context.GreetingMessages.Add(greeting);
            _context.SaveChanges();
        }

        public List<GreetingMessageEntity> GetAllGreetings()
        {
            return _context.GreetingMessages.ToList();
        }
    }
}
