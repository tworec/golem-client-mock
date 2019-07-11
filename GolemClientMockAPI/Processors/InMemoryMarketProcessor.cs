using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Processors.Operations;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors
{
    public class SubscriptionPipeline<T, U> where T : Subscription
    {
        public T Subscription { get; set; }

        public BlockingCollection<U> PipelineQueue { get; set; } = new BlockingCollection<U>();

    }

    public class InMemoryMarketProcessor : IRequestorMarketProcessor, IProviderMarketProcessor
    {
        public ISubscriptionRepository SubscriptionRepository { get; set; }
        public IProposalRepository ProposalRepository { get; set; }
        public IAgreementRepository AgreementRepository { get; set; }

        public GolemMarketResolver MarketResolver { get; set; } = new GolemMarketResolver();

        /// <summary>
        /// Requestor subscription pipelines, indexed by SubscriptionId
        /// </summary>
        protected IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> RequestorEventPipelines = new Dictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>>();

        /// <summary>
        /// Dictionary of Demand subscriptionIds indexed by Demand/Proposal Ids which have been issued in those subscriptions.
        /// </summary>
        protected IDictionary<string, string> DemandSubscriptions = new Dictionary<string, string>();

        /// <summary>
        /// Provider subscription pipelines, indexed by SubscriptionId
        /// </summary>
        protected IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> ProviderEventPipelines = new Dictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>>();

        /// <summary>
        /// Dictionary of Offer subscriptionIds indexed by Offer/Proposal Ids which have been issued in those subscriptions.
        /// </summary>
        protected IDictionary<string, string> OfferSubscriptions = new Dictionary<string, string>();


        public InMemoryMarketProcessor(ISubscriptionRepository subscriptionRepository, 
                                       IProposalRepository proposalRepository,
                                       IAgreementRepository agreementRepository)
        {
            this.SubscriptionRepository = subscriptionRepository;
            this.ProposalRepository = proposalRepository;
            this.AgreementRepository = agreementRepository;
        }

        #region Requestor interface

        public DemandSubscription SubscribeDemand(Demand demand)
        {
            return new SubscribeDemandOperation(
                this.SubscriptionRepository,
                this.ProposalRepository, 
                this.RequestorEventPipelines,
                this.DemandSubscriptions,
                this.ProviderEventPipelines
                ).Run(demand);
        }

        public Task<ICollection<RequestorEvent>> CollectRequestorEventsAsync(string subscriptionId, float? timeout, int? maxEvents)
        {
            return new CollectRequestorEventsOperation(
                this.SubscriptionRepository,
                this.ProposalRepository,
                this.RequestorEventPipelines
                ).Run(subscriptionId, timeout, maxEvents);
        }

        public DemandProposal CreateDemandProposal(string demandSubscriptionId, string offerProposalId, Demand demand)
        {
            return new CreateDemandProposalOperation(
                this.SubscriptionRepository,
                this.ProposalRepository,
                this.RequestorEventPipelines,
                this.DemandSubscriptions,
                this.ProviderEventPipelines,
                this.OfferSubscriptions
                ).Run(demandSubscriptionId, offerProposalId, demand);
        }

        public Agreement CreateAgreement(string subscriptionId, string offerProposalId)
        {
            throw new NotImplementedException();
        }

        public void ConfirmAgreement(string agreementId, float? timeout)
        {
            throw new NotImplementedException();
        }

        public void CancelAgreement(string agreementId)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeDemand(string subscriptionId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Provider interface

        public OfferSubscription SubscribeOffer(Offer offer)
        {
            return new SubscribeOfferOperation(
                this.SubscriptionRepository,
                this.ProposalRepository,
                this.RequestorEventPipelines,
                this.DemandSubscriptions,
                this.ProviderEventPipelines,
                this.OfferSubscriptions
                ).Run(offer);
        }

        public Task<ICollection<ProviderEvent>> CollectProviderEventsAsync(string subscriptionId, float? timeout, int? maxEvents)
        {
            return new CollectProviderEventsOperation(
                this.SubscriptionRepository,
                this.ProposalRepository,
                this.RequestorEventPipelines,
                this.DemandSubscriptions,
                this.ProviderEventPipelines,
                this.OfferSubscriptions
                ).Run(subscriptionId, timeout, maxEvents);
        }

        public OfferProposal CreateOfferProposal(string offerSubscriptionId, string demandProposalId, Offer offer)
        {
            return new CreateOfferProposalOperation(
                this.SubscriptionRepository,
                this.ProposalRepository,
                this.RequestorEventPipelines,
                this.DemandSubscriptions,
                this.ProviderEventPipelines,
                this.OfferSubscriptions
                ).Run(offerSubscriptionId, demandProposalId, offer);
        }

        public void RejectAgreement(string agreementId)
        {
            throw new NotImplementedException();
        }

        public Agreement ApproveAgreement(string agreementId)
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeOffer(string subscriptionId)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
