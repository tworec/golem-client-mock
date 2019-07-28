using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class CollectProviderEventsOperation : InMemoryMarketOperationBase
    {
        public CollectProviderEventsOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions) 
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {

        }

        public async Task<ICollection<ProviderEvent>> Run(string subscriptionId, float? timeout, int? maxEvents)
        {
            var result = new List<ProviderEvent>();

            if (this.ProviderEventPipelines.ContainsKey(subscriptionId))
            {
                var pipeline = this.ProviderEventPipelines[subscriptionId];

                var subscription = this.SubscriptionRepository.GetOfferSubscription(subscriptionId);

                this.SubscriptionRepository.UpdateLastActive(subscriptionId, DateTime.Now);

                // First check if there aren't any unfetched demend proposals in the subscription
                // NOTE: This is for demand prospoals only. Should Agreement proposals be persisted in DB too???

                var recordedProposals = this.ProposalRepository.GetDemandProposals(subscriptionId, subscription.LastReceivedProposalId).OrderBy(prop => prop.InternalId);

                if (recordedProposals.Any())
                {
                    var lastProposalInternalId = recordedProposals.Last().InternalId;

                    // put the proposals in response
                    result.AddRange(recordedProposals.Select(prop => new ProviderEvent()
                    {
                        EventType = ProviderEvent.ProviderEventType.Proposal,
                        DemandProposal = prop,
                        RequestorId = prop.Demand.NodeId
                    }));

                    // if pipeline not empty, iterate reading from pipeline, ignoring the proposals which have been previously persisted

                    while (pipeline.PipelineQueue.Any())
                    {
                        var provEvent = pipeline.PipelineQueue.Take();

                        switch (provEvent.EventType)
                        {
                            case ProviderEvent.ProviderEventType.PropertyQuery:
                                result.Add(provEvent);
                                break;
                            case ProviderEvent.ProviderEventType.Proposal:
                                if (provEvent.DemandProposal.InternalId > lastProposalInternalId)
                                {
                                    result.Add(provEvent);
                                }
                                break;
                            case ProviderEvent.ProviderEventType.AgreementProposal:
                                result.Add(provEvent);
                                break;
                            default:
                                throw new Exception($"Unknown Request Event Type {provEvent.EventType}!");

                        }
                    }

                    // mark the last processed proposal id in the subscription
                    this.SubscriptionRepository.UpdateLastProposalId(subscriptionId, lastProposalInternalId);
                }
                else
                {
                    var receivedEvents = (await Task<ICollection<ProviderEvent>>.Run<ICollection<ProviderEvent>>(() =>
                    {
                        var pipelineResult = new List<ProviderEvent>();

                        if (pipeline.PipelineQueue.TryTake(out ProviderEvent reqEvent, (int)timeout))
                        {
                            pipelineResult.Add(reqEvent);
                        }

                        return pipelineResult;
                    }
                    ));

                    if (receivedEvents.Any())
                    {
                        // mark the last processed proposal id in the subscription
                        this.SubscriptionRepository.UpdateLastProposalId(subscriptionId, receivedEvents.Max(ev => ev.DemandProposal?.InternalId));

                        // put the proposals in response
                        result.AddRange(receivedEvents);
                    }
                }
            }

            return result;
        }
    }
}
