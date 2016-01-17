using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Auluxa.WebApp.Auth
{
    // Configure the application sign-in manager which is used in this application.  
    public class AuthSignInManager : SignInManager<AuthUser, string>
    {
        public AuthSignInManager(
            AuthUserManager userManager, 
            IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AuthUser user) => user.GenerateUserIdentityAsync((AuthUserManager)UserManager);

        public static AuthSignInManager Create(
            IdentityFactoryOptions<AuthSignInManager> options, 
            IOwinContext context) => new AuthSignInManager(context.GetUserManager<AuthUserManager>(), context.Authentication);
    }
}
