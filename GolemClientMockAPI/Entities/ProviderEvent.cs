using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class ProviderEvent
    {
        public enum ProviderEventType
        {
            Proposal,
            AgreementProposal,
            PropertyQuery
        }

        public ProviderEventType EventType { get; set; }

        public string RequestorId { get; set; }

        /// <summary>
        /// For EventType = Proposal - this contains the received Demand Proposal
        /// </summary>
        public DemandProposal DemandProposal { get; set; }

    }
}
