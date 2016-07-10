using Auluxa.WebApp.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Auluxa.WebApp.Controllers
{
    [Authorize]

    public class AdminController : Controller
    {
        /// <summary>
        /// Returns the main page.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            OAuthToken oAuthToken = await AuthProxy.LoginAsync("ambroise.couissin@gmail.com", "aaaa1111");
            ViewBag.token = oAuthToken.access_token;
            return View();
        }
    }
}