using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Auluxa.WebApp.Subscription
{
    public class Subscription
    {
        private static readonly List<Subscription> Subscriptions = new List<Subscription>
        {
            new Subscription {SubscriptionType = SubscriptionType.Free},
            new Subscription { SubscriptionType = SubscriptionType.Advanced},
            new Subscription {SubscriptionType = SubscriptionType.Premium}
        };

        public string Id { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public decimal CostPerMonth { get; set; }
        public int UserCountLimitation { get; set; }
        public int ThirdPartyCountLimitation { get; set; }
        public decimal StorageLimitation { get; set; }

        public static Subscription FromType(SubscriptionType type) =>
            Subscriptions.Find(s => s.SubscriptionType == type);
    }

    public class SubscriptionViewModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SubscriptionType SubscriptionType { get; set; }
    }
}