using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class CollectActivityEventsOperation : InMemoryActivityOperationBase
    {
        public CollectActivityEventsOperation(
                IAgreementRepository agreementRepo, 
                IActivityRepository activityRepo,
                IDictionary<string, BlockingCollection<ActivityProviderEvent>> providerEventQueues) 
            : base(agreementRepo, activityRepo, providerEventQueues, null)
        {
        }

        public async Task<ICollection<ActivityProviderEvent>> RunAsync(string providerNodeId, int timeout)
        {
            var result = new List<ActivityProviderEvent>();

            this.EnsureProviderQueueExists(providerNodeId);

            // TODO implement mechanism to enable survival of server restart (ie persistence of received events)


            var receivedEvents = (await Task<ICollection<ActivityProviderEvent>>.Run<ICollection<ActivityProviderEvent>>(() =>
            {
                var pipelineResult = new List<ActivityProviderEvent>();

                if (this.ProviderEventQueues[providerNodeId].TryTake(out ActivityProviderEvent provEvent, timeout))
                {
                    pipelineResult.Add(provEvent);
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
