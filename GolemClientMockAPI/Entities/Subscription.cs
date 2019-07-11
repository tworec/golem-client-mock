using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class Subscription
    {
        public string Id { get; set; }

        public long? LastReceivedProposalId { get; set; }

    }
}
