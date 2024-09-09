using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MortezaeeShop.Data.Repositories;
using MortezaeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MortezaeeShop.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            Users users = new Users()
            {
                Email = register.Email.ToLower(),
                Password = register.Password,
                IsAdmin = false,
                RegisterDate = DateTime.Now
            };

            _userRepository.AddUser(users);
            return View("SuccessRegister", register);
        }
        #endregion
        public IActionResult VerifyEmail(string email)
        {
            if (_userRepository.IsExistUserByEmail(email.ToLower()))
            {
                return Json($"ایمیل {email} وارد شده تکرار می باشد");
            }

            return Json(true);
        }
        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVieModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userRepository.GetUserForLogin(login.Email.ToLower(),login.Password);
            if(user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات وارد شده صحیح ننیست");
                return View(login);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };

             HttpContext.SignInAsync(principal, properties);
            return Redirect("/");
        }
        #endregion

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Account/Login");
        }
    }
}
