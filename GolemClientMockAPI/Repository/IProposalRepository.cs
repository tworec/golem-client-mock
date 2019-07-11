using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public interface IProposalRepository
    {
        /// <summary>
        /// Persists a Demand Proposal.
        /// </summary>
        /// <param name="subscriptionId">"Receiving" (Provider) subscription Id</param>
        /// <param name="demand"></param>
        /// <param name="offerId">If this Demand Proposal is a counter-proposal to a specific Offer, put OfferId here.</param>
        /// <returns></returns>
        DemandProposal SaveDemandProposal(string receivingSubscriptionId, Demand demand, string offerId = null);

        /// <summary>
        /// Persists an Offer Proposal.
        /// </summary>
        /// <param name="subscriptionId">"Receiving" (Requestor) subscription Id</param>
        /// <param name="offer"></param>
        /// <param name="demandId">If this Offer Proposal is a counter-proposal to a specific Demand, put DemandId here.</param>
        /// <returns></returns>
        OfferProposal SaveOfferProposal(string receivingSubscriptionId, Offer offer, string demandId = null);

        /// <summary>
        /// Get Proposals received by Subscription
        /// </summary>
        /// <param name="subscriptionId">Receiving (Requestor) subscription</param>
        /// <param name="lastReceivedProposalId"></param>
        /// <returns></returns>
        ICollection<OfferProposal> GetOfferProposals(string subscriptionId, long? lastReceivedProposalId = null);

        /// <summary>
        /// Get Proposals received by Subscription
        /// </summary>
        /// <param name="subscriptionId">Receiving (Provider) subscription</param>
        /// <param name="lastReceivedProposalId"></param>
        /// <returns></returns>
        ICollection<DemandProposal> GetDemandProposals(string subscriptionId, long? lastReceivedProposalId = null);
    }
}
