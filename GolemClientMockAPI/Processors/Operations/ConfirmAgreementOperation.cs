using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class ConfirmAgreementOperation : InMemoryMarketOperationBase
    {
        public IDictionary<string, BlockingCollection<AgreementResultEnum>> AgreementResultPipelines { get; set; }

        public ConfirmAgreementOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IAgreementRepository agreementRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions,
                                        IDictionary<string, BlockingCollection<AgreementResultEnum>> agreementResultPipelines) 
            : base(subscriptionRepo, proposalRepo, agreementRepo, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {
            this.AgreementResultPipelines = agreementResultPipelines;
        }

        public async Task<AgreementResultEnum> Run(string agreementId, float? timeout)
        {
            // 0. Validate the agreement
            var agreement = this.AgreementRepository.GetAgreement(agreementId);

            if (agreement != null)
            {
                // 1. Set agreement state to Proposed and persist

                this.AgreementRepository.UpdateAgreementState(agreementId, AgreementState.Proposed);

                // 2. Send the Agreement in Proposed state to the Provider

                var providerSubscriptionId = this.ProposalRepository.GetOfferProposal(agreementId).Id;

                if (this.OfferSubscriptions.ContainsKey(providerSubscriptionId))
                {
                    var receivingOfferSubscription = this.ProviderEventPipelines[this.OfferSubscriptions[providerSubscriptionId]].Subscription;

                    this.ProviderEventPipelines[this.OfferSubscriptions[providerSubscriptionId]].PipelineQueue.Add(
                        new ProviderEvent()
                        {
                            EventType = ProviderEvent.ProviderEventType.AgreementProposal,
                            Agreement = agreement,
                            RequestorId = agreement.Demand.NodeId
                        });
                }
                else
                {
                    throw new Exception($"OfferId {providerSubscriptionId} not found in OfferSubscriptions mapping table...");
                }

                // 3. Hook into respective agreement response queue (until timeout expires) waiting for response from Provider

                this.AgreementResultPipelines[agreementId] = new BlockingCollection<AgreementResultEnum>();

                try
                {

                    var receivedResponse = (await Task<AgreementResultEnum>.Run<AgreementResultEnum>(() =>
                        {
                            var pipelineResult = new List<ProviderEvent>();

                            if (this.AgreementResultPipelines[agreementId].TryTake(out AgreementResultEnum response, (int)timeout))
                            {
                                return response;
                            }

                            return AgreementResultEnum.Timeout;
                        }
                    ));

                    return receivedResponse;
                }
                finally
                {
                    // cleanup the Agrement result pipeline...
                    this.AgreementResultPipelines.Remove(agreementId);
                }
            }
            else
            {
                throw new Exception($"Agreement Id {agreementId} does not exist...");
            }
        }
    }
}
