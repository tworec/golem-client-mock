using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class GetExecBatchResultsOperation : InMemoryActivityOperationBase
    {
        public GetExecBatchResultsOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityRequestorEvent>> exeBatchQueues) 
            : base(agreementRepo, activityRepo, null, exeBatchQueues)
        {
        }

        public async Task<ICollection<ActivityRequestorEvent>> RunAsync(string exeScriptBatchId, int timeout)
        {
            var result = new List<ActivityRequestorEvent>();

            this.EnsureExeBatchQueueExists(exeScriptBatchId);

            // TODO implement mechanism to enable survival of server restart (ie persistence of received events)


            var receivedEvents = (await Task<ICollection<ActivityRequestorEvent>>.Run<ICollection<ActivityRequestorEvent>>(() =>
            {
                var pipelineResult = new List<ActivityRequestorEvent>();

                if (this.ExeBatchQueues[exeScriptBatchId].TryTake(out ActivityRequestorEvent reqEvent, timeout))
                {
                    pipelineResult.Add(reqEvent);
                }

                return pipelineResult;
            }
            ));

            if (receivedEvents.Any())
            {
                // mark the last processed proposal id in the subscription
                // TODO implement mechanism to enable survival of server restart

                // put the proposals in response
                result.AddRange(receivedEvents);
            }

            return result;
        }
    }
}
