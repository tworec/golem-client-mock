using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations
{
    public class CreateAgreementOperation : InMemoryMarketOperationBase
    {
        public CreateAgreementOperation(ISubscriptionRepository subscriptionRepo,
                                        IProposalRepository proposalRepo,
                                        IAgreementRepository agreementRepo,
                                        IDictionary<string, SubscriptionPipeline<DemandSubscription, RequestorEvent>> requestorEventPipelines,
                                        IDictionary<string, string> demandSubscriptions,
                                        IDictionary<string, SubscriptionPipeline<OfferSubscription, ProviderEvent>> providerEventPipelines,
                                        IDictionary<string, string> offerSubscriptions) 
            : base(subscriptionRepo, proposalRepo, agreementRepo, requestorEventPipelines, demandSubscriptions, providerEventPipelines, offerSubscriptions)
        {
        }

        public Agreement Run(string demandSubscriptionId, string offerProposalId)
        {
            // 0. Validate the subscription
            var offerProposal = this.ProposalRepository.GetOfferProposal(offerProposalId);
            DemandSubscription subscription = this.SubscriptionRepository.GetDemandSubscription(offerProposal.ReceivingSubscriptionId);

            if (subscription != null)
            {
                // 1. Locate offer to which this demand is a response to, and put the OfferProposal in its Subscription pipeline

                var demand = offerProposal.DemandId == null ? 
                                subscription.Demand : 
                                this.ProposalRepository.GetDemandProposal(offerProposal.DemandId)?.Demand;

                if(offerProposal == null)
                {
                    throw new Exception($"OfferProposalId {offerProposalId} not found in Proposal repository...");
                }

                if(demand == null)
                {
                    throw new Exception($"Demand Id {offerProposal.DemandId} for Offer Proposal Id {offerProposalId} not found...");
                }

                // 2. Persist the agreement
                var agreement = this.AgreementRepository.CreateAgreement(demand, offerProposal);

                return agreement;
            }
            else
            {
                throw new Exception($"Demand Subscription {demandSubscriptionId} does not exist...");
            }
        }
    }
}
