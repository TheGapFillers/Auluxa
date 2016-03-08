using System.Threading.Tasks;
using System.Web.Http;
using Auluxa.WebApp.Auth;
using Microsoft.AspNet.Identity;

namespace Auluxa.WebApp.Subscription
{
    [Authorize]
    [RoutePrefix("subscription")]
    public class SubscriptionController : ApiController
    {
        private readonly AuthUserManager _userManager;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionController(AuthUserManager userManager, ISubscriptionRepository subscriptionRepository)
        {
            _userManager = userManager;
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> SubscribeToPlan(string email, [FromBody]Subscription subscription)
        {
            // Get the user
            AuthUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("user", "This email has not been registered before.");
                return BadRequest(ModelState);
            }

            // subscribe the user to a new plan
            Subscription createdSubscription = await _subscriptionRepository.SubscribeToPlanAsync(
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