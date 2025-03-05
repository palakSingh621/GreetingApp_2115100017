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
        public void SaveGreeting(string message)
        {
            _greetingRL.SaveGreeting(message);
        }

        public List<GreetingMessageEntity> GetAllGreetings()
        {
            return _greetingRL.GetAllGreetings();
        }
        public GreetingMessageEntity GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }
        public bool UpdateGreeting(int id, string newMessage)
        {
            return _greetingRL.UpdateGreeting(id, newMessage);
        }
    }
}
