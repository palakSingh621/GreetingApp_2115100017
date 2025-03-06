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
            try
            {
                _logger.LogInformation("API Endpoint Hit");
                return Ok(new ResponseModel<string> { Success = true, Message = "API Endpoint Hit", Data = "Hello, World!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get method");
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// Post Method to send the greeting message
        /// </summary>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            try
            {
                greetings[requestModel.key] = requestModel.value;
                _logger.LogInformation("Request Received Successfully!");
                return Ok(new ResponseModel<string> { Success = true, Message = "Request Received Successfully!", Data = $"Key: {requestModel.key}, Value: {requestModel.value}" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Post method");
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// Put method to update an existing greeting
        /// </summary>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            try
            {
                if (greetings.ContainsKey(requestModel.key))
                {
                    greetings[requestModel.key] = requestModel.value;
                    _logger.LogInformation("Greeting Updated Successfully!");
                    return Ok(new ResponseModel<string> { Success = true, Message = "Greeting Updated Successfully!", Data = requestModel.value });
                }
                return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Put method");
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// Delete method to remove a greeting
        /// </summary>
        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            try
            {
                if (greetings.ContainsKey(key))
                {
                    greetings.Remove(key);
                    _logger.LogInformation("Greeting Deleted Successfully!");
                    return Ok(new ResponseModel<string> { Success = true, Message = "Greeting Deleted Successfully!", Data = key });
                }
                return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Delete method");
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// Patch method to Appending part of a greeting
        /// </summary>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            try
            {
                if (greetings.ContainsKey(requestModel.key))
                {
                    greetings[requestModel.key] = $"{greetings[requestModel.key]} {requestModel.value}";

                    var response = new ResponseModel<string>
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
            catch (Exception ex)
            {
                _logger.LogError($"Error in Patch Method: {ex.Message}");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal Server Error" });
            }
        }

        //UC2
        /// <summary>
        /// Get Method to print Hello World
        /// </summary>
        [HttpGet]
        public IActionResult GetGreetingForName(string firstName, string lastName)
        {
            try
            {
                _logger.LogInformation($"Received request to get greeting for: {firstName} {lastName}");

                string greetingMessage = _greetingBL.GetGreetingsBL(firstName, lastName);

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = greetingMessage,
                    Data = greetingMessage
                };

                _logger.LogInformation($"Greeting generated: {greetingMessage}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetGreetingForName: {ex.Message}");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal Server Error" });
            }
        }
        //UC4
        /// <summary>
        /// Save Greeting Message
        /// </summary>
        [HttpPost]
        [Route("saveGreeting")]
        public IActionResult SaveGreeting([FromBody] string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    _logger.LogWarning("Attempt to save an empty greeting message.");
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Message cannot be empty!" });
                }

                _greetingBL.SaveGreeting(message);
                _logger.LogInformation($"Saved greeting message: {message}");

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting saved successfully",
                    Data = $"Saved Message: {message}"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SaveGreeting: {ex.Message}");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal Server Error" });
            }
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
            try
            {
                var greeting = _greetingBL.GetGreetingById(id);
                if (greeting == null)
                    return NotFound(new ResponseModel<GreetingMessageEntity> { Success = false, Message = "Greeting not found" });

                return Ok(new ResponseModel<GreetingMessageEntity> { Success = true, Message = "Greeting found", Data = greeting });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetGreetingById method");
                return StatusCode(500, "Internal Server Error");
            }
        }
        //UC6
        /// <summary>
        /// Retrieve All Greetings
        /// </summary>
        [HttpGet]
        [Route("getAllGreetings")]
        public IActionResult GetAllGreetings()
        {
            try
            {
                _logger.LogInformation("Fetching all greeting messages...");

                var greetings = _greetingBL.GetAllGreetings();

                if (greetings == null || greetings.Count == 0)
                {
                    _logger.LogWarning("No greeting messages found.");
                    return NotFound(new ResponseModel<List<GreetingMessageEntity>>
                    {
                        Success = false,
                        Message = "No greeting messages found."
                    });
                }

                var response = new ResponseModel<List<GreetingMessageEntity>>
                {
                    Success = true,
                    Message = "Greeting messages fetched successfully!",
                    Data = greetings
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllGreetings: {ex.Message}");
                return StatusCode(500, new ResponseModel<List<GreetingMessageEntity>>
                {
                    Success = false,
                    Message = "Internal Server Error"
                });
            }
        }
        //UC7
        /// <summary>
        /// Update Greeting Message By id
        /// </summary>
        [HttpPut]
        [Route("updateGreeting/{id}")]
        public IActionResult UpdateGreeting(int id, [FromBody]string newMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newMessage))
                {
                    _logger.LogWarning($"Invalid update attempt for greeting ID {id} with an empty message.");
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "New message cannot be empty."
                    });
                }

                _logger.LogInformation($"Attempting to update greeting with ID: {id}");

                bool isUpdated = _greetingBL.UpdateGreeting(id, newMessage);

                if (!isUpdated)
                {
                    _logger.LogWarning($"Greeting with ID {id} not found.");
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = $"Greeting with ID {id} not found."
                    });
                }

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = $"Greeting with ID {id} updated successfully.",
                    Data = newMessage
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateGreeting: {ex.Message}");
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Internal Server Error"
                });
            }
        }
        //UC8
        /// <summary>
        /// Deleting Greeting message By ID
        /// </summary>
        [HttpDelete]
        [Route("deleteGreeting/{id}")]
        public IActionResult DeleteGreeting(int id)
        {
            try
            {
                _logger.LogInformation($"Attempting to delete greeting with ID: {id}");

                bool isDeleted = _greetingBL.DeleteGreeting(id);

                if (!isDeleted)
                {
                    _logger.LogWarning($"Greeting message with ID {id} not found.");
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Greeting not found."
                    });
                }

                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting deleted successfully.",
                    Data = $"Deleted Greeting ID: {id}"
                };

                _logger.LogInformation($"Greeting message with ID {id} deleted successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteGreeting: {ex.Message}");
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Internal Server Error"
                });
            }
        }
    }
}
