using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using ModelLayer.Model;
using GreetingApp.Helper;

namespace GreetingApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly JwtTokenHelper _jwtHelper;
        private readonly IUserBL _userBL;
        ResponseModel<string> response;

        public UserController(ILogger<UserController> logger,IUserBL userBL,JwtTokenHelper jwtTokenHelper)
        {
            _logger= logger;
            _userBL = userBL;
            _jwtHelper = jwtTokenHelper;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            _logger.LogInformation("User Register successfully..");
            try
            {
                response = new ResponseModel<string>();
                var user = _userBL.RegisterUser(model);
                if (user != null)
                {
                    response.Success = true;
                    response.Message = "User Registered Successfully!";
                    var token = _jwtHelper.GenerateToken(user);
                    response.Data = model.ToString();
                    _logger.LogInformation("User Registered successfully.");
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "User with this email already exists!";
                _logger.LogWarning("User registration failed: Email already exists.");
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user.");
                return StatusCode(500, new { Success = false, Message = "Internal Server Error", Error = ex.Message });
            }  
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            try
            {
                _logger.LogInformation("Login attemp for user: {0}", model.Email);
                var user = _userBL.LoginUser(model);
                if (user == null)
                {
                    _logger.LogWarning("Invalid login attempt for user: {0}", model.Email);
                    return Unauthorized(new { Success = false, Message = "Invalid username or password." });
                }
                var token = _jwtHelper.GenerateToken(user);
                _logger.LogInformation("User {0} logged in successfully.", model.Email);
                return Ok(new { Success = true, Message = "Login Successful.", Token = token });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Login failed.");
                return BadRequest(new { Success = false, Message = "Login failed.", Error = ex.Message });
            }
        }

        [HttpPost]
        [Route("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgetPasswordRequest model)
        {
            var response = _userBL.ForgotPassword(model);
            return Ok(response);
        }

        [HttpPost]
        [Route("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest model)
        {
            var response = _userBL.ResetPassword(model);
            return Ok(response);
        }
    }
}
