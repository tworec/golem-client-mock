using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class MarketProviderEvent
    {
        public enum MarketProviderEventType
        {
            Proposal,
            AgreementProposal,
            PropertyQuery
        }

        public MarketProviderEventType EventType { get; set; }

        public string RequestorId { get; set; }

        /// <summary>
        /// For EventType = Proposal - this contains the received Demand Proposal
        /// </summary>
        public DemandProposal DemandProposal { get; set; }

        /// <summary>
        /// For EventType = AgreementProposal - this contains received Agreement
        /// </summary>
        public Agreement Agreement { get; set; }
    }
}
