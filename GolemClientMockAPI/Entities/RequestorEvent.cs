using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class RequestorEvent
    {
        public enum RequestorEventType
        {
            Proposal,
            PropertyQuery
        }

        public RequestorEventType EventType { get; set; }

        public string ProviderId { get; set; }

        /// <summary>
        /// For EventType = Proposal - this contains the received Offer Proposal
        /// </summary>
        public OfferProposal OfferProposal { get; set; }
    }
}
