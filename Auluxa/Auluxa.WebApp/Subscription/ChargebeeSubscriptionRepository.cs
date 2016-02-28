using System;
using System.Threading.Tasks;
using Auluxa.WebApp.Auth;
using ChargeBee.Api;

namespace Auluxa.WebApp.Subscription
{
    public class ChargebeeSubscriptionRepository : ISubscriptionRepository
    {
        public async Task<Subscription> SubscribeToPlanAsync(AuthUser user, Subscription subscription)
        {
            ApiConfig.Configure("auluxa-test", "test_HQFev3LA5amQop9cNCXjOnEor0kG4fmQ");

            string cbSubscriptionPlan;
            switch (subscription.SubscriptionType)
            {
                case SubscriptionType.Free: cbSubscriptionPlan = "free"; break;
                case SubscriptionType.Advanced: cbSubscriptionPlan = "advanced"; break;
                case SubscriptionType.Premium: cbSubscriptionPlan = "premium"; break;
                default: throw new ArgumentOutOfRangeException();
            }

            ChargeBee.Models.Subscription chargebeeSubscription = await Task.Run
            (() => 
                ChargeBee.Models.Subscription.Create()
                    .PlanId(cbSubscriptionPlan)
                    .CustomerEmail(user.Email)
                    .CustomerFirstName(user.UserName)
                    .CustomerPhone(user.PhoneNumber)
                    .Request().Subscription
            );

            return subscription;
        }

        public async Task<Subscription> GetSubscriptionAsync(AuthUser user)
        {
            ApiConfig.Configure("auluxa-test", "test_HQFev3LA5amQop9cNCXjOnEor0kG4fmQ");

            EntityResult cbResult = await Task.Run
            (() =>
                ChargeBee.Models.Subscription.Retrieve(user.ChargeBeeSubscriptionId)
                .Request()
            );

            SubscriptionType subType;
            switch (cbResult.Subscription.PlanId)
            {
                case "free": subType = SubscriptionType.Free; break;
                case "advanced": subType = SubscriptionType.Advanced; break;
                case "premium": subType = SubscriptionType.Premium; break;
                default: throw new ArgumentOutOfRangeException();
            }

            var subscription = new Subscription
            {
                SubscriptionType = subType,
                CostPerMonth = cbResult.Plan.Price,
            };

            return subscription;
        }

        public Task<Subscription> UpdateSubscriptionAsync(AuthUser user, Subscription subscription)
        {
            throw new NotImplementedException();
        }

        public Task<Subscription> DeleteSubscriptionAsync(AuthUser user, Subscription subscription)
        {
            throw new NotImplementedException();
        }

        public async Task<PaymentResult> PayAsync(string userId, Payment payment)
        {
            throw new NotImplementedException();
        }
    }
}