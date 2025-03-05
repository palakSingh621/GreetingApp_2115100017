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
        void SaveGreeting(string message);
        List<GreetingMessageEntity> GetAllGreetings();
        GreetingMessageEntity GetGreetingById(int id);
        bool UpdateGreeting(int id, string newMessage);
        bool DeleteGreeting(int id);
    }
}
