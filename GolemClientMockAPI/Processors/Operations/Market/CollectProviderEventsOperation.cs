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
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, MarketRequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, MarketProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions) 
            : base(subscriptionRepo, proposalRepo, null, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {

        }

        public async Task<ICollection<MarketProviderEvent>> Run(string subscriptionId, float? timeout, int? maxEvents)
        {
            var result = new List<MarketProviderEvent>();

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
                    result.AddRange(recordedProposals.Select(prop => new MarketProviderEvent()
                    {
                        EventType = MarketProviderEvent.MarketProviderEventType.Proposal,
                        DemandProposal = prop,
                        RequestorId = prop.Demand.NodeId
                    }));

                    // if pipeline not empty, iterate reading from pipeline, ignoring the proposals which have been previously persisted

                    while (pipeline.PipelineQueue.Any())
                    {
                        var provEvent = pipeline.PipelineQueue.Take();

                        switch (provEvent.EventType)
                        {
                            case MarketProviderEvent.MarketProviderEventType.PropertyQuery:
                                result.Add(provEvent);
                                break;
                            case MarketProviderEvent.MarketProviderEventType.Proposal:
                                if (provEvent.DemandProposal.InternalId > lastProposalInternalId)
                                {
                                    result.Add(provEvent);
                                }
                                break;
                            case MarketProviderEvent.MarketProviderEventType.AgreementProposal:
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
                    var receivedEvents = (await Task<ICollection<MarketProviderEvent>>.Run<ICollection<MarketProviderEvent>>(() =>
                    {
                        var pipelineResult = new List<MarketProviderEvent>();

                        if (pipeline.PipelineQueue.TryTake(out MarketProviderEvent reqEvent, (int)timeout))
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
