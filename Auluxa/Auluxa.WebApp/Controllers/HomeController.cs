﻿using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace Auluxa.WebApp.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the login view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Login");
        }

        /// <summary>
        /// POST: /Home/Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            // Requests the bearer from oAuthToken.
            FormsAuthentication.SetAuthCookie("username", true);
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Admin");
        }

        /// <summary>
        /// GET: /Home/ForgotPassword
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ForgotPassword()
        {
            // Send email.
            return View("ForgotPassword");
        }

        /// <summary>
        /// POST: /Home/ForgotPassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            // Send email.
            return Index();
        }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
