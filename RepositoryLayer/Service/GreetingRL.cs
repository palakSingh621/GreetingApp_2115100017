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

        public bool UpdateGreeting(int id, string newMessage)
        {
            var greeting = _context.GreetingMessages.FirstOrDefault(gre => gre.Id == id);
            if(greeting ==null)
            {
                return false;
            }
            greeting.Message = newMessage;
            _context.SaveChanges();
            return true;
        }
        public bool DeleteGreeting(int id)
        {
            var greeting = _context.GreetingMessages.Find(id);
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
