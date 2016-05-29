using System.Data.Entity;
using Microsoft.AspNet.Identity;
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

        public class ApplicationDbContextInitializer : DropCreateDatabaseAlways<AuthDbContext>
        {
            protected override void Seed(AuthDbContext context)
            {
                // initialize roles
                context.Roles.Add(new IdentityRole("Admin"));

                var userManager = new AuthUserManager(new UserStore<AuthUser>(context));
                userManager.Create(new AuthUser
                {
                    UserName = "ambroise.couissin@gmail.com",
                    Email = "ambroise.couissin@gmail.com",
                    EmailConfirmed = true,
                }, "aaaa1111");
                AuthUser ambroise = userManager.FindByName("ambroise.couissin@gmail.com");
                userManager.AddToRole(ambroise.Id, "Admin");

                context.SaveChanges();
                base.Seed(context);
            }
        }
    }
}
