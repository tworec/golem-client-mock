using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class SubscribeDemandOperation : InMemoryMarketOperationBase
    {
        public SubscribeDemandOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> providerEventPipelines) 
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, demandSubscriptions, providerEventPipelines, null)
        {

        }

        public DemandSubscription Run(Demand demand)
        {
            // 1. Persist subscription
            var demandSubscription = this.SubscriptionRepository.CreateDemandSubscription(demand);

            // 2. Construct the pipeline
            var pipeline = new SubscriptionPipeline<DemandSubscription, RequestorEvent>()
            {
                Subscription = demandSubscription
            };

            this.RequestorEventPipelines.Add(demandSubscription.Id, pipeline);

            // 3. Resolve the Demand against existing Offer subscriptions and:
            //    - pull the matching ones immediately to pipeline 
            //    - ???push the Demand into pipelines of matching Offers??? 
            //      this is questionable, as this would "duplicate" the message exchange - 
            //      - Providers would receive a "market proposal" from matching, and immediately respond with a counter proposal, 
            //        thus the Requestors would almost certainly receive a "market proposal" and a subsequent "direct proposal" 
            //        from most Providers (as there are few Requestors and many willing Providers)
            //      - So it is tempting to 'break the symmetry' and not forward the Demand automatically to Provider subscription pipelines.

            foreach (var offerSubscription in this.ProviderEventPipelines.Values)
            {
                var offer = offerSubscription.Subscription.Offer;

                var matchingResult = this.MarketResolver.MatchDemandOffer(demand.Properties.Select(prop => prop.Key + ((prop.Value == null) ? "" : ("=" + prop.Value))).ToArray(), demand.Constraints,
                                                     offer.Properties.Select(prop => prop.Key + ((prop.Value == null) ? "" : ("=" + prop.Value))).ToArray(), offer.Constraints);

                switch (matchingResult)
                {
                    case GolemMarketResolver.ResultEnum.True:
                        // Persist OfferProposal with 
                        var offerProposal = this.ProposalRepository.SaveOfferProposal(demandSubscription.Id, offerSubscription.Subscription.Id, offer); // no "previous proposal id" as we are mathcing the "on market" demands/offers
                        this.OfferSubscriptions.Add(offerProposal.Id, offerSubscription.Subscription.Id);

                        // Build Provider OfferProposal event and put in Requestor pipeline
                        var requestorEvent = new RequestorEvent()
                        {
                            EventType = RequestorEvent.RequestorEventType.Proposal,
                            OfferProposal = offerProposal,
                            ProviderId = offer.NodeId
                        };

                        if (!pipeline.PipelineQueue.TryAdd(requestorEvent))
                        {
                            // TODO what do we do if unable to Add (eg. due to collection full)?
                            // Log warning?
                            // Should iteration be stopped now (to allow the Requestor to collect results?) 
                            // What about other possible Offers that were skipped? they will not have a chance of being matched again...
                        }
                        break;
                    default:
                        break;
                }
            }

            return demandSubscription;

        }
    }
}
