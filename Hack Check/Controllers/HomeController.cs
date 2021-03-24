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
        #endregion

        #region Post functions
        [HttpPost]
        public IActionResult CreateAccount(CreateAccountViewModel createAccountViewModel) 
        {
            VerifyNewAccount verifyNewAccount = new VerifyNewAccount();

            //First check if they have the right requirments
            if (verifyNewAccount.ServerSideValidation(createAccountViewModel) == false)
            {
                ModelState.AddModelError("", "Server validation failed");
                return View();
            }

            //Check if the username is already in use
            if (verifyNewAccount.UsernameAlreadyTaken(createAccountViewModel) == true)
            {
                ModelState.AddModelError("","Username is already in use");
                return View();
            }

            //Check if the email is already in use
            if (verifyNewAccount.EmailAlreadyTaken(createAccountViewModel) == true)
            {
                ModelState.AddModelError("", "Email address is already in use");
                return View();
            }

            //Check if the passwords match
            if (verifyNewAccount.DoThePasswordsMatch(createAccountViewModel) == false)
            {
                return View();
            }

            //Hash and Salt password and add it all to the Database
            SetupNewAccount setupNewAccount = new SetupNewAccount();
            if (setupNewAccount.AddAccountToDatabase(setupNewAccount.DatabaseReadyCreateAccountViewModel(createAccountViewModel)) == true)
            {
                //Return to the login page
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel) 
        {
            VerifyLogin verifyLogin = new VerifyLogin();

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

            int UserId = verifyLogin.FillLoginWithUserId(loginViewModel.Username);

            if (UserId == -1)
            {
                ModelState.AddModelError("", "An error occurred while processing your request, please contact support");
                return View();
            }

            loginViewModel.UserId = UserId;

            HttpContext.Session.SetString("Username", loginViewModel.Username);
            HttpContext.Session.SetString("UserId", loginViewModel.UserId.ToString());
            return RedirectToAction("Home", "Home");
        }

        [HttpPost]
        public IActionResult Account(AccountViewModel accountViewModel) 
        {
            AccountActions accountActions = new AccountActions();

            if (accountActions.ServerSideValidation(accountViewModel) == false)
            {
                ModelState.AddModelError("Password", "Please fill out the fields correctly");
                return View(accountViewModel);
            }

            accountViewModel.Id = int.Parse(HttpContext.Session.GetString("UserId"));

            if (accountActions.UpdatePassword(accountActions.SecurePassword(accountViewModel)) == false)
            {
                ModelState.AddModelError("Password", "Something went wrong updating your password");
                return View(accountViewModel);
            }

            ModelState.AddModelError("", "Password has been updated");
            return View(accountViewModel);


            // this is for changing the username what i scraped later

            // Check if they want to change the password or username
            //if (accountViewModel.Username != null)
            //{
            //    AccountActions accountActions = new AccountActions();

            //    if (accountActions.UsernameAlreadyTaken(accountViewModel) == true)
            //    {
            //        ModelState.AddModelError("Username", "Username is already in use");
            //        return View(accountActions.FilledAccountViewModel(int.Parse(HttpContext.Session.GetString("UserId"))));
            //    }

            //    if (accountActions.UsernameServerValidation(accountViewModel) == false)
            //    {
            //        ModelState.AddModelError("Username", "Server validation failed");
            //        return View(accountActions.FilledAccountViewModel(int.Parse(HttpContext.Session.GetString("UserId"))));
            //    }

            //    accountViewModel.Id = int.Parse(HttpContext.Session.GetString("UserId"));

            //    if (accountActions.UsernameUpdate(accountViewModel) == false)
            //    {
            //        ModelState.AddModelError("Username", "Failed to update your username");
            //        return View(accountActions.FilledAccountViewModel(int.Parse(HttpContext.Session.GetString("UserId"))));
            //    }

            //    HttpContext.Session.SetString("Username", accountViewModel.Username);
            //    ModelState.AddModelError("", "Username has been updated");
            //    return View(accountActions.FilledAccountViewModel(int.Parse(HttpContext.Session.GetString("UserId"))));
            //}
            //else if (accountViewModel.Password != null && accountViewModel.ConfirmPassword != null)
            //{

            //}
        }
        #endregion

        #region Other functions
        public IActionResult LogoutUser()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private bool CheckForSession() 
        {
            if (HttpContext.Session.GetString("Username") == null && HttpContext.Session.GetString("UserId") == null)
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
