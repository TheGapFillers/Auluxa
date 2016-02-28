using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Auluxa.WebApp.Subscription
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> SubscribeToPlan(string userId, Subscription subscitption);
        Task<PaymentResult> PayAsync(string userId, Payment payment);

    }
}