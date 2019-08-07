using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class SendActivityExecResultsOperation : InMemoryActivityOperationBase
    {

        public SendActivityExecResultsOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityRequestorEvent>> exeBatchQueues) 
            : base(agreementRepo, activityRepo, null, exeBatchQueues)
        {
            
        }

        public void Run(string activityId, string exeScriptBatchId, ActivityExecResult execResult)
        {
            var activity = this.ActivityRepository.GetActivity(activityId);

            if(activity == null)
            {
                throw new Exception($"Activity {activityId} does not exist...");
            }

            this.EnsureExeBatchQueueExists(exeScriptBatchId);

            var requestorEvent = new ActivityRequestorEvent()
            {
                ActivityId = activityId,
                EventType = ActivityRequestorEvent.ActivityRequestorEventType.ExecResult,
                ExecResult = execResult
            };

            this.ExeBatchQueues[exeScriptBatchId].Add(requestorEvent);
        }
    }
}
