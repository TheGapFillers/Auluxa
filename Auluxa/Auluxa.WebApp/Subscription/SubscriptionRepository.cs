using System.Threading.Tasks;
using Auluxa.WebApp.Auth;

namespace Auluxa.WebApp.Subscription
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> SubscribeToPlanAsync(AuthUser user, Subscription subscription);
        Task<Subscription> GetSubscriptionAsync(AuthUser user);
        Task<Subscription> UpdateSubscriptionAsync(AuthUser user, Subscription subscription);
        Task<Subscription> DeleteSubscriptionAsync(AuthUser user, Subscription subscription);
        Task<PaymentResult> PayAsync(string userId, Payment payment);

    }
}