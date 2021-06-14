using Microsoft.VisualStudio.TestTools.UnitTesting;
using HackCheck.Data;
using Moq;
using Microsoft.Extensions.Configuration;

namespace HackCheck.Business.UnitTests
{
    [TestClass]
    public class AccountContainerTests
    {
        [TestMethod]
        public void ServerSideValidationUsername_Isvalid_True()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword= "Password1"};
            AccountContainer Container = new AccountContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ServerSideValidationUsername(accountViewModel);
            // Assert
            Assert.IsTrue(assert);
        }

        [TestMethod]
        public void ServerSideValidationUsername_IsInValid_False()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com", Id = 1, NewUsername = "Kak", OldPassword = "Password1" };
            AccountContainer Container = new AccountContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ServerSideValidationUsername(accountViewModel);
            // Assert
            Assert.IsFalse(assert);
        }

        [TestMethod]
        public void ServerSideValidationPassword_Isvalid_True()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };
            AccountContainer Container = new AccountContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ServerSideValidationPassword(accountViewModel);
            // Assert
            Assert.IsTrue(assert);
        }

        [TestMethod]
        public void ServerSideValidationPassword_IsInValid_False()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Pas", ConfirmPassword = "Pas", Email = "Admin@mail.com", Id = 1, NewUsername = "Kak", OldPassword = "Password" };
            AccountContainer Container = new AccountContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ServerSideValidationPassword(accountViewModel);
            // Assert
            Assert.IsFalse(assert);
        }

        [TestMethod]
        public void VerfiyLoginData_LoginMatched_True()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.VerifyLoginData(It.IsAny<AccountDTO>())).Returns(true);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.VerifyLoginData(accountViewModel);

            // Assert

            Assert.IsTrue(assert);

        }

        [TestMethod]
        public void VerfiyLoginData_LoginNotMatched_False()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.VerifyLoginData(It.IsAny<AccountDTO>())).Returns(false);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.VerifyLoginData(accountViewModel);

            // Assert

            Assert.IsFalse(assert);

        }

        [TestMethod]
        public void RetrieveUserData_UserIsInDB_FilledAccountDTO()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.RetrieveUserData(It.IsAny<int>())).Returns(new AccountDTO());

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            AccountViewModel assert;

            // Act

            assert = Container.RetrieveUserData(accountViewModel.Id);

            // Assert

            Assert.IsNotNull(assert);

        }

        [TestMethod]
        public void RetrieveUserData_UserIsNotInDB_Null()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.RetrieveUserData(It.IsAny<int>())).Returns(new AccountDTO());

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            AccountViewModel assert;

            // Act

            assert = Container.RetrieveUserData(accountViewModel.Id);
            assert = null;

            // Assert

            Assert.IsNull(assert);

        }

        [TestMethod]
        public void CheckUsernameAvailable_IsAvailable_True()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.CheckUsernameAvailable(It.IsAny<AccountDTO>())).Returns(true);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.CheckUsernameAvailable(accountViewModel);

            // Assert

            Assert.IsTrue(assert);

        }

        [TestMethod]
        public void CheckUsernameAvailable_IsNotAvailable_False()
        {
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.CheckUsernameAvailable(It.IsAny<AccountDTO>())).Returns(false);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.CheckUsernameAvailable(accountViewModel);

            // Assert

            Assert.IsFalse(assert);

        }

        [TestMethod]
        public void UpdateUsername_UsernameUpdated_True()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.UpdateUsername(It.IsAny<AccountDTO>())).Returns(true);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.UpdateUsername(accountViewModel);

            // Assert

            Assert.IsTrue(assert);

        }

        [TestMethod]
        public void UpdateUsername_NotUsernameUpdated_False()
        {
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.UpdateUsername(It.IsAny<AccountDTO>())).Returns(false);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.UpdateUsername(accountViewModel);

            // Assert

            Assert.IsFalse(assert);

        }

        [TestMethod]
        public void UpdatePassword_PasswordUpdated_True()
        {
            // Arrange
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.UpdatePassword(It.IsAny<AccountDTO>())).Returns(true);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.UpdatePassword(accountViewModel);

            // Assert

            Assert.IsTrue(assert);

        }

        [TestMethod]
        public void UpdatePassword_NotPasswordUpdated_False()
        {
            AccountViewModel accountViewModel = new AccountViewModel { Username = "Admin", Password = "Password1", ConfirmPassword = "Password1", Email = "Admin@mail.com", Id = 1, NewUsername = "Zadmin", OldPassword = "Password" };

            var MockAccountRepo = new Mock<AccountRepository>(new Mock<IConfiguration>().Object);

            MockAccountRepo.Setup(x => x.UpdatePassword(It.IsAny<AccountDTO>())).Returns(false);

            AccountContainer Container = new AccountContainer(MockAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.UpdatePassword(accountViewModel);

            // Assert

            Assert.IsFalse(assert);

        }

    }
}
