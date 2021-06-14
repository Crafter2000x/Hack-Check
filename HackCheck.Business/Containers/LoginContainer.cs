using HackCheck.Data;
using Microsoft.Extensions.Configuration;

namespace HackCheck.Business
{
    public class LoginContainer
    {
        private LoginRepository Repo;

        public LoginContainer(LoginRepository _Repo)
        {
            Repo = _Repo;
        }

        public LoginContainer(IConfiguration _Configuration)
        {
            Repo = new LoginRepository(_Configuration);
        }
        public bool ValidateLogin(LoginViewModel loginViewModel)
        {
            return Repo.ValidateLogin(new LoginDTO { Username = loginViewModel.Username, Password = loginViewModel.Password });
        }

        public bool VerifyLoginData(LoginViewModel loginViewModel) 
        {
            return Repo.VerifyLoginData(new LoginDTO { Username = loginViewModel.Username, Password = loginViewModel.Password });    
        }

        public int GetUserId(LoginViewModel loginViewModel)
        {
            return Repo.GetUserId(new LoginDTO { Username = loginViewModel.Username});
        }
    }
}
