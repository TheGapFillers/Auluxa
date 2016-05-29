using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Auluxa.WebApp.Auth
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly AuthUserManager _userManager;

        public UsersController(AuthUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> GetCurrentUserAsync()
        {
            AuthUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            var userVm = new AuthUserViewModel
            {
                Email = user.Email,
                Role = user.Roles.Any() ? "Admin" : null
            };

            return Ok(userVm);
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
        [HttpGet]
        [Route("profiles")]
        public async Task<IHttpActionResult> GetProfilesAsync()
        {
            // Get the parentId
            AuthUser parentUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            // Get all the child profiles
            IEnumerable<AuthUser> users = await _userManager.GetUsersFromParentId(parentUser.Id);

            // Get all the roles
            IEnumerable<AuthUserViewModel> userVms = users.Concat(new[] { parentUser }).Select(u =>
               new AuthUserViewModel
               {
                   Email = u.Email,
                   Role = u.Roles.Any() ? "Admin" : null
               });

            return Ok(userVms);
        }

        [AuluxaAuthorization(Roles = "Admin")]
        [HttpDelete]
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

        [AuluxaAuthorization(Roles = "Admin")]
        [HttpDelete]
        [Route("profiles")]
        public async Task<IHttpActionResult> DeleteProfileAsync(string email)
        {
            if (string.Equals(email, User.Identity.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                ModelState.AddModelError("", "You cannot delete your own profile.");
                return BadRequest(ModelState);
            }

            AuthUser parentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            AuthUser user = await _userManager.GetUserFromParentId(parentUser.ParentUserId, email);
            if (user == null)
            {
                ModelState.AddModelError("", "This profile doesn't exist.");
                return BadRequest(ModelState);
            }

            if (await _userManager.IsInRoleAsync(user.Id, "Admin"))
            {
                ModelState.AddModelError("", "You cannot delete an Admin.");
                return BadRequest(ModelState);
            }

            // delete the user
            IdentityResult result = await _userManager.DeleteAsync(user);
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