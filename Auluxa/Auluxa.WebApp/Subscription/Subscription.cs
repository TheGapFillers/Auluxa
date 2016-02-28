using Auluxa.WebApp.Auth;

namespace Auluxa.WebApp.Subscription
{
    public class Subscription
    {
        public SubscriptionType SubscriptionType { get; set; }
        public decimal CostPerMonth { get; set; }
        public int UserCountLimitation { get; set; }
        public int ThirdPartyCountLimitation { get; set; }
        public decimal StorageLimitation { get; set; }
    }
}