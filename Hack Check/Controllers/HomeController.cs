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

        [HttpPost]
        public IActionResult CreateAccount(CreateAccountViewModel createAccountViewModel) 
        {
            VerifyNewAccount verifyNewAccount = new VerifyNewAccount();

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

            if (verifyNewAccount.ServerSideValidation(createAccountViewModel) == false)
            {
                ModelState.AddModelError("", "Server validation failed, do you have javascript enabled?");
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
        public IActionResult LoginUser(LoginViewModel loginViewModel) 
        {
            VerifyLogin verifyLogin = new VerifyLogin();

            if (verifyLogin.VerifyLoginData(loginViewModel) == false)
            {
                return RedirectToAction("Login", "Home");
            }

            HttpContext.Session.SetString("Username", loginViewModel.Username);
            return RedirectToAction("Home", "Home");
        }

        [HttpPost]
        public IActionResult LogoutUser()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private bool CheckForSession() 
        {
            if (HttpContext.Session.GetString("Username") == null)
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
    }
}
