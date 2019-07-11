using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class OfferProposal : Proposal
    {
        public string DemandId { get; set; }
        public Offer Offer { get; set; }
    }
}
