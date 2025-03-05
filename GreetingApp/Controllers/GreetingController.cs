using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;
namespace GreetingApp.Controllers
{
    /// <summary>
    /// Class Providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GreetingController : ControllerBase
    {
        private readonly ILogger<GreetingController> _logger;
        private readonly IGreetingBL _greetingBL;
        private static Dictionary<string, string> greetings = new Dictionary<string, string>();
        public GreetingController(ILogger<GreetingController> logger, IGreetingBL greetingBL)
        {
            _logger = logger;
            _greetingBL = greetingBL;
        }
        //UC1
        /// <summary>
        /// Get method to get the greeting message
        /// </summary>
        [HttpGet]
        [Route("Get")]
        public IActionResult Get()
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Success = true;
            response.Message = "API Endpoint Hit";
            _logger.LogInformation("API Endpoint Hit");
            response.Data = "Hello, World!";
            return Ok(response);
        }
        /// <summary>
        /// Post Method to send the greeting message
        /// </summary>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            greetings[requestModel.key] = requestModel.value;
            ResponseModel<string> response = new ResponseModel<string>();
            response.Success = true;
            response.Message = "Request Receives Successfully!";
            _logger.LogInformation("Request Receives Successfully!");
            response.Data = $"Key: {requestModel.key}, value:{requestModel.value}";
            return Ok(response);
        }
        /// <summary>
        /// Put method to update an existing greeting
        /// </summary>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            if (greetings.ContainsKey(requestModel.key))
            {
                greetings[requestModel.key] = requestModel.value;

                ResponseModel<string> response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting Updated Successfully!",
                    Data = $"Key: {requestModel.key}, Updated Value: {requestModel.value}"
                };

                _logger.LogInformation($"Greeting Updated: {requestModel.key} -> {requestModel.value}");
                return Ok(response);
            }
            _logger.LogWarning($"Greeting Key Not Found: {requestModel.key}");
            return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
        }
        /// <summary>
        /// Delete method to remove a greeting
        /// </summary>
        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            if (greetings.ContainsKey(key))
            {
                greetings.Remove(key);
                ResponseModel<string> response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting Deleted Successfully!",
                    Data = $"Key Deleted: {key}"
                };
                _logger.LogInformation($"Greeting Deleted: {key}");
                return Ok(response);
            }
            _logger.LogWarning($"Greeting Key Not Found: {key}");
            return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
        }
        /// <summary>
        /// Patch method to Appending part of a greeting
        /// </summary>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            if (greetings.ContainsKey(requestModel.key))
            {
                greetings[requestModel.key] = greetings[requestModel.key] + " " + requestModel.value;

                ResponseModel<string> response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting Modified Successfully!",
                    Data = $"Key: {requestModel.key}, Modified Value: {greetings[requestModel.key]}"
                };
                _logger.LogInformation($"Greeting Modified: {requestModel.key} -> {greetings[requestModel.key]}");
                return Ok(response);
            }
            _logger.LogWarning($"Greeting Key Not Found: {requestModel.key}");
            return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
        }

        //UC2
        /// <summary>
        /// Get Method to print Hello World
        /// </summary>
        [HttpGet]
        public IActionResult GetGreetingForName(string firstName, string LastName)
        {
            _logger.LogInformation("Printing Hello to the User fromervices Layers.");
            ResponseModel<string> response = new ResponseModel<string>
            {
                Success = true,
                Message = _greetingBL.GetGreetingsBL(firstName, LastName),
                Data = _greetingBL.GetGreetingsBL(firstName, LastName)
            };
            return Ok(response);
        }
        //UC4
        /// <summary>
        /// Save Greeting Message
        /// </summary>
        [HttpPost]
        [Route("saveGreeting")]
        public IActionResult SaveGreeting([FromBody] string message)
        {
            _greetingBL.SaveGreeting(message);
            _logger.LogInformation("Taking the Greetings Message to save.");
            ResponseModel<string> response = new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting saved successfully",
                Data = $"Saved Message: {message}"
            };
            return Ok(response);
        }

        //UC5
        /// <summary>
        /// Getting Greeting By ID
        /// </summary>
        /// </summary>
        [HttpGet]
        [Route("getGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
        {
            _logger.LogInformation($"Getting Greeting message by Id: {id}");
            var greeting = _greetingBL.GetGreetingById(id);
            ResponseModel<GreetingMessageEntity> response = new ResponseModel<GreetingMessageEntity>();
            if (greeting == null)
            {
                response.Success = false;
                response.Message = $"Greeting with ID {id} not found.";
                return NotFound(response);
            }
            response.Success = true;
            response.Message = $"Greeting with ID {id} found.";
            response.Data = greeting;
            return Ok(response);
        }
        //UC6
        /// <summary>
        /// Retrieve All Greetings
        /// </summary>
        [HttpGet]
        [Route("getAllGreetings")]
        public IActionResult GetAllGreetings()
        {
            _logger.LogInformation("Fetching Greetings Messages...");
            var greetings = _greetingBL.GetAllGreetings();
            ResponseModel<List<GreetingMessageEntity>> response = new ResponseModel<List<GreetingMessageEntity>>();
            if (greetings == null || greetings.Count == 0)
            {
                response.Success = false;
                response.Message = "No Greeting Messages Found";
                return NotFound("No greetings found.");
            }
             response.Success = true;
             response.Message = "Greeting Messages Fetched!!";
             response.Data = _greetingBL.GetAllGreetings();
            return Ok(response);
        }
        //UC7
        /// <summary>
        /// Update Greeting Message By id
        /// </summary>
        [HttpPut]
        [Route("updateGreeting/{id}")]
        public IActionResult UpdateGreeting(int id, [FromBody]string newMessage)
        {
            _logger.LogInformation($"Updating Greeting By id: {id}");
            ResponseModel<String> response = new ResponseModel<String>();
            bool isUpdated=_greetingBL.UpdateGreeting(id, newMessage);
            if(!isUpdated)
            {
                response.Success = false;
                response.Message = $"Greeting with ID {id} not found.";
                return NotFound(response);
            }
            response.Success = true;
            response.Message = $"Greeting with ID {id} updated successfully.";
            response.Data= newMessage;
            return Ok(response);
        }
        //UC8
        /// <summary>
        /// Deleting Greeting message By ID
        /// </summary>
        [HttpDelete]
        [Route("deleteGreeting/{id}")]
        public IActionResult DeleteGreeting(int id)
        {
            bool isDeleted = _greetingBL.DeleteGreeting(id);
            if (isDeleted)
            {
                _logger.LogInformation($"Greeting message with ID {id} deleted successfully.");
                ResponseModel<string> response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting deleted successfully",
                    Data = $"Deleted Greeting ID: {id}"
                };
                return Ok(response);
            }
            else
            {
                _logger.LogWarning($"Greeting message with ID {id} not found.");
                return NotFound(new { Success = false, Message = "Greeting not found" });
            }
        }
    }
}
