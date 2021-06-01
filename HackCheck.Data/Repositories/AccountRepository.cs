using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCheck.Data
{
    public class AccountRepository
    {
        private IAccountContext context;

        public AccountRepository(IAccountContext context)
        {
            this.context = context;
        }

        public AccountRepository()
        {
            context = new AccountMSSQLContext();
        }

        public AccountDTO RetrieveUserData(int UserId) 
        {
            return context.RetrieveUserData(UserId);
        }
        public bool ServerSideValidationUsername(AccountDTO accountDTO) 
        {
            return context.ServerSideValidationUsername(accountDTO);
        }

        public bool ServerSideValidationPassword(AccountDTO accountDTO) 
        {
            return context.ServerSideValidationPassword(accountDTO);
        }

        public bool CheckUsernameAvailable(AccountDTO accountDTO) 
        {
            return context.CheckUsernameAvailable(accountDTO);
        }
        public bool VerifyLoginData(AccountDTO accountDTO) 
        {
            return context.VerifyLoginData(accountDTO);
        }
        public bool UpdateUsername(AccountDTO accountDTO) 
        {
            return context.UpdateUsername(accountDTO);
        }
        public bool UpdatePassword(AccountDTO accountDTO)
        {
            return context.UpdatePassword(accountDTO);
        }
    }
}
