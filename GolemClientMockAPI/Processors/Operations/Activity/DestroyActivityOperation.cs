using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class DestroyActivityOperation : InMemoryActivityOperationBase
    {
        public DestroyActivityOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityProviderEvent>> providerEventQueues) 
            : base(agreementRepo, activityRepo, providerEventQueues, null)
        {
        }

        public GolemClientMockAPI.Entities.Activity Run(string activityId)
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

                // 1. TODO Update state of the activity record

                // this.ActivityRepository.SetActivityState(activityId, ActivityState.)

                // 2. Send the DestroyActivity event to the Provider

                var providerEvent = new Entities.ActivityProviderEvent()
                {
                    EventType = Entities.ActivityProviderEvent.ActivityProviderEventType.DestroyActivity,
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

                return activity;
            }
        }
    }
}
