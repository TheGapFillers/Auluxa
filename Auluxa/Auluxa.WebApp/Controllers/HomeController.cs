using Auluxa.WebApp.Auth;
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

        public HomeController()
        {

        }

        public HomeController(AuthUserManager userManager, AuthSignInManager signInManager)
        {

        }

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

        /// <summary>
        /// Returns the login view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() => View("Login");

        /// <summary>
        /// POST: /Home/Login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            // Requests the bearer from oAuthToken.
            AuthSignInManager signInManager = HttpContext.GetOwinContext().Get<AuthSignInManager>();
            var userManager = HttpContext.GetOwinContext().GetUserManager<AuthUserManager>();

            AuthUser authUser = await userManager.FindAsync("valter.santos.matos@gmail.com", "qweqweqwe");
            await signInManager.SignInAsync(authUser, true, true);


            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        [AllowAnonymous]
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
                        OAuthToken oAuthToken = await AuthProxy.LoginAsync("valter.santos.matos@gmail.com", "qweqweqwe");
                        return RedirectToAction("Index", "Admin");
                    }

                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            //return View(result.Succeeded ? "ConfirmEmail" : "Error"); // Todo
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
}
