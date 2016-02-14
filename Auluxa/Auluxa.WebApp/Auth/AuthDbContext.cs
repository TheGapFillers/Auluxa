using Microsoft.AspNet.Identity.EntityFramework;

namespace Auluxa.WebApp.Auth
{
    public class AuthDbContext : IdentityDbContext<AuthUser>
    {
        public AuthDbContext()
            : base("IdentityConnection", throwIfV1Schema: false)
        {
        }

        public static AuthDbContext Create() => new AuthDbContext();
    }
}
