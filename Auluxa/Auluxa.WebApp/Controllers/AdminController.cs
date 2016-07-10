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
            string token = Session["token"] as string;
            ViewBag.token = token;
            ViewBag.user = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
