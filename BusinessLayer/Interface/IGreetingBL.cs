using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreetingsBL(string firstName, string lastName);
        void SaveGreeting(int userId, string message);
        List<GreetingMessageEntity> GetAllGreetings(int userId);
        GreetingMessageEntity GetGreetingById(int userId, int id);
        bool UpdateGreeting(int userId, int id, string newMessage);
        bool DeleteGreeting(int userId, int id);
    }
}
