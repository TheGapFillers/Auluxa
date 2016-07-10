using Auluxa.WebApp.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
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
        public async Task<ActionResult> ForgotPassword() =>
            View("ForgotPassword");

        /// <summary>
        /// POST: /Home/ForgotPassword
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // GetUserAsync the user, check that his email is confirmed
            AuthUser user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null || !await UserManager.IsEmailConfirmedAsync(user.Id))
                // Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPassword");

            await SendConfirmPasswordAsync(user.Id);

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public async Task<ActionResult> ForgotPasswordConfirmation() =>
            View("ForgotPasswordConfirmation");


        [HttpGet]
        public async Task<ActionResult> ResetPassword(string userId, string code)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(userId))
                return View("Error");

            AuthUser user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");

            ViewBag.email = user.Email;

            ResetPasswordViewModel ResetPasswordViewModel = new ResetPasswordViewModel();
            ResetPasswordViewModel.Code = code;
            ResetPasswordViewModel.Email = user.Email;

            return View("ResetPassword", ResetPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            AuthUser user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Home");

            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation", "Home");

            ModelState.AddModelError("", "Failed to reset the account.");
            return View();
        }

        [HttpGet]
        public ActionResult ResetPasswordConfirmation() => View();

        /// <summary>
        /// Confirms the email or a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View("ConfirmEmailConfirmation");
        }

        private async Task SendConfirmPasswordAsync(string userId)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
            code = HttpUtility.UrlEncode(code);

            // Create a callback Url with the code inside
            string callbackUrl =
                $"{ConfigurationManager.AppSettings["auluxa-auth:Url"]}Home/ResetPassword?userId={userId}&code={code}";

            // Send an email with the callback Url
            await UserManager.SendEmailAsync(userId, "Confirm your password",
                    "Please confirm your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        }

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
