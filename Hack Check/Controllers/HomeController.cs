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

        public IActionResult Index(TestViewModel TestViewModel)
        {
            if (TestViewModel.Password != null)
            {
                TestViewModel.HashedPassword = ComputeStringToSha256Hash(TestViewModel.Password);
            }
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

            SetupNewAccount setupNewAccount = new SetupNewAccount();
            
            if (verifyNewAccount.EmailAlreadyTaken(setupNewAccount.DatabaseReadyCreateAccountViewModel(createAccountViewModel)))
            {

            }
            //Hash and Salt password and add it all to the Database


            //Return to the login page
            return RedirectToAction("Index", "Home");
        }

























        static string ComputeStringToSha256Hash(string plainText)
        {
            // Create a SHA256 hash from string   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computing Hash - returns here byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));

                // now convert byte array to a string   
                StringBuilder stringbuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringbuilder.Append(bytes[i].ToString("x2"));
                }
                return stringbuilder.ToString();
            }
        }
    }
}
