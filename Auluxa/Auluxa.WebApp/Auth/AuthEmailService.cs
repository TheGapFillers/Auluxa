using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Auluxa.WebApp.Auth
{
    public class AuthEmailService : IIdentityMessageService
    {
        // Plug in your email service here to send an email.
        public Task SendAsync(IdentityMessage message) => Task.FromResult(0);
    }
}
