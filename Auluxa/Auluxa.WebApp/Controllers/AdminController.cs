using Auluxa.WebApp.Auth;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Auluxa.WebApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public async Task<ActionResult> Index(object abs)
        {            
            OAuthToken oAuthToken = await AuthProxy.LoginAsync("ambroise.couissin@gmail.com", "aaaa1111");
            ViewBag.token = oAuthToken.access_token;
            return View();
        }

        public ActionResult LogOut()
        {
            // Log out the user.
            return RedirectToAction("Index", "Home");
        }
    }
}