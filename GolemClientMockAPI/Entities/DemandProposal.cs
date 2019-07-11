using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class DemandProposal : Proposal
    {
        public string OfferId { get; set; }
        public Demand Demand { get; set; }
    }
}
