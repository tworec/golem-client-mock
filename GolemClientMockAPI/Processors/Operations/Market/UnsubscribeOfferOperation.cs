using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class UnsubscribeOfferOperation : InMemoryMarketOperationBase
    {
        public UnsubscribeOfferOperation(ISubscriptionRepository subscriptionRepo,
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
            var offerSubscription = this.SubscriptionRepository.GetOfferSubscription(subscriptionId);

            if(offerSubscription == null)
            {
                throw new Exception($"Demand Subscription Id {subscriptionId} does not exist!");
            }

            // 2. Cleanup the pipeline and subscription dictionaries

            this.ProviderEventPipelines.Remove(offerSubscription.Id);
            this.OfferSubscriptions.Where(pair => pair.Value == offerSubscription.Id)
                .Select(pair => pair.Key)
                .ToList()
                .ForEach(key => { this.OfferSubscriptions.Remove(key); });

            this.SubscriptionRepository.DeleteOfferSubscription(offerSubscription.Id);

        }
    }
}
