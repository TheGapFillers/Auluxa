using Microsoft.AspNet.Identity.EntityFramework;

namespace Auluxa.Auth.Repositories
{
    public class AuthDbContext : IdentityDbContext<AuthUser>
    {
        public AuthDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static AuthDbContext Create()
        {
            return new AuthDbContext();
        }
    }
}
