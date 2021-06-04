using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCheck.Data
{
    public class CreateAccountRepository
    {
        private ICreateAccountContext context;

        public CreateAccountRepository(ICreateAccountContext context)
        {
            this.context = context;
        }

        public CreateAccountRepository()
        {
            this.context = new CreateAccountMSSQLContext();
        }

        public virtual bool AddAccountToDatabase(CreateAccountDTO createaccountDTO) 
        {
            return context.AddAccountToDatabase(createaccountDTO);
        }

        public virtual bool CheckForUsernameTaken(CreateAccountDTO createaccountDTO) 
        {
            return context.CheckForUsernameTaken(createaccountDTO);
        }

        public virtual bool CheckForEmailTaken(CreateAccountDTO createaccountDTO)
        {
            return context.CheckForEmailTaken(createaccountDTO);
        }

        public bool ValidateAccountCreation(CreateAccountDTO createaccountDTO) 
        {
            return context.ValidateAccountCreation(createaccountDTO);
        }
    }
}
