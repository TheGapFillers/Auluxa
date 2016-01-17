using System.Web.Mvc;

namespace Auluxa.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index() => View();
    }
}
