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
        UserModel RegisterUser(RegisterRequest model);
        UserModel LoginUser(LoginRequest model);
        ResponseModel<string> ForgotPassword(ForgetPasswordRequest model);
        ResponseModel<string> ResetPassword(ResetPasswordRequest model);
    }
}
