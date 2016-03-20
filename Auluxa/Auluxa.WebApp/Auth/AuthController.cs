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
            IdentityResult roleResult = await _userManager.AddToRolesAsync(createdUser.Id, "Admin");
            if (!roleResult.Succeeded)
            {
                foreach (string error in result.Errors)
                    ModelState.AddModelError(error, error);
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [AuluxaAuthorization(Roles = "Admin")]
        [HttpPost]
        [Route("createprofile")]
        public async Task<IHttpActionResult> CreateProfileAsync([FromBody] AuthUserViewModel authUserViewModel)
        {
            // Get the parentId
            AuthUser parentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (parentUser == null)
            {
                ModelState.AddModelError("", "This email has not been registered before.");
                return BadRequest(ModelState);
            }

            // transform view model to model
            var userToCreate = new AuthUser
            {
                UserName = authUserViewModel.Email,
                Email = authUserViewModel.Email,
                ParentUserId = parentUser.Id
            };

            // create the user
            IdentityResult result = await _userManager.CreateAsync(userToCreate, authUserViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (string error in result.Errors)
                    ModelState.AddModelError(error, error);
                return BadRequest(ModelState);
            }

            return Ok(result);
        }
    }
}