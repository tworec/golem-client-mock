using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class CollectRequestorEventsOperation : InMemoryMarketOperationBase
    {
        public CollectRequestorEventsOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> requestorEventPipelines) 
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, null, null, null)
        {

        }

        public async Task<ICollection<RequestorEvent>> Run(string subscriptionId, float? timeout, int? maxEvents)
        {
            var result = new List<RequestorEvent>();

            if (this.RequestorEventPipelines.ContainsKey(subscriptionId))
            {
                var pipeline = this.RequestorEventPipelines[subscriptionId];

                var subscription = this.SubscriptionRepository.GetDemandSubscription(subscriptionId);

                // First check if there aren't any unfetched events in the subscription

                var recordedProposals = this.ProposalRepository.GetOfferProposals(subscriptionId, subscription.LastReceivedProposalId).OrderBy(prop => prop.InternalId);

                if (recordedProposals.Any())
                {
                    var lastProposalInternalId = recordedProposals.Last().InternalId;

                    // put the proposals in response
                    result.AddRange(recordedProposals.Select(prop => new RequestorEvent()
                    {
                        EventType = RequestorEvent.RequestorEventType.Proposal,
                        OfferProposal = prop,
                        ProviderId = prop.Offer.NodeId
                    }));

                    // if pipeline not empty, iterate reading from pipeline, ignoring the proposals which have been previously persisted

                    while (pipeline.PipelineQueue.Any())
                    {
                        var reqEvent = pipeline.PipelineQueue.Take();

                        switch (reqEvent.EventType)
                        {
                            case RequestorEvent.RequestorEventType.PropertyQuery:
                                result.Add(reqEvent);
                                break;
                            case RequestorEvent.RequestorEventType.Proposal:
                                if (reqEvent.OfferProposal.InternalId > lastProposalInternalId)
                                {
                                    result.Add(reqEvent);
                                }
                                break;
                            default:
                                throw new Exception($"Unknown Request Event Type {reqEvent.EventType}!");

                        }
                    }

                    // mark the last processed proposal id in the subscription
                    this.SubscriptionRepository.UpdateLastProposalId(subscriptionId, lastProposalInternalId);
                }
                else
                {
                    var receivedEvents = (await Task<ICollection<RequestorEvent>>.Run<ICollection<RequestorEvent>>(() =>
                    {
                        var pipelineResult = new List<RequestorEvent>();

                        if (pipeline.PipelineQueue.TryTake(out RequestorEvent reqEvent, (int)timeout))
                        {
                            pipelineResult.Add(reqEvent);
                        }

                        return pipelineResult;
                    }
                    ));

                    if (receivedEvents.Any())
                    {
                        // mark the last processed proposal id in the subscription
                        this.SubscriptionRepository.UpdateLastProposalId(subscriptionId, receivedEvents.Max(ev => ev.OfferProposal?.InternalId));

                        // put the proposals in response
                        result.AddRange(receivedEvents);
                    }
                }
            }

            return result;
        }
    }
}
