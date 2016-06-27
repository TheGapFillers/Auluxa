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
