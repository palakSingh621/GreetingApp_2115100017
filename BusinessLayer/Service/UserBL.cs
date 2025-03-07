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

        public UserModel RegisterUser(RegisterRequest model)
        {
            if (_userRepository.UserExists(model.Email))
                return null;
            string hashedPassword = HashingHelper.HashPassword(model.Password);
            var user=_userRepository.CreateUser(model.UserName, model.Email, hashedPassword);
            return user;
        }

        public UserModel LoginUser(LoginRequest model)
        {
            var user = _userRepository.GetUserByEmail(model.Email);
            if (user == null || !HashingHelper.VerifyPassword(model.Password, user.PasswordHash))
                return null;
            return user;
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
