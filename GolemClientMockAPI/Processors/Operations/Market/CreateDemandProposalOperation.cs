using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class CreateDemandProposalOperation : InMemoryMarketOperationBase
    {
        public CreateDemandProposalOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, MarketRequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, MarketProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions) 
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {

        }

        public DemandProposal Run(string demandSubscriptionId, string offerProposalId, Demand demand)
        {
            // 0. Validate the subscription
            var subscription = this.SubscriptionRepository.GetDemandSubscription(demandSubscriptionId);

            if (subscription != null)
            {
                // 1. Locate offer to which this demand is a response to, and put the OfferProposal in its Subscription pipeline

                if (this.OfferSubscriptions.ContainsKey(offerProposalId))
                {
                    var receivingOfferSubscription = this.ProviderEventPipelines[this.OfferSubscriptions[offerProposalId]].Subscription;

                    // 2. Persist the demand proposal
                    var demandProposal = this.ProposalRepository.SaveDemandProposal(receivingOfferSubscription.Id, demandSubscriptionId, demand, offerProposalId);

                    this.DemandSubscriptions.Add(demandProposal.Id, demandSubscriptionId);

                    // TODO should matching be checked here as well between the demand and responding offer proposal?

                    this.ProviderEventPipelines[this.OfferSubscriptions[offerProposalId]].PipelineQueue.Add(
                        new MarketProviderEvent()
                        {
                            EventType = MarketProviderEvent.MarketProviderEventType.Proposal,
                            DemandProposal = demandProposal,
                            RequestorId = demandProposal.Demand.NodeId
                        });

                    return demandProposal;
                }
                else
                {
                    throw new Exception($"Offer ProposalId {offerProposalId} not found in OfferSubscriptions mapping table...");
                }
            }
            else
            {
                throw new Exception($"Demand Subscription {demandSubscriptionId} does not exist...");
            }
        }
    }
}
