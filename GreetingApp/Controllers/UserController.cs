using Microsoft.AspNetCore.Mvc;
using BusinessLayer.Interface;
using ModelLayer.Model;
using GreetingApp.Helper;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GreetingApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly JwtTokenHelper _jwtHelper;
        private readonly IUserBL _userBL;
        private readonly IEmailService _emailService;
        ResponseModel<string> response;

        public UserController(ILogger<UserController> logger,IUserBL userBL,JwtTokenHelper jwtTokenHelper, IEmailService emailService)
        {
            _logger= logger;
            _userBL = userBL;
            _jwtHelper = jwtTokenHelper;
            _emailService = emailService;
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
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordRequest model)
        {
            _logger.LogInformation("ForgotPassword request received with email: {Email}", model.Email);
            response = new ResponseModel<string>();
            try
            {
                if(string.IsNullOrWhiteSpace(model.Email))
                {
                    return BadRequest(new { success = false, message = "Email is required" });
                }
                var user = _userBL.GetUserByEmail(model.Email);
                if (user != null)
                {
                    string token = _userBL.GenerateResetToken(user.Id, user.Email);
                    await _emailService.SendResetEmail(user.Email, token);
                    response.Success = true;
                    response.Message = "Reset password link sent to email";
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "User not found";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Process Failed");
                return BadRequest(new { Success = false, Message = "Process Failed", Error = ex.Message });
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public IActionResult ResetPassword([FromQuery] string token, [FromBody] ResetPasswordDTO model)
        {
            _logger.LogInformation("Resetting Password...");
            response= new ResponseModel<string>();
            try
            {
                var user = _userBL.ResetPassword(token, model);
                if ( user!=null)
                {
                    response.Success = true;
                    response.Message = "Password reset successful";
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Invalid or expired token";
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Process Failed");
                return BadRequest(new { Success = false, Message = "Process Failed", Error = ex.Message });
            }
        }
    }
}
