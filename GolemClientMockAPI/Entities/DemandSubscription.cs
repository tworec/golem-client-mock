using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class DemandSubscription : Subscription
    {
        public Demand Demand { get; set; }

    }
}
