using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class ExecOperation : InMemoryActivityOperationBase
    {
        public ExecOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityProviderEvent>> providerEventQueues) 
            : base(agreementRepo, activityRepo, providerEventQueues, null)
        {
        }

        public string Run(string activityId, ExeScript exeScript)
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

                // 1. Create the exeScript batch record

                var exeScriptBatch = this.ActivityRepository.CreateExeScriptBatch(activityId, exeScript);

                // 2. Send the Exec event to the Provider

                var providerEvent = new Entities.ActivityProviderEvent()
                {
                    EventType = Entities.ActivityProviderEvent.ActivityProviderEventType.Exec,
                    ActivityId = activity.Id,
                    ExeScript = exeScriptBatch
                };

                if( this.ProviderEventQueues.ContainsKey(providerNodeId) )
                {
                    this.ProviderEventQueues[providerNodeId].Add(providerEvent);
                }
                else
                {
                    throw new Exception($"Unable to find provider node id {providerNodeId} in ProviderEventQueues...");
                }

                return exeScriptBatch.BatchId;
            }
        }
    }
}
