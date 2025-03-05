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
        GreetingMessageEntity GetGreetingById(int id);
        string GetGreetingsRL(string firstName, string lastName);
        void SaveGreeting(string message);
        List<GreetingMessageEntity> GetAllGreetings();
    }
}
