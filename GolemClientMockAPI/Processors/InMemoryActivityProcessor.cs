using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Processors.Operations.Activity;
using GolemClientMockAPI.Repository;

namespace GolemClientMockAPI.Processors
{
    public class InMemoryActivityProcessor : IRequestorActivityProcessor, IProviderActivityProcessor
    {
        public IAgreementRepository AgreementRepository { get; set; }
        public IActivityRepository ActivityRepository { get; set; }

        /// <summary>
        /// Collection of Provider Activity Event queues, indexed by Provider Node Id
        /// </summary>
        public IDictionary<string, BlockingCollection<ActivityProviderEvent>> ProviderEventQueues { get; set; } = new Dictionary<string, BlockingCollection<ActivityProviderEvent>>();

        /// <summary>
        /// Collection of Requestor Activity Event queues, indexed by ExeScriptBatchIds
        /// </summary>
        public IDictionary<string, BlockingCollection<ActivityRequestorEvent>> ExeScriptBatchQueues { get; set; } = new Dictionary<string, BlockingCollection<ActivityRequestorEvent>>();

        /// <summary>
        /// Collection of Requestor Activity Event queues, indexed by ActivityIds
        /// </summary>
        public IDictionary<string, BlockingCollection<ActivityRequestorEvent>> ActivityEventQueues { get; set; } = new Dictionary<string, BlockingCollection<ActivityRequestorEvent>>();


        public InMemoryActivityProcessor(IAgreementRepository agreementRepository,
            IActivityRepository activityRepository)
        {
            this.AgreementRepository = agreementRepository;
            this.ActivityRepository = activityRepository;
        }


        #region Provider operations
        public Task<ICollection<ActivityProviderEvent>> CollectActivityEventsAsync(string providerNodeId, int timeout)
        {
            return new CollectActivityEventsOperation(
                    this.AgreementRepository,
                    this.ActivityRepository,
                    this.ProviderEventQueues)
                .RunAsync(providerNodeId, timeout);
        }

        public void SendActivityExecResult(string activityId, string exeScriptBatchId, ActivityExecResult execResult)
        {
            new SendActivityExecResultsOperation(
                    this.AgreementRepository,
                    this.ActivityRepository,
                    this.ExeScriptBatchQueues)
                .Run(activityId, exeScriptBatchId, execResult);
        }

        public void SendActivityStateDetails(string activityId, ActivityStateDetails stateDetails)
        {
            new SendActivityStateDetailsOperation(
                    this.AgreementRepository,
                    this.ActivityRepository,
                    this.ExeScriptBatchQueues)
                .Run(activityId, stateDetails);
        }


        #endregion

        #region RequestorOperations

        public Activity CreateActivity(string agreementId)
        {
            return new CreateActivityOperation(
                    this.AgreementRepository,
                    this.ActivityRepository,
                    this.ProviderEventQueues)
                .Run(agreementId);

        }

        public void DestroyActivity(string activityId)
        {
            new DestroyActivityOperation(
                    this.AgreementRepository,
                    this.ActivityRepository,
                    this.ProviderEventQueues)
                .Run(activityId);
        }

        public string ExecAsync(string activityId, ExeScript exeScript)
        {
            return new ExecOperation(
                    this.AgreementRepository,
                    this.ActivityRepository,
                    this.ProviderEventQueues)
                .Run(activityId, exeScript);
        }

        public Task<ICollection<ActivityRequestorEvent>> GetExecBatchResultsAsync(string exeScriptBatchId, int timeout)
        {
            return new GetExecBatchResultsOperation(
                    this.AgreementRepository,
                    this.ActivityRepository,
                    this.ExeScriptBatchQueues)
                .RunAsync(exeScriptBatchId, timeout);
        }

        public ActivityStateDetails GetStateDetails(string activityId, int timeout)
        {
            new GetOperation(
                this.AgreementRepository,
                    this.ActivityRepository,
                    this.ProviderEventQueues,
                    this.ActivityEventQueues)
                    .Run(activityId, ActivityProviderEvent.ActivityProviderEventType.GetState);

            // wait for response (and return Unresponsive if timeout)
            if (this.ActivityEventQueues[activityId].TryTake(out ActivityRequestorEvent reqEvent, timeout))
            {
                return reqEvent.Details;
            }
            else
            {
                return new ActivityStateDetails()
                {
                    State = ActivityState.Unresponsive
                };
            };
        }

        #endregion

    }
}
