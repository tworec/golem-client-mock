using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class OfferSubscription : Subscription
    {
        public Offer Offer { get; set; }

    }
}
