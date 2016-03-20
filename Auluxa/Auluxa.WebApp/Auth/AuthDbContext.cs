using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auluxa.WebApp.Auth
{
    public class AuthDbContext : IdentityDbContext<AuthUser>
    {
        public AuthDbContext()
            : base("IdentityConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDbContextInitializer());
        }

        public static AuthDbContext Create() => new AuthDbContext();

        public class ApplicationDbContextInitializer : DropCreateDatabaseIfModelChanges<AuthDbContext>
        {
            protected override void Seed(AuthDbContext context)
            {
                // intialize roles
                context.Roles.Add(new IdentityRole("Admin"));
                context.SaveChanges();
                base.Seed(context);
            }
        }
    }
}
