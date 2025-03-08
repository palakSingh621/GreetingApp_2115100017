using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using RepositoryLayer.Interface;
using RepositoryLayer.Entity;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }
        public string GetGreetingsBL(string firstName, string lastName)
        {
            return _greetingRL.GetGreetingsRL(firstName,lastName);
        }
        public void SaveGreeting(int userId,string message)
        {
            _greetingRL.SaveGreeting(userId,message);
        }

        public List<GreetingMessageEntity> GetAllGreetings(int userId)
        {
            return _greetingRL.GetAllGreetings(userId);
        }
        public GreetingMessageEntity GetGreetingById(int userId,int id)
        {
            return _greetingRL.GetGreetingById(userId,id);
        }
        public bool UpdateGreeting(int userId, int id, string newMessage)
        {
            return _greetingRL.UpdateGreeting(userId, id, newMessage);
        }
        public bool DeleteGreeting(int userId, int id)
        {
            return _greetingRL.DeleteGreeting(userId, id);
        }
    }
}
