using ModelLayer.Model;
using RepositoryLayer.Interface;
using BusinessLayer.Interface;
using RepositoryLayer.Hashing;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRepository;

        public UserBL(IUserRL userRepository)
        {
            _userRepository = userRepository;
        }

        public ResponseModel<string> RegisterUser(RegisterRequest model)
        {
            if (_userRepository.UserExists(model.Email))
                return new ResponseModel<string> { Success = false, Message = "Email already exists" };

            string hashedPassword = HashingHelper.HashPassword(model.Password);
            _userRepository.CreateUser(model.UserName, model.Email, hashedPassword);

            return new ResponseModel<string> { Success = true, Message = "User registered successfully" };
        }

        public ResponseModel<string> LoginUser(LoginRequest model)
        {
            var user = _userRepository.GetUserByEmail(model.Email);
            if (user == null || !HashingHelper.VerifyPassword(model.Password, user.PasswordHash))
                return new ResponseModel<string> { Success = false, Message = "Invalid credentials" };

            return new ResponseModel<string> { Success = true, Message = "Login successful", Data = "JWT_TOKEN_HERE" };
        }

        public ResponseModel<string> ForgotPassword(ForgetPasswordRequest model)
        {
            // Logic to send a password reset link via email
            return new ResponseModel<string> { Success = true, Message = "Password reset link sent" };
        }

        public ResponseModel<string> ResetPassword(ResetPasswordRequest model)
        {
            string hashedPassword = HashingHelper.HashPassword(model.NewPassword);
            _userRepository.UpdateUserPassword(model.Email, hashedPassword);

            return new ResponseModel<string> { Success = true, Message = "Password reset successful" };
        }
    }

}
