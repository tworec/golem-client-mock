using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Models
{
    public class SubscriptionStats
    {
        public string SubscriptionId { get; set; }
        public int ProposalCount { get; set; }
        public string Properties { get; set; }
        public string Constraints { get; set; }
    }
}
