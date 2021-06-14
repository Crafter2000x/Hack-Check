using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCheck.Data
{
    public class LoginRepository
    {
        private ILoginContext context;

        public LoginRepository(ILoginContext context)
        {
            this.context = context;
        }

        public LoginRepository(IConfiguration _Configuration)
        {
            this.context = new LoginMSSQLContext(_Configuration);
        }

        public bool ValidateLogin(LoginDTO loginDTO) 
        {
            return context.ValidateLogin(loginDTO);
        }

        public virtual bool VerifyLoginData(LoginDTO loginDTO) 
        {
            return context.VerifyLoginData(loginDTO);
        }

        public virtual int GetUserId(LoginDTO loginDTO) 
        {
            return context.GetUserId(loginDTO);
        }
    }
}
