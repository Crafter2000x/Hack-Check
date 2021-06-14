using Microsoft.VisualStudio.TestTools.UnitTesting;
using HackCheck.Data;
using Moq;
using Microsoft.Extensions.Configuration;

namespace HackCheck.Business.UnitTests
{
    [TestClass]
    public class CreateAccountContainerTests
    {
        [TestMethod]
        public void ValidateAccountCreation_IsValid_True()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com" };
            CreateAccountContainer Container = new CreateAccountContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ValidateAccountCreation(createAccountViewModel);
            // Assert
            Assert.IsTrue(assert);
        }

        [TestMethod]
        public void ValidateAccountCreation_IsInValid_False()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Ad", Password = "Password1", ConfirmPassword = "Password2", Email = "Admin.com" };
            CreateAccountContainer Container = new CreateAccountContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ValidateAccountCreation(createAccountViewModel);
            // Assert
            Assert.IsFalse(assert);
        }

        [TestMethod]
        public void AddAccountToDatabase_AccountAdded_True()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com" };

            var MockCreateAccountRepo = new Mock<CreateAccountRepository>(new Mock<IConfiguration>().Object);

            MockCreateAccountRepo.Setup(x => x.AddAccountToDatabase(It.IsAny<CreateAccountDTO>())).Returns(true);

            CreateAccountContainer Container = new CreateAccountContainer(MockCreateAccountRepo.Object);

            bool assert;
            
            // Act

            assert = Container.AddAccountToDatabase(createAccountViewModel);

            // Assert

            Assert.IsTrue(assert);

        }

        [TestMethod]
        public void AddAccountToDatabase_AccountAddedFailed_False()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com" };

            var MockCreateAccountRepo = new Mock<CreateAccountRepository>(new Mock<IConfiguration>().Object);

            MockCreateAccountRepo.Setup(x => x.AddAccountToDatabase(It.IsAny<CreateAccountDTO>())).Returns(false);

            CreateAccountContainer Container = new CreateAccountContainer(MockCreateAccountRepo.Object);

            bool assert;

            // Act

            assert = Container.AddAccountToDatabase(createAccountViewModel);

            // Assert

            Assert.IsFalse(false);

        }

        [TestMethod]
        public void CheckForEmailTaken_EmailIsTaken_True()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com" };

            var MockCreateAccountRepo = new Mock<CreateAccountRepository>(new Mock<IConfiguration>().Object);

            MockCreateAccountRepo.Setup(x => x.CheckForEmailTaken(It.IsAny<CreateAccountDTO>())).Returns(true);

            CreateAccountContainer Container = new CreateAccountContainer(MockCreateAccountRepo.Object);


            bool assert;

            // Act

            assert = Container.CheckForEmailTaken(createAccountViewModel);

            // Assert

            Assert.IsTrue(true);

        }

        [TestMethod]
        public void CheckForEmailTaken_EmailIsNotTaken_False()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com" };

            var MockCreateAccountRepo = new Mock<CreateAccountRepository>(new Mock<IConfiguration>().Object);

            MockCreateAccountRepo.Setup(x => x.CheckForEmailTaken(It.IsAny<CreateAccountDTO>())).Returns(false);

            CreateAccountContainer Container = new CreateAccountContainer(MockCreateAccountRepo.Object);


            bool assert;

            // Act

            assert = Container.CheckForEmailTaken(createAccountViewModel);

            // Assert

            Assert.IsFalse(false);

        }

        [TestMethod]
        public void CheckForUsernameRaken_UsernameIsTaken_True()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com" };

            var MockCreateAccountRepo = new Mock<CreateAccountRepository>(new Mock<IConfiguration>().Object);

            MockCreateAccountRepo.Setup(x => x.CheckForUsernameTaken(It.IsAny<CreateAccountDTO>())).Returns(true);

            CreateAccountContainer Container = new CreateAccountContainer(MockCreateAccountRepo.Object);


            bool assert;

            // Act

            assert = Container.CheckForUsernameTaken(createAccountViewModel);

            // Assert

            Assert.IsTrue(true);

        }

        [TestMethod]
        public void CheckForUsernameRaken_UsernameIsNotTaken_False()
        {
            // Arrange
            CreateAccountViewModel createAccountViewModel = new CreateAccountViewModel { Username = "Admin", Password = "Password", ConfirmPassword = "Password", Email = "Admin@mail.com" };

            var MockCreateAccountRepo = new Mock<CreateAccountRepository>(new Mock<IConfiguration>().Object);

            MockCreateAccountRepo.Setup(x => x.CheckForUsernameTaken(It.IsAny<CreateAccountDTO>())).Returns(false);

            CreateAccountContainer Container = new CreateAccountContainer(MockCreateAccountRepo.Object);


            bool assert;

            // Act

            assert = Container.CheckForUsernameTaken(createAccountViewModel);

            // Assert

            Assert.IsFalse(false);

        }
    }
}
