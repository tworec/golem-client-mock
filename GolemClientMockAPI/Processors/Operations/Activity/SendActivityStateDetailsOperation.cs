using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class SendActivityStateDetailsOperation : InMemoryActivityOperationBase
    {

        public SendActivityStateDetailsOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityRequestorEvent>> activityStateQueues) 
            : base(agreementRepo, activityRepo, null, null)
        {
            this.ActivityStateQueues = activityStateQueues;    
        }

        public void Run(string activityId, ActivityStateDetails details)
        {
            var activity = this.ActivityRepository.GetActivity(activityId);

            if(activity == null)
            {
                throw new Exception($"Activity {activityId} does not exist...");
            }

            this.EnsureActivityStateQueueExists(activityId);

            var requestorEvent = new ActivityRequestorEvent()
            {
                ActivityId = activityId,
                EventType = ActivityRequestorEvent.ActivityRequestorEventType.StateDetails,
                Details = details
            };

            this.ActivityStateQueues[activityId].Add(requestorEvent);
        }
    }
}
