using HackCheck.Data;

namespace HackCheck.Business
{
    public class AccountContainer
    {
        private AccountRepository Repo;

        public AccountContainer(AccountRepository _Repo)
        {
            Repo = _Repo;
        }

        public AccountContainer()
        {
            Repo = new AccountRepository();
        }

        public AccountViewModel RetrieveUserData(int UserId) 
        {
            AccountDTO DTO = Repo.RetrieveUserData(UserId);
            return new AccountViewModel {Id = DTO.Id, Email = DTO.Email, Username = DTO.Username, OldPassword = DTO.OldPassword, Password = DTO.Password, ConfirmPassword = DTO.ConfirmPassword, NewUsername = DTO.NewUsername, Salt = DTO.Salt };
        }

        public bool ServerSideValidationUsername(AccountViewModel accountViewModel) 
        {
            return Repo.ServerSideValidationUsername(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });

        }

        public bool ServerSideValidationPassword(AccountViewModel accountViewModel) 
        {
            return Repo.ServerSideValidationPassword(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool CheckUsernameAvailable(AccountViewModel accountViewModel) 
        {
            return Repo.CheckUsernameAvailable(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool VerifyLoginData(AccountViewModel accountViewModel) 
        {
            return Repo.VerifyLoginData(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool UpdateUsername(AccountViewModel accountViewModel) 
        {
            return Repo.UpdateUsername(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool UpdatePassword(AccountViewModel accountViewModel) 
        {
            return Repo.UpdatePassword(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }
    }
}
