using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
