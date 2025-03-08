using System.Security.Claims;
using BusinessLayer.Interface;
using CacheLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;
namespace GreetingApp.Controllers
{
    /// <summary>
    /// Class Providing API for HelloGreeting
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GreetingController : ControllerBase
    {
        private readonly ILogger<GreetingController> _logger;
        private readonly IGreetingBL _greetingBL;
        private readonly IRedisCacheService _redisCacheService;
        private static Dictionary<string, string> greetings = new Dictionary<string, string>();
        public GreetingController(ILogger<GreetingController> logger, IGreetingBL greetingBL, IRedisCacheService redisCacheService)
        {
            _logger = logger;
            _greetingBL = greetingBL;
            _redisCacheService = redisCacheService;
        }
        private int GetUserIdFromToken()
        {
            var userId = User.FindFirstValue("userId");
            if (userId == null)
            {
                _logger.LogWarning("Invalid or missing user ID in token.");
                throw new UnauthorizedAccessException(userId);
            }
            return int.Parse(userId);
                //if (User == null || !User.Identity.IsAuthenticated)
                //{
                //    _logger.LogWarning("User is not authenticated.");
                //    throw new UnauthorizedAccessException("User is not authenticated.");
                //}
                //foreach (var claim in User.Claims)
                //{
                //    _logger.LogInformation($"Claim Type: {claim.Type}, Value: {claim.Value}");
                //}
                //var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                //if (string.IsNullOrEmpty(userIdClaim))
                //{
                //    _logger.LogWarning("Invalid or missing user ID in token.");
                //    throw new UnauthorizedAccessException(userIdClaim);
                //}
                //return int.Parse(userIdClaim);
            }
            ////UC1
            ///// <summary>
            ///// Get method to get the greeting message
            ///// </summary>
            //[HttpGet]
            //[Route("Get")]
            //public IActionResult Get()
            //{
            //    try
            //    {
            //        _logger.LogInformation("API Endpoint Hit");
            //        return Ok(new ResponseModel<string> { Success = true, Message = "API Endpoint Hit", Data = "Hello, World!" });
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "Error in Get method");
            //        return StatusCode(500, "Internal Server Error");
            //    }
            //}
            ///// <summary>
            ///// Post Method to send the greeting message
            ///// </summary>
            //[HttpPost]
            //public IActionResult Post(RequestModel requestModel)
            //{
            //    try
            //    {
            //        greetings[requestModel.key] = requestModel.value;
            //        _logger.LogInformation("Request Received Successfully!");
            //        return Ok(new ResponseModel<string> { Success = true, Message = "Request Received Successfully!", Data = $"Key: {requestModel.key}, Value: {requestModel.value}" });
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "Error in Post method");
            //        return StatusCode(500, "Internal Server Error");
            //    }
            //}
            ///// <summary>
            ///// Put method to update an existing greeting
            ///// </summary>
            //[HttpPut]
            //public IActionResult Put(RequestModel requestModel)
            //{
            //    try
            //    {
            //        if (greetings.ContainsKey(requestModel.key))
            //        {
            //            greetings[requestModel.key] = requestModel.value;
            //            _logger.LogInformation("Greeting Updated Successfully!");
            //            return Ok(new ResponseModel<string> { Success = true, Message = "Greeting Updated Successfully!", Data = requestModel.value });
            //        }
            //        return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "Error in Put method");
            //        return StatusCode(500, "Internal Server Error");
            //    }
            //}
            ///// <summary>
            ///// Delete method to remove a greeting
            ///// </summary>
            //[HttpDelete("{key}")]
            //public IActionResult Delete(string key)
            //{
            //    try
            //    {
            //        if (greetings.ContainsKey(key))
            //        {
            //            greetings.Remove(key);
            //            _logger.LogInformation("Greeting Deleted Successfully!");
            //            return Ok(new ResponseModel<string> { Success = true, Message = "Greeting Deleted Successfully!", Data = key });
            //        }
            //        return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "Error in Delete method");
            //        return StatusCode(500, "Internal Server Error");
            //    }
            //}
            ///// <summary>
            ///// Patch method to Appending part of a greeting
            ///// </summary>
            //[HttpPatch]
            //public IActionResult Patch(RequestModel requestModel)
            //{
            //    try
            //    {
            //        if (greetings.ContainsKey(requestModel.key))
            //        {
            //            greetings[requestModel.key] = $"{greetings[requestModel.key]} {requestModel.value}";

            //            var response = new ResponseModel<string>
            //            {
            //                Success = true,
            //                Message = "Greeting Modified Successfully!",
            //                Data = $"Key: {requestModel.key}, Modified Value: {greetings[requestModel.key]}"
            //            };

            //            _logger.LogInformation($"Greeting Modified: {requestModel.key} -> {greetings[requestModel.key]}");
            //            return Ok(response);
            //        }

            //        _logger.LogWarning($"Greeting Key Not Found: {requestModel.key}");
            //        return NotFound(new ResponseModel<string> { Success = false, Message = "Key not found!" });
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError($"Error in Patch Method: {ex.Message}");
            //        return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal Server Error" });
            //    }
            //}

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
        [Authorize]
        [HttpPost]
        [Route("saveGreeting")]
        public IActionResult SaveGreeting([FromBody] string message)
        {
            try
            {
                int userId = GetUserIdFromToken();
                _logger.LogInformation($"User ID: {userId} is trying to save a message.");

                if (string.IsNullOrWhiteSpace(message))
                {
                    _logger.LogWarning("Attempt to save an empty greeting message.");
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Message cannot be empty!" });
                }

                _greetingBL.SaveGreeting(userId, message);
                _logger.LogInformation($"Saved greeting message: {message}");

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting saved successfully",
                    Data = $"Saved Message: {message}"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"Unauthorized access: {ex.Message}");
                return Unauthorized(new ResponseModel<string> { Success = false, Message = "Unauthorized access." });
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
        public async Task<IActionResult> GetGreetingById(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                _logger.LogInformation($"Extracted User ID from token: {userId}");
                var cacheKey = $"greeting_{id}";
                var cachedGreeting = await _redisCacheService.GetCachedData<GreetingMessageEntity>(cacheKey);
                
                if (cachedGreeting != null)
                {
                    if (cachedGreeting.UserId != userId) // Ensure the cached greeting belongs to the user
                    {
                    _logger.LogWarning($"Unauthorized access attempt by UserId {userId} for Greeting ID {id}");
                    return Unauthorized(new ResponseModel<string> { Success = false, Message = "Unauthorized access" });
                    }
                    _logger.LogInformation($"Cache hit for greeting ID: {id}");
                    return Ok(new ResponseModel<GreetingMessageEntity> { Success = true, Message = "Greeting found (from cache)", Data = cachedGreeting });
                }
                _logger.LogInformation($"Fetching greeting from DB for UserID: {userId}, GreetingID: {id}");
                var greeting = _greetingBL.GetGreetingById(userId,id);
                if (greeting == null)
                    return NotFound(new ResponseModel<GreetingMessageEntity> { Success = false, Message = "Greeting not found" });
                await _redisCacheService.SetCachedData(cacheKey, greeting, TimeSpan.FromMinutes(10));
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
        [Authorize]
        [HttpGet]
        [Route("getAllGreetings")]
        public async Task<IActionResult> GetAllGreetings()
        {
            try
            {
                int userId = GetUserIdFromToken();
                _logger.LogInformation("Checking cache for greeting messages...");

                // Check Redis Cache
                var cacheKey = $"greetings_user_{userId}";
                var cachedGreetings = await _redisCacheService.GetCachedData<List<GreetingMessageEntity>>(cacheKey);
                
                if (cachedGreetings != null)
                {
                    _logger.LogInformation("Returning greetings from cache.");
                    return Ok(new ResponseModel<List<GreetingMessageEntity>>
                    {
                        Success = true,
                        Message = $"Cache hit for user {userId} greetings",
                        Data = cachedGreetings
                    });
                }
                _logger.LogInformation("Fetching all greeting messages from Database...");

                var greetings = _greetingBL.GetAllGreetings(userId);

                if (greetings == null || greetings.Count == 0)
                {
                    _logger.LogWarning("No greeting messages found.");
                    return NotFound(new ResponseModel<List<GreetingMessageEntity>>
                    {
                        Success = false,
                        Message = "No greeting messages found."
                    });
                }
                // Store in Redis Cache for 10 minutes
                await _redisCacheService.SetCachedData("all_greetings", greetings, TimeSpan.FromMinutes(10));

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
        [Authorize]
        [HttpPut]
        [Route("updateGreeting/{id}")]
        public async Task<IActionResult> UpdateGreeting(int id, [FromBody]string newMessage)
        {
            int userId = GetUserIdFromToken();
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

                bool isUpdated = _greetingBL.UpdateGreeting(userId, id, newMessage);

                if (!isUpdated)
                {
                    _logger.LogWarning($"Greeting with ID {id} not found.");
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = $"Greeting with ID {id} not found."
                    });
                }
                // Remove Cache since data changed
                await _redisCacheService.RemoveCachedData("all_greetings");
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
        [Authorize]
        [HttpDelete]
        [Route("deleteGreeting/{id}")]
        public async Task<IActionResult> DeleteGreeting(int id)
        {
            int userId = GetUserIdFromToken();
            try
            {
                _logger.LogInformation($"Attempting to delete greeting with ID: {id}");

                bool isDeleted = _greetingBL.DeleteGreeting(userId, id);

                if (!isDeleted)
                {
                    _logger.LogWarning($"Greeting message with ID {id} not found.");
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Greeting not found."
                    });
                }
                // Remove Cache
                await _redisCacheService.RemoveCachedData("all_greetings");

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
