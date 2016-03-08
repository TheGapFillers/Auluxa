using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Auluxa.WebApp.Auth
{
    [Authorize]
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        private readonly AuthUserManager _userManager;

        public AuthController(AuthUserManager userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")] 
        public async Task<IHttpActionResult> RegisterAsync([FromBody]AuthUser authUser)
        {
            // create the user
            IdentityResult result = await _userManager.CreateAsync(authUser);
            if (result.Succeeded)
                return Ok(result);

            foreach (string error in result.Errors)
                ModelState.AddModelError(error, error);
            return BadRequest(ModelState);
        }
    }
}