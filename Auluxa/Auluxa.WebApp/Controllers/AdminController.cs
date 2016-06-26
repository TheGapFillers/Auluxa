using System.Web.Mvc;

namespace Auluxa.WebApp.Controllers
{
   // [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            // Log out the user.
            return RedirectToAction("Index", "Home");
        }
    }
}