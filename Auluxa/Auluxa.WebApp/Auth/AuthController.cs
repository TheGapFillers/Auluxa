using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Subscription;
using Microsoft.AspNet.Identity;

namespace Auluxa.WebApp.Auth
{
    [Authorize]
    [RoutePrefix("auth")]
    public class AuthController : ApiController
    {
        private readonly AuthUserManager _userManager;
        private readonly ISubscriptionRepository _subscriptuionRepository;

        public AuthController(AuthUserManager userManager, ISubscriptionRepository subscriptionRepository)
        {
            _userManager = userManager;
            _subscriptuionRepository = subscriptionRepository;
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

        [HttpPost]
        [Route("subscribe")]
        public async Task<IHttpActionResult> SubscribeToPlan(
            string email,
            [FromBody] Subscription.Subscription subscription)
        {
            // Get the user
            AuthUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("user", "This email has not been registered before.");
                return BadRequest(ModelState);
            }

            // subscribe the user to a new plan
            Subscription.Subscription createdSubscription = await _subscriptuionRepository.SubscribeToPlanAsync(
                user, subscription);

            // update the user repository
            user.ChargeBeeSubscriptionId = createdSubscription.Id;
            user.SubscriptionType = createdSubscription.SubscriptionType;
            IdentityResult updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
                return Ok(subscription);

            foreach (string error in updateResult.Errors)
                ModelState.AddModelError(error, error);
            return BadRequest(ModelState);
        }
    }
}