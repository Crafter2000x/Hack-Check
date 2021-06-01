using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCheck.Data
{
    public interface ICreateAccountContext
    {
        bool AddAccountToDatabase(CreateAccountDTO accountDTO);
        bool ValidateAccountCreation(CreateAccountDTO accountDTO);
        public bool CheckForUsernameTaken(CreateAccountDTO createaccountDTO);
        public bool CheckForEmailTaken(CreateAccountDTO createaccountDTO);
    }
}
