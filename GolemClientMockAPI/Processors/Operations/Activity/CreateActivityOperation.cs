using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class CreateActivityOperation : InMemoryActivityOperationBase
    {
        public CreateActivityOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityProviderEvent>> providerEventQueues) 
            : base(agreementRepo, activityRepo, providerEventQueues, null)
        {
        }

        public GolemClientMockAPI.Entities.Activity Run(string agreementId)
        {
            var agreement = this.AgreementRepository.GetAgreement(agreementId);

            if(agreement == null)
            {
                throw new Exception($"Agreement {agreementId} does not exist...");
            }
            else
            {
                var requestorNodeId = agreement.Demand.NodeId;
                var providerNodeId = agreement.Offer.NodeId;

                // 1. Create the activity record

                var activity = this.ActivityRepository.CreateActivity(agreementId, requestorNodeId, providerNodeId);

                // 2. Send the CreateActivity event to the Provider

                var providerEvent = new Entities.ActivityProviderEvent()
                {
                    EventType = Entities.ActivityProviderEvent.ActivityProviderEventType.CreateActivity,
                    ActivityId = activity.Id
                };

                this.EnsureProviderQueueExists(providerNodeId);

                this.ProviderEventQueues[providerNodeId].Add(providerEvent);

                // TODO: consider the activity states here - what should the activity state be at this point...???

                return activity;
            }
        }
    }
}
