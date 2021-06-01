using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HackCheck.Data;

namespace HackCheck.Business
{
    public class LoginContainer
    {
        public bool ValidateLogin(LoginViewModel loginViewModel)
        {
            LoginRepository repo = new LoginRepository();
            return repo.ValidateLogin(new LoginDTO { Username = loginViewModel.Username, Password = loginViewModel.Password });
        }

        public bool VerifyLoginData(LoginViewModel loginViewModel) 
        {
            LoginRepository repo = new LoginRepository();
            return repo.VerifyLoginData(new LoginDTO { Username = loginViewModel.Username, Password = loginViewModel.Password });    
        }

        public int GetUserId(LoginViewModel loginViewModel)
        {
            LoginRepository repo = new LoginRepository();
            return repo.GetUserId(new LoginDTO { Username = loginViewModel.Username});
        }
    }
}
