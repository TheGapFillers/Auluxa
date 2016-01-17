using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Auluxa.WebApp.Auth
{
    public class AuthSmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message) => Task.FromResult(0);
    }
}
