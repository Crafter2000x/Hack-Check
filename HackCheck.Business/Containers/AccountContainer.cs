using HackCheck.Data;

namespace HackCheck.Business
{
    public class AccountContainer
    {
        public AccountViewModel RetrieveUserData(int UserId) 
        {
            AccountRepository repo = new AccountRepository();
            AccountDTO DTO = repo.RetrieveUserData(UserId);
            return new AccountViewModel {Id = DTO.Id, Email = DTO.Email, Username = DTO.Username, OldPassword = DTO.OldPassword, Password = DTO.Password, ConfirmPassword = DTO.ConfirmPassword, NewUsername = DTO.NewUsername, Salt = DTO.Salt };
        }

        public bool ServerSideValidationUsername(AccountViewModel accountViewModel) 
        {
            AccountRepository repo = new AccountRepository();
            return repo.ServerSideValidationUsername(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });

        }

        public bool ServerSideValidationPassword(AccountViewModel accountViewModel) 
        {
            AccountRepository repo = new AccountRepository();
            return repo.ServerSideValidationPassword(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool CheckUsernameAvailable(AccountViewModel accountViewModel) 
        {
            AccountRepository repo = new AccountRepository();
            return repo.CheckUsernameAvailable(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool VerifyLoginData(AccountViewModel accountViewModel) 
        {
            AccountRepository repo = new AccountRepository();
            return repo.VerifyLoginData(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool UpdateUsername(AccountViewModel accountViewModel) 
        {
            AccountRepository repo = new AccountRepository();
            return repo.UpdateUsername(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }

        public bool UpdatePassword(AccountViewModel accountViewModel) 
        {
            AccountRepository repo = new AccountRepository();
            return repo.UpdatePassword(new AccountDTO { Id = accountViewModel.Id, Email = accountViewModel.Email, Username = accountViewModel.Username, OldPassword = accountViewModel.OldPassword, Password = accountViewModel.Password, ConfirmPassword = accountViewModel.ConfirmPassword, NewUsername = accountViewModel.NewUsername, Salt = accountViewModel.Salt });
        }
    }
}
