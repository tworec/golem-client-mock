using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors.Operations.Activity
{
    public class InMemoryActivityOperationBase
    {
        protected IAgreementRepository AgreementRepository { get; set; }
        protected IActivityRepository ActivityRepository { get; set; }
        protected IDictionary<string, BlockingCollection<ActivityProviderEvent>> ProviderEventQueues { get; set; }

        /// <summary>
        /// ExeScript Result queues, indexed by Batch Ids
        /// </summary>
        protected IDictionary<string, BlockingCollection<ActivityRequestorEvent>> ExeBatchQueues { get; set; }

        /// <summary>
        /// Activity State Details queues, indexed by activityId
        /// </summary>
        protected IDictionary<string, BlockingCollection<ActivityRequestorEvent>> ActivityStateQueues { get; set; }

        public InMemoryActivityOperationBase(
            IAgreementRepository agreementRepo,
            IActivityRepository activityRepo,
            IDictionary<string, BlockingCollection<ActivityProviderEvent>> providerEventQueues,
            IDictionary<string, BlockingCollection<ActivityRequestorEvent>> exeBatchQueues
            )
        {
            this.AgreementRepository = agreementRepo;
            this.ActivityRepository = activityRepo;
            this.ProviderEventQueues = providerEventQueues;
            this.ExeBatchQueues = exeBatchQueues;
        }

        public InMemoryActivityOperationBase(
            IAgreementRepository agreementRepo,
            IActivityRepository activityRepo,
            IDictionary<string, BlockingCollection<ActivityProviderEvent>> providerEventQueues,
            IDictionary<string, BlockingCollection<ActivityRequestorEvent>> exeBatchQueues,
            IDictionary<string, BlockingCollection<ActivityRequestorEvent>> activityStateQueues
            )
        {
            this.AgreementRepository = agreementRepo;
            this.ActivityRepository = activityRepo;
            this.ProviderEventQueues = providerEventQueues;
            this.ExeBatchQueues = exeBatchQueues;
            this.ActivityStateQueues = activityStateQueues;
        }

        protected void EnsureProviderQueueExists(string providerNodeId)
        {
            if (!this.ProviderEventQueues.ContainsKey(providerNodeId))
            {
                lock(this.ProviderEventQueues)
                {
                    if (!this.ProviderEventQueues.ContainsKey(providerNodeId))
                        this.ProviderEventQueues.Add(providerNodeId, new BlockingCollection<ActivityProviderEvent>());
                }
            }

        }

        protected void EnsureExeBatchQueueExists(string batchId)
        {
            if (!this.ExeBatchQueues.ContainsKey(batchId))
            {
                lock (this.ExeBatchQueues)
                {
                    if (!this.ExeBatchQueues.ContainsKey(batchId))
                        this.ExeBatchQueues.Add(batchId, new BlockingCollection<ActivityRequestorEvent>());
                }
            }

        }

        protected void EnsureActivityStateQueueExists(string activityId)
        {
            if (!this.ActivityStateQueues.ContainsKey(activityId))
            {
                lock (this.ActivityStateQueues)
                {
                    if (!this.ActivityStateQueues.ContainsKey(activityId))
                        this.ActivityStateQueues.Add(activityId, new BlockingCollection<ActivityRequestorEvent>());
                }
            }

        }

    }
}
