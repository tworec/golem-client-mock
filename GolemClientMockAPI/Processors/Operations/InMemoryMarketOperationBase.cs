using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class InMemoryMarketOperationBase
    {
        protected ISubscriptionRepository SubscriptionRepository { get; set; }
        protected IProposalRepository ProposalRepository { get; set; }
        protected IAgreementRepository AgreementRepository { get; set; }

        protected GolemMarketResolver MarketResolver { get; set; } = new GolemMarketResolver();

        /// <summary>
        /// Requestor subscription pipelines, indexed by SubscriptionId
        /// </summary>
        protected IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> RequestorEventPipelines;

        /// <summary>
        /// Dictionary of Demand subscriptionIds indexed by Demand/Proposal Ids which have been issued in those subscriptions.
        /// </summary>
        protected IDictionary<string, string> DemandSubscriptions;

        /// <summary>
        /// Provider subscription pipelines, indexed by SubscriptionId
        /// </summary>
        protected IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> ProviderEventPipelines;

        /// <summary>
        /// Dictionary of Offer subscriptionIds indexed by Offer/Proposal Ids which have been issued in those subscriptions.
        /// </summary>
        protected IDictionary<string, string> OfferSubscriptions;

        public InMemoryMarketOperationBase(
            ISubscriptionRepository subscriptionRepo,
            IProposalRepository proposalRepo,
            IAgreementRepository agreementRepo,
            IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> requestorEventPipelines,
            IDictionary<string, string> demandSubscriptions,
            IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> providerEventPipelines,
            IDictionary<string, string> offerSubscriptions
            )
        {
            this.SubscriptionRepository = subscriptionRepo;
            this.ProposalRepository = proposalRepo;
            this.AgreementRepository = agreementRepo;
            this.RequestorEventPipelines = requestorEventPipelines;
            this.DemandSubscriptions = demandSubscriptions;
            this.ProviderEventPipelines = providerEventPipelines;
            this.OfferSubscriptions = offerSubscriptions;
        }

    }
}
