using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ChargeBee.Api;

namespace Auluxa.WebApp.Subscription
{
    public class ChargebeeSubscriptionRepository : ISubscriptionRepository
    {
        public async Task<Subscription> SubscribeToPlan(string userId, Subscription subscitption)
        {
            ApiConfig.Configure("", "");

            ChargeBee.Models.Subscription chargebeeSubscription = await Task.Run(() =>
             ChargeBee.Models.Subscription.Create()
                .PlanId("Basic")
                .Request()
                .Subscription
            );

            var subscription = new Subscription
            {
                
            };

            return subscription;
        }

        public async Task<PaymentResult> PayAsync(string userId, Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}