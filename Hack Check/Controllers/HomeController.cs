using Hack_Check.Models;
using Hack_Check.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hack_Check.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

#region Return views for pages

        public IActionResult Index()
        {
            if (CheckForSession())
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        public IActionResult Login() 
        {
            if (CheckForSession())
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        public IActionResult CreateAccount() 
        {
            if (CheckForSession())
            {
                return RedirectToAction("Home", "Home");
            }
            return View();
        }

        public IActionResult Home() 
        {

            if (CheckForSession())
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Account()
        {
            if (CheckForSession())
            {
                AccountActions accountActions = new AccountActions();
                AccountViewModel accountViewModel = accountActions.FilledAccountViewModel(int.Parse(HttpContext.Session.GetString("UserId")));

                if (accountViewModel == null)
                {
                    ModelState.AddModelError("","A error has occurred while try to get your information, please contact support");
                }

                return View(accountViewModel);
            }
            return RedirectToAction("Login", "Home");
        }

        public IActionResult ChangePassword()
        {

            if (CheckForSession())
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        public IActionResult ChangeUsername()
        {

            if (CheckForSession())
            {
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

#endregion

#region Post functions

        [HttpPost]
        public IActionResult CreateAccount(CreateAccountViewModel createAccountViewModel) 
        {
            VerifyNewAccount verifyNewAccount = new VerifyNewAccount();

            //First check on the server if the fields are empty incase they have javascript turned off
            if (verifyNewAccount.ServerSideValidation(createAccountViewModel) == false)
            {
                ModelState.AddModelError("", "Server validation failed");
                return View();
            }

            //Check if the username is already in use because you can't have 2 people with the same username
            if (verifyNewAccount.UsernameAlreadyTaken(createAccountViewModel) == true)
            {
                ModelState.AddModelError("","Username is already in use");
                return View();
            }

            //Check if the email is already in use because only one email address use is allowed 
            if (verifyNewAccount.EmailAlreadyTaken(createAccountViewModel) == true)
            {
                ModelState.AddModelError("", "Email address is already in use");
                return View();
            }

            //Check if the passwords match, this can be added to ServerSideValidation
            if (verifyNewAccount.DoThePasswordsMatch(createAccountViewModel) == false)
            {
                return View();
            }

            //Hash and Salt password and add it all to the Database for more secure password storage
            SetupNewAccount setupNewAccount = new SetupNewAccount();
            if (setupNewAccount.AddAccountToDatabase(setupNewAccount.DatabaseReadyCreateAccountViewModel(createAccountViewModel)) == true)
            {
                //Return to the login page so you can login with your new data
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel) 
        {
            VerifyLogin verifyLogin = new VerifyLogin();

            //First check on the server if the fields are empty incase they have javascript turned off
            if (verifyLogin.ServerSideValidation(loginViewModel) == false)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            if (verifyLogin.VerifyLoginData(loginViewModel) == false)
            {
                ModelState.AddModelError("", "Username or password is inccorect");
                return View();
            }

            //I need the ID to store in the session so I can user it in other parts
            int UserId = verifyLogin.FillLoginWithUserId(loginViewModel.Username);

            if (UserId == -1)
            {
                ModelState.AddModelError("", "An error occurred while processing your request, please contact support");
                return View();
            }

            loginViewModel.UserId = UserId;

            //I put the username in a session too for later use
            HttpContext.Session.SetString("Username", loginViewModel.Username);
            HttpContext.Session.SetString("UserId", loginViewModel.UserId.ToString());
            return RedirectToAction("Home", "Home");
        }

        [HttpPost]
        public IActionResult ChangePassword(AccountViewModel accountViewModel)
        {
            AccountActions accountActions = new AccountActions();

            accountViewModel.Username = HttpContext.Session.GetString("Username");
            accountViewModel.Id = int.Parse(HttpContext.Session.GetString("UserId"));

            //First check on the server if the fields are empty incase they have javascript turned off
            if (accountActions.ServerSideValidationPassword(accountViewModel) == false)
            {
                ModelState.AddModelError("Id", "Please fill out the fields correctly");
                return View();
            }

            //Making sure the user is the actual user 
            if (accountActions.CheckPassword(accountViewModel) == false)
            {
                ModelState.AddModelError("OldPassword", "The password was incorrect");
                return View();
            }

            if (accountActions.UpdatePassword(accountActions.SecurePassword(accountViewModel)) == false)
            {
                ModelState.AddModelError("Id", "Something went wrong updating your password");
                return View();
            }

            ModelState.AddModelError("", "Password has been updated");
            return View();
        }

        [HttpPost]
        public IActionResult ChangeUsername(AccountViewModel accountViewModel)
        {
            AccountActions accountActions = new AccountActions();

            accountViewModel.Username = HttpContext.Session.GetString("Username");
            accountViewModel.Id = int.Parse(HttpContext.Session.GetString("UserId"));

            //First check on the server if the fields are empty incase they have javascript turned off
            if (accountActions.ServerSideValidationUsername(accountViewModel) == false)
            {
                ModelState.AddModelError("Id", "Please fill out the fields correctly");
                return View();
            }

            accountViewModel.Password = accountViewModel.OldPassword;

            //Making sure the user is the actual user and also using this to change the password later on
            if (accountActions.CheckPassword(accountViewModel) == false)
            {
                ModelState.AddModelError("OldPassword", "The password was incorrect");
                return View();
            }

            //Check if the username is already in use because you can't have 2 people with the same username
            if (accountActions.CheckUsernameAvailable(accountViewModel) == false)
            {
                ModelState.AddModelError("NewUsername", "Username is already in use");
                return View();
            }

            if (accountActions.UpdateUsername(accountViewModel) == false)
            {
                ModelState.AddModelError("Id", "Failed to update your username");
                return View();
            }

            accountViewModel.Username = accountViewModel.NewUsername;

            //Because the password is stored cominbed in a way with the username I need to update the password to reflect the change in username
            if (accountActions.UpdatePassword(accountActions.SecurePassword(accountViewModel)) == false)
            {
                ModelState.AddModelError("Id", "Failed to update your username");
                return View();
            }

            //Updating the session username for later use
            HttpContext.Session.SetString("Username", accountViewModel.Username);
            ModelState.AddModelError("", "Username has been updated");
            return View();
        }
#endregion

#region Other functions
        public IActionResult LogoutUser()
        {
            //Clear the session for new use
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private bool CheckForSession() 
        {
            if (HttpContext.Session.GetString("Username") == null || HttpContext.Session.GetString("UserId") == null)
            {
                return false;
            }
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
#endregion
    }
}
