using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class UnsubscribeDemandOperation : InMemoryMarketOperationBase
    {
        public UnsubscribeDemandOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, MarketRequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, MarketProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions) 
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {

        }

        public void Run(string subscriptionId)
        {
            // 1. Validate subscription
            var demandSubscription = this.SubscriptionRepository.GetDemandSubscription(subscriptionId);

            if(demandSubscription == null)
            {
                throw new Exception($"Demand Subscription Id {subscriptionId} does not exist!");
            }

            // 2. Cleanup the pipeline and subscription dictionaries

            this.RequestorEventPipelines.Remove(demandSubscription.Id);
            this.DemandSubscriptions.Where(pair => pair.Value == demandSubscription.Id)
                .Select(pair => pair.Key)
                .ToList()
                .ForEach(key => { this.DemandSubscriptions.Remove(key); });

            this.SubscriptionRepository.DeleteDemandSubscription(demandSubscription.Id);

        }
    }
}
