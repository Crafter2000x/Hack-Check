using Hack_Check.Models;
using Hack_Check.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;


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
            //return View(TestViewModel);
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Privacy()
        {
            //return View();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Login() 
        {
            return View();
        }

        public IActionResult CreateAccount() 
        {
            return View();
        }

        public IActionResult HashPassword(TestViewModel TestViewModel) 
        {
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CreateNewAccount(CreateAccountViewModel createAccountViewModel) 
        {
            VerifyNewAccount verifyNewAccount = new VerifyNewAccount();

            //Check if the passwords match
            if (verifyNewAccount.DoThePasswordsMatch(createAccountViewModel) == false)
            {
                return RedirectToAction("CreateAccount", "Home");
            }

            //Check if the email is already in use
            if (verifyNewAccount.EmailAlreadyTaken(createAccountViewModel) == true)
            {
                return RedirectToAction("CreateAccount", "Home");
            }

            //Hash and Salt password and add it all to the Database
            SetupNewAccount setupNewAccount = new SetupNewAccount();
            if (setupNewAccount.AddAccountToDatabase(setupNewAccount.DatabaseReadyCreateAccountViewModel(createAccountViewModel)) == true)
            {
                //Return to the login page
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("CreateAccount", "Home");
        }

        public IActionResult LoginUser(LoginViewModel loginViewModel) 
        {
            VerifyLogin verifyLogin = new VerifyLogin();

            if (verifyLogin.VerifyLoginData(loginViewModel) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("CreateAccount", "Home");

        }
    }
}
