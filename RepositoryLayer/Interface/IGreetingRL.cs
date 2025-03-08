using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        GreetingMessageEntity GetGreetingById(int userId, int id);
        string GetGreetingsRL(string firstName, string lastName);
        GreetingMessageEntity SaveGreeting(int userId, string message);
        List<GreetingMessageEntity> GetAllGreetings(int userId);
        bool UpdateGreeting(int userId, int id, string newMessage);
        bool DeleteGreeting(int userId, int id);
    }
}
