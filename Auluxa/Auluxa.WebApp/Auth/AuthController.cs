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
        public async Task<IHttpActionResult> RegisterAsync([FromBody]AuthUserViewModel authUserViewModel)
        {
            if (string.IsNullOrEmpty(authUserViewModel.Role))
            {
                ModelState.AddModelError("", "The role of the user is missing");
                return BadRequest(ModelState);
            }

            // transform view model to model
            var userToCreate = new AuthUser
            {
                UserName = authUserViewModel.Email,
                Email = authUserViewModel.Email,
            };

            // create the user
            IdentityResult result = await _userManager.CreateAsync(userToCreate, authUserViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (string error in result.Errors)
                    ModelState.AddModelError(error, error);
                return BadRequest(ModelState);
            }

            // assign user to specific role
            AuthUser createdUser = await _userManager.FindByEmailAsync(userToCreate.Email);
            IdentityResult roleResult = await _userManager.AddToRolesAsync(createdUser.Id, authUserViewModel.Role);
            if (!roleResult.Succeeded)
            {
                foreach (string error in result.Errors)
                    ModelState.AddModelError(error, error);
                return BadRequest(ModelState);
            }



            return Ok(result);
        }
    }
}