using System.Security.Claims;
using System.Threading.Tasks;
using Auluxa.WebApp.Subscription;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auluxa.WebApp.Auth
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class AuthUser : IdentityUser
    {
        public string ParentUserId { get; set; }

        public SubscriptionType SubscriptionType { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AuthUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
