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
        DemandProposal SaveDemandProposal(string receivingSubscriptionId, string sendingSubscriptionId, Demand demand, string offerProposalId = null);

        /// <summary>
        /// Persists an Offer Proposal.
        /// </summary>
        /// <param name="subscriptionId">"Receiving" (Requestor) subscription Id</param>
        /// <param name="offer"></param>
        /// <param name="demandId">If this Offer Proposal is a counter-proposal to a specific Demand, put DemandId here.</param>
        /// <returns></returns>
        OfferProposal SaveOfferProposal(string receivingSubscriptionId, string sendingSubscriptionId, Offer offer, string demandProposalId = null);

        /// <summary>
        /// Get Proposals received by Subscription
        /// </summary>
        /// <param name="subscriptionId">Receiving (Requestor) subscription</param>
        /// <param name="lastReceivedProposalId"></param>
        /// <returns></returns>
        ICollection<OfferProposal> GetOfferProposals(string receivingSubscriptionId, long? lastReceivedProposalInternalId = null);

        /// <summary>
        /// Get Offer Proposal by Proposal Id
        /// </summary>
        /// <param name="offerProposalId">Offer Id of the proposal</param>
        /// <returns></returns>
        OfferProposal GetOfferProposal(string offerProposalId);

        /// <summary>
        /// Get Proposals received by Subscription
        /// </summary>
        /// <param name="subscriptionId">Receiving (Provider) subscription</param>
        /// <param name="lastReceivedProposalId"></param>
        /// <returns></returns>
        ICollection<DemandProposal> GetDemandProposals(string receivingSubscriptionId, long? lastReceivedProposalInternalId = null);

        /// <summary>
        /// Get Demand Proposal by Proposal Id
        /// </summary>
        /// <param name="demandProposalId">Demand Id of the proposal</param>
        /// <returns></returns>
        DemandProposal GetDemandProposal(string demandProposalId);

    }
}
