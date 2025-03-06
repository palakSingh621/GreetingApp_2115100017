using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        ResponseModel<string> RegisterUser(RegisterRequest model);
        ResponseModel<string> LoginUser(LoginRequest model);
        ResponseModel<string> ForgotPassword(ForgetPasswordRequest model);
        ResponseModel<string> ResetPassword(ResetPasswordRequest model);
    }
}
