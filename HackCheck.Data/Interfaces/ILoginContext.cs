using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCheck.Data
{
    public interface ILoginContext
    {
        bool ValidateLogin(LoginDTO loginDTO);
        bool VerifyLoginData(LoginDTO loginDTO);
        int GetUserId(LoginDTO loginDTO);
    }
}