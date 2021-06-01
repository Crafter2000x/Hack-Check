using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCheck.Data
{
    public interface IAccountContext
    {
        AccountDTO RetrieveUserData(int UserId);
        bool ServerSideValidationUsername(AccountDTO accountDTO);
        bool ServerSideValidationPassword(AccountDTO accountDTO);
        bool CheckUsernameAvailable(AccountDTO accountDTO);
        bool VerifyLoginData(AccountDTO accountDTO);
        bool UpdateUsername(AccountDTO accountDTO);
        bool UpdatePassword(AccountDTO accountDTO);

    }
}
