using Auluxa.WebApp.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Auluxa.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private AuthSignInManager _signInManager;
        private AuthUserManager _userManager;

        public AuthSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<AuthSignInManager>(); }
            private set { _signInManager = value; }
        }
        public AuthUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AuthUserManager>(); }
            private set { _userManager = value; }
        }

        public HomeController()
        {

        }

        /// <summary>
        /// Returns the login view or redirects the user to the admin area.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()


        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View("Login");
            }
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            SignInStatus result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    {
                        OAuthToken oAuthToken = await AuthProxy.LoginAsync(model.Email, model.Password);
                        if (!string.IsNullOrEmpty(oAuthToken?.access_token))
                        {
                            return RedirectToAction("Index", "Admin");
                        }

                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }

                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Log out the user.
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Returns the forget password page.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ForgotPassword()
        {
            return View("ForgotPassword");
        }

        /// <summary>
        /// POST: /Home/ForgotPassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            // BAMBI redo this to call ur forgot password API, you have the email int he model.
            // BAMBI redo this to call ur forgot password API, you have the email int he model.
            // BAMBI redo this to call ur forgot password API, you have the email int he model.
            // BAMBI redo this to call ur forgot password API, you have the email int he model.
            
            // If it goes well :)
            return RedirectToAction("ForgotPasswordConfirmation");


            // Template CODE
            // Template CODE
            // Template CODE
            // Template CODE

            //if (!ModelState.IsValid)
            //    return View(model);

            //var user = await UserManager.FindByNameAsync(model.Email);
            //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            //{
            //    // Don't reveal that the user does not exist or is not confirmed
            //    return View("ForgotPassword");
            //}

            //// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //// Send an email with this link
            //// string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //// var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            //// await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            //return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public async Task<ActionResult> ForgotPasswordConfirmation()
        {
            return View("ForgotPasswordConfirmation");
        }


        [HttpGet]
        public ActionResult ResetPassword(string code, string email)
        {
            // http://localhost:57776/home/ResetPassword?code=codecodecode&email=asd@asd.com
            if (code == null)
            {
                return View("Error");
            }
            else
            {
                ViewBag.code = code;
                ViewBag.email = email;
                return View("ResetPassword");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // BAMBI redo this to call ur reset password API.
            // BAMBI redo this to call ur reset password API.
            // BAMBI redo this to call ur reset password API.
            // BAMBI redo this to call ur reset password API.

            // If all goes well
            return RedirectToAction("ResetPasswordConfirmation", "Home");



            // Template CODE
            // Template CODE
            // Template CODE
            // Template CODE

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            //var user = await UserManager.FindByNameAsync(model.Email);
            //if (user == null)
            //{
            //    // Don't reveal that the user does not exist
            //    return RedirectToAction("ResetPasswordConfirmation", "Home");
            //}
            //var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            //if (result.Succeeded)
            //{
            //    return RedirectToAction("ResetPasswordConfirmation", "Home");
            //}

            //return View();
        }

        [HttpGet]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }






        ///// <summary>
        ///// Confirms the email or a user.
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return View("Error");
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return RedirectToAction("Index", "Admin");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
