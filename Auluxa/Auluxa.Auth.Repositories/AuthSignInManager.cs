using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auluxa.Auth.Repositories
{
    // Configure the application sign-in manager which is used in this application.  
    public class AuthSignInManager : SignInManager<AuthUser, string>
    {
        public AuthSignInManager(
            AuthUserManager userManager, 
            IAuthenticationManager authenticationManager) 
            : base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AuthUser user)
        {
            return user.GenerateUserIdentityAsync((AuthUserManager)UserManager);
        }

        public static AuthSignInManager Create(
            IdentityFactoryOptions<AuthSignInManager> options, 
            IOwinContext context)
        {
            return new AuthSignInManager(context.GetUserManager<AuthUserManager>(), context.Authentication);
        }
    }
}
