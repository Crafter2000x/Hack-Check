using HackCheck.Data;

namespace HackCheck.Business
{
    public class CreateAccountContainer
    {
        public bool AddAccountToDatabase(CreateAccountViewModel createaccountViewModel ) 
        {
            CreateAccountRepository repo = new CreateAccountRepository();
            return repo.AddAccountToDatabase(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }

        public bool ValidateAccountCreation(CreateAccountViewModel createaccountViewModel) 
        {
            CreateAccountRepository repo = new CreateAccountRepository();
            return repo.ValidateAccountCreation(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }

        public bool CheckForEmailTaken(CreateAccountViewModel createaccountViewModel)
        {
            CreateAccountRepository repo = new CreateAccountRepository();
            return repo.CheckForEmailTaken(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }

        public bool CheckForUsernameTaken(CreateAccountViewModel createaccountViewModel)
        {
            CreateAccountRepository repo = new CreateAccountRepository();
            return repo.CheckForUsernameTaken(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }
    }
}
