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
        public async Task<IEnumerable<AuthUser>> GetUsersFromParentId(string parentId) => 
            await Users.Where(u => u.ParentUserId == parentId).ToListAsync();
    }
}