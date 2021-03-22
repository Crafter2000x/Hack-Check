using Hack_Check.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hack_Check.Classes
{
    public class AccountActions
    {
        public AccountViewModel FilledAccountViewModel(int UserId) 
        {
            Queries queries = new Queries();

            AccountViewModel FilledModel = queries.AccountInformation(UserId);

            return FilledModel;

        }
    }
}
