using Microsoft.VisualStudio.TestTools.UnitTesting;
using HackCheck.Data;
using Moq;
using Microsoft.Extensions.Configuration;

namespace HackCheck.Business.UnitTests
{
    [TestClass]
    public class LoginContainerTests
    {
        [TestMethod]
        public void ValidateLogin_IsValid_True()
        {
            // Arrange
            LoginViewModel loginViewModel = new LoginViewModel { Username = "Admin", Password = "Password" };
            LoginContainer Container = new LoginContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ValidateLogin(loginViewModel);
            // Assert
            Assert.IsTrue(assert);
        }

        [TestMethod]
        public void VValidateLogin_IsInValid_False()
        {
            // Arrange
            LoginViewModel loginViewModel = new LoginViewModel { Username = "Ad", Password = "Pass" };
            LoginContainer Container = new LoginContainer(new Mock<IConfiguration>().Object);
            bool assert;
            // Act
            assert = Container.ValidateLogin(loginViewModel);
            // Assert
            Assert.IsFalse(assert);
        }

        [TestMethod]
        public void VerfiyLoginData_LoginMatched_True()
        {
            // Arrange
            LoginViewModel loginViewModel = new LoginViewModel { Username = "Admin", Password = "Password"};

            var MockLoginRepo = new Mock<LoginRepository>(new Mock<IConfiguration>().Object);

            MockLoginRepo.Setup(x => x.VerifyLoginData(It.IsAny<LoginDTO>())).Returns(true);

            LoginContainer Container = new LoginContainer(MockLoginRepo.Object);

            bool assert;

            // Act

            assert = Container.VerifyLoginData(loginViewModel);

            // Assert
            
            Assert.IsTrue(assert);

        }

        [TestMethod]
        public void VerfiyLoginData_LoginNotMatched_False()
        {
            // Arrange
            LoginViewModel loginViewModel = new LoginViewModel { Username = "Admin", Password = "Password" };

            var MockLoginRepo = new Mock<LoginRepository>(new Mock<IConfiguration>().Object);

            MockLoginRepo.Setup(x => x.VerifyLoginData(It.IsAny<LoginDTO>())).Returns(false);

            LoginContainer Container = new LoginContainer(MockLoginRepo.Object);

            bool assert;

            // Act

            assert = Container.VerifyLoginData(loginViewModel);

            // Assert

            Assert.IsFalse(assert);

        }

        [TestMethod]
        public void GetUserId_UserInDB_NotMinusOne()
        {
            // Arrange
            LoginViewModel loginViewModel = new LoginViewModel { Username = "Admin", Password = "Password" };

            var MockLoginRepo = new Mock<LoginRepository>(new Mock<IConfiguration>().Object);

            MockLoginRepo.Setup(x => x.GetUserId(It.IsAny<LoginDTO>())).Returns(1);

            LoginContainer Container = new LoginContainer(MockLoginRepo.Object);

            int assert;

            // Act

            assert = Container.GetUserId(loginViewModel);

            // Assert

            Assert.AreNotEqual(-1,assert);

        }

        [TestMethod]
        public void GetUserId_UserNotInDB_MinusOne()
        {
            // Arrange
            LoginViewModel loginViewModel = new LoginViewModel { Username = "Admin", Password = "Password" };

            var MockLoginRepo = new Mock<LoginRepository>(new Mock<IConfiguration>().Object);

            MockLoginRepo.Setup(x => x.GetUserId(It.IsAny<LoginDTO>())).Returns(-1);

            LoginContainer Container = new LoginContainer(MockLoginRepo.Object);

            int assert;

            // Act

            assert = Container.GetUserId(loginViewModel);

            // Assert

            Assert.AreEqual(-1, assert);

        }
    }
}
