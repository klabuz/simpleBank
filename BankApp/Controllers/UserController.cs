using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;
using System.Diagnostics;
using System.Linq;

namespace SimpleBank.Controllers
{
    public class UserController : Controller
    {
        private SimpleBankContext _context;

        public UserController(SimpleBankContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Route("sign-up")]
        public IActionResult SignUp(UserViewModel user)
        {
            SignUp SUP = user.SUP;

            if (ModelState.IsValid)
            {
                User RegisterUser = _context.Users.SingleOrDefault(i => i.Email == SUP.Email);
                if (RegisterUser != null)
                {
                    ModelState.AddModelError("SUP.Email", "This email exists. Please use a different email.");

                    return View("SignUp");
                }
                PasswordHasher<SignUp> Hasher = new PasswordHasher<SignUp>();
                SUP.PasswordHash = Hasher.HashPassword(SUP, SUP.PasswordHash);

                User newUser = new User()
                {
                    FirstName = SUP.FirstName,
                    LastName = SUP.LastName,
                    UserName = SUP.UserName,
                    PasswordHash = SUP.PasswordHash,
                    Phone = SUP.Phone,
                    Email = SUP.Email,
                    State = SUP.State,
                    City = SUP.City,
                    Country = SUP.Country,
                    StreetAddress = SUP.StreetAddress
                };

                _context.Add(newUser);
                _context.SaveChanges();

                User currentUser = _context.Users.SingleOrDefault(u => u.Email == newUser.Email);
                HttpContext.Session.SetInt32("UserId", currentUser.Id);

                return RedirectToAction("Dashboard", "Dashboard");
            }
            return View("Index", "Home");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserViewModel user)
        {
            Login Log = user.Log;
            if (ModelState.IsValid)
            {
                var currentUser = _context.Users.SingleOrDefault(u => u.UserName == Log.UserName);
                if (currentUser != null && Log.PasswordHash != null)
                {
                    var Hasher = new PasswordHasher<User>();

                    if (0 != Hasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, Log.PasswordHash))
                    {
                        HttpContext.Session.SetInt32("UserId", currentUser.Id);
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("Log.PasswordHash", "Incorrect password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Log.UserName", "This username doesn't exist.");
                }
            }
            return View("Index", "Home");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index", "Home");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}