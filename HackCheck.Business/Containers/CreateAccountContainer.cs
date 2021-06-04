using HackCheck.Data;

namespace HackCheck.Business
{
    public class CreateAccountContainer
    {
        private CreateAccountRepository Repo;

        public CreateAccountContainer(CreateAccountRepository _Repo)
        {
            Repo = _Repo;
        }

        public CreateAccountContainer()
        {
            Repo = new CreateAccountRepository();
        }

        public bool AddAccountToDatabase(CreateAccountViewModel createaccountViewModel ) 
        {
            return Repo.AddAccountToDatabase(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }

        public bool ValidateAccountCreation(CreateAccountViewModel createaccountViewModel) 
        {
            return Repo.ValidateAccountCreation(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }

        public bool CheckForEmailTaken(CreateAccountViewModel createaccountViewModel)
        {
            return Repo.CheckForEmailTaken(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }

        public bool CheckForUsernameTaken(CreateAccountViewModel createaccountViewModel)
        {
            return Repo.CheckForUsernameTaken(new CreateAccountDTO { Username = createaccountViewModel.Username, Password = createaccountViewModel.Password, ConfirmPassword = createaccountViewModel.ConfirmPassword, Email = createaccountViewModel.Email });
        }
    }
}
