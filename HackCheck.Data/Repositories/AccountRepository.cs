
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

        public virtual AccountDTO RetrieveUserData(int UserId) 
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

        public virtual bool CheckUsernameAvailable(AccountDTO accountDTO) 
        {
            return context.CheckUsernameAvailable(accountDTO);
        }
        public virtual bool VerifyLoginData(AccountDTO accountDTO) 
        {
            return context.VerifyLoginData(accountDTO);
        }
        public virtual bool UpdateUsername(AccountDTO accountDTO) 
        {
            return context.UpdateUsername(accountDTO);
        }
        public virtual bool UpdatePassword(AccountDTO accountDTO)
        {
            return context.UpdatePassword(accountDTO);
        }
    }
}
