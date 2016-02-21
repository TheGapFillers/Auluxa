using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auluxa.WebApp.Auth
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