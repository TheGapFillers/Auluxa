using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auluxa.WebApp.Auth
{
    public class AuthUserStore : UserStore<AuthUser>
    {
        public AuthUserStore()
        {
            
        }

        public AuthUserStore(DbContext dbContext) 
            : base(dbContext)
        {
            
        }

        public async Task<IEnumerable<AuthUser>> GetUsersFromParentId(string parentId) => 
            await Users.Where(u => u.ParentUserId == parentId).ToListAsync();

        public async Task<AuthUser> GetUserFromParentId(string parentId, string userName) =>
            await Users.Where(u => 
            u.ParentUserId == parentId &&
            u.UserName == userName).SingleOrDefaultAsync();
    }
}