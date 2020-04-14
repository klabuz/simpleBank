using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBank.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace SimpleBank.Controllers
{
    public class UserController : Controller
    {
        private SimpleBankContext _context;

        public UserController(SimpleBankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("signup")]
        public ActionResult SignUp()
        {
            return View("SignUp");
        }

        [HttpGet]
        [Route("login")]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("register")]
        public ActionResult SignUp(UserViewModel user)
        {
            SignUp SUP = user.SUP;

            if (ModelState.IsValid)
            {
                User RegisterUser = _context.Users.SingleOrDefault(i => i.Email == SUP.Email);
                if (RegisterUser != null)
                {
                    // ViewBag.Message = "This email exists. Please use a different email.";
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

                //_context.Add(newUser);
                //_context.SaveChanges();

                User currentUser = _context.Users.SingleOrDefault(u => u.Email == newUser.Email);
                HttpContext.Session.SetInt32("UserId", currentUser.Id);

                return RedirectToAction("Index", "Dashboard");
            }
            return View("Index", "Home");
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login(UserViewModel user)
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
                        return RedirectToAction("Index", "Dashboard");
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
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index", "Home");
        }

    }
}