using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class CreateOfferProposalOperation : InMemoryMarketOperationBase
    {
        public CreateOfferProposalOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions) 
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {

        }

        public OfferProposal Run(string offerSubscriptionId, string demandProposalId, Offer offer)
        {
            // 0. Validate the subscription
            var subscription = this.SubscriptionRepository.GetOfferSubscription(offerSubscriptionId);

            if (subscription != null)
            {
                // 1. Locate demand to which this offer is a response to, and put the OfferProposal in its Subscription pipeline

                if (this.DemandSubscriptions.ContainsKey(demandProposalId))
                {
                    var receivingDemandSubscription = this.RequestorEventPipelines[this.DemandSubscriptions[demandProposalId]].Subscription;

                    // 2. Persist the offer proposal
                    var offerProposal = this.ProposalRepository.SaveOfferProposal(receivingDemandSubscription.Id, offerSubscriptionId, offer, demandProposalId);

                    this.OfferSubscriptions.Add(offerProposal.Id, offerSubscriptionId);

                    // TODO should matching be checked here as well between the demand and responding offer proposal?

                    this.RequestorEventPipelines[this.DemandSubscriptions[demandProposalId]].PipelineQueue.Add(
                        new RequestorEvent()
                        {
                            EventType = RequestorEvent.RequestorEventType.Proposal,
                            OfferProposal = offerProposal,
                            ProviderId = subscription.Offer.NodeId
                        });

                    return offerProposal;
                }
                else
                {
                    throw new Exception($"DemandId {demandProposalId} not found in DemandSubscriptions mapping table...");
                }
            }
            else
            {
                throw new Exception($"Offer Subscription {offerSubscriptionId} does not exist...");
            }

        }
    }
}
