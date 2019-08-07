using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;

namespace GolemClientMockAPI.Repository
{
    public class InProcessActivityRepository : IActivityRepository
    {
        public IDictionary<string, Activity> Activities { get; set; } = new Dictionary<string, Activity>();

        public IDictionary<string, ExeScript> ExeBatches { get; set; } = new Dictionary<string, ExeScript>();

        public Activity CreateActivity(string agreementId, string requestorNodeId, string providerNodeId)
        {
            var activity = new Activity()
            {
                AgreementId = agreementId,
                ProviderNodeId = providerNodeId,
                RequestorNodeId = requestorNodeId,
                Id = "" + Guid.NewGuid(),
                State = ActivityState.New
            };

            this.Activities.Add(activity.Id, activity);

            return activity;
        }

        public ExeScript CreateExeScriptBatch(string activityId, ExeScript exeScript)
        {
            exeScript.BatchId = "" + Guid.NewGuid();

            this.ExeBatches.Add(exeScript.BatchId, exeScript);

            return exeScript;
        }

        public Activity GetActivity(string activityId)
        {
            if(this.Activities.ContainsKey(activityId))
            {
                return this.Activities[activityId];
            }
            else
            {
                return null;
            }
        }

        public ExeScript GetExeScriptBatch(string batchId)
        {
            if (this.ExeBatches.ContainsKey(batchId))
            {
                return this.ExeBatches[batchId];
            }
            else
            {
                return null;
            }
        }

        public void SetActivityState(string activityId, ActivityState state)
        {
            if (this.Activities.ContainsKey(activityId))
            {
                this.Activities[activityId].State = state;
            }
            else
            {
                throw new Exception($"Activity Id {activityId} deos not exist...");
            }
        }
    }
}
