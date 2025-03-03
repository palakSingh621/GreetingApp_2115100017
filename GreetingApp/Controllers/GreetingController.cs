using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
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
        public string GetGreeting()
        {
            _logger.LogInformation("Printing Hello World using Services Layers.");
            return _greetingBL.GetGreetingsBL();
        }

    }
}
