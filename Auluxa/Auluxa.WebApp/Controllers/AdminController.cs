using Auluxa.WebApp.Auth;
using System.Web.Mvc;

namespace Auluxa.WebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(object abs)
        {
            ////OAuthToken oAuthToken = await AuthProxy.LoginAsync("valter.santos.matos@gmail.com", "qweqweqwe");


            var bearer = "anY-DbKBR67wXKTU3XVLrLYTaggK3S-NYQSBdQyMflqBcOufFbnFIzQXbzYQICztzgaogCQnpscgbbqVsIvOIheHpKtOa83Adl5BlKklmIfw6UNKkPm7bFlGjxUy_KVs57F7OBNkZEulfOkx1ZGSBJrGSGK-_3ycCahhBGc_xVEK4xQsLQSsgcu0T4rV9kNySLaWiNJQoQSDBwe_P9u1rnEbSfxuTlYlR4wYv-xLus-lamx0dTeneWkaFJwYEJhj2pSOkb4MMCy2woK-gWRDGdHE6qfH_64G1FD4YQxalOaCDaZCu_DPvFruH0DEBv3Zi8vP2q5Th06fWRyzT6xwXgTDuY2XIFh-rtQBidlhA9ZvXoRWs0_G89EpRNs6fL3F_5N7sosOVDwFvJuUuDgRXVsiPAbszYB8l4l9uq3apvllvZy9yf4WYAwl0rZDswIi_z5qZSaPSd8ytEpaVm_jNxvgIYarxK0LCbUH1SpYEj6TkkWsY-uazEHf6BFGPjiNm7lSZfNhA9dxHH9zqfHqLA";


            return View(new Bearer { value = bearer });
        }

        public ActionResult LogOut()
        {
            // Log out the user.
            return RedirectToAction("Index", "Home");
        }
    }
}