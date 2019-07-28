using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Models
{
    public class NodeStats
    {
        public string NodeId { get; set; }
        public DateTime Connected { get; set; }
        public DateTime LastActive { get; set; }
        public int SubscriptionCount { get; set; }
        public IEnumerable<SubscriptionStats> Subscriptions { get; set; }
    }
}
