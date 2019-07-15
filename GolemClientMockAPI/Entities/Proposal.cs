using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class Proposal
    {
        /// <summary>
        /// Proposal Id (as perceived by Proposal receiver)
        /// </summary>
        public string Id { get; set; }

        public int InternalId { get; set; }

        /// <summary>
        /// SubscriptionId of the sender (required to identify the sending pipeline)
        /// </summary>
        public string SendingSubscriptionId { get; set; }

        /// <summary>
        /// SubscriptionId of the receiver (required to identify the receiving pipeline)
        /// </summary>
        public string ReceivingSubscriptionId { get; set; }

    }
}
