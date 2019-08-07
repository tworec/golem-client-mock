using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class GetOperation : InMemoryActivityOperationBase
    {
        public GetOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityProviderEvent>> providerEventQueues,
                IDictionary<string, BlockingCollection<ActivityRequestorEvent>> activityEventQueues) 
            : base(agreementRepo, activityRepo, providerEventQueues, null, activityEventQueues)
        {
        }

        public void Run(string activityId, ActivityProviderEvent.ActivityProviderEventType eventType)
        {
            var activity = this.ActivityRepository.GetActivity(activityId);

            if(activity == null)
            {
                throw new Exception($"Activity {activityId} does not exist...");
            }
            else
            {
                var requestorNodeId = activity.RequestorNodeId;
                var providerNodeId = activity.ProviderNodeId;

                // 1. Send the event to Provider

                var providerEvent = new Entities.ActivityProviderEvent()
                {
                    EventType = eventType,
                    ActivityId = activity.Id
                };

                if( this.ProviderEventQueues.ContainsKey(providerNodeId) )
                {
                    this.ProviderEventQueues[providerNodeId].Add(providerEvent);
                }
                else
                {
                    throw new Exception($"Unable to find provider node id {providerNodeId} in ProviderEventQueues...");
                }
            }
        }
    }
}
