using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            IdentityResult roleResult = await _userManager.AddToRolesAsync(createdUser.Id, "SuperAdmin");
            if (!roleResult.Succeeded)
            {
                foreach (string error in result.Errors)
                    ModelState.AddModelError(error, error);
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [AuluxaAuthorization(Roles = "SuperAdmin")]
        [HttpGet]
        [Route("profiles")]
        public async Task<IHttpActionResult> GetProfilesAsync()
        {
            // Get the parentId
            AuthUser parentUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            // Get all the child profiles
            IEnumerable<AuthUser> users = await _userManager.GetUsersFromParentId(parentUser.Id);

            IEnumerable<AuthUserViewModel> userVms = users.Select(u =>
                new AuthUserViewModel
                {
                    Email = u.Email
                });

            return Ok(userVms);
        }

        [AuluxaAuthorization(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("profiles")]
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