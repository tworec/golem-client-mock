using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public interface IActivityRepository
    {
        Activity CreateActivity(string agreementId, string requestorNodeId, string providerNodeId);

        void SetActivityState(string activityId, ActivityState state);

        Activity GetActivity(string activityId);

        /// <summary>
        /// Persists the exeScript batch under unique batch id
        /// </summary>
        /// <param name="exeScript"></param>
        /// <returns></returns>
        ExeScript CreateExeScriptBatch(string activityId, ExeScript exeScript);

        /// <summary>
        /// Fetch the persisted exescript batch by batchId.
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        ExeScript GetExeScriptBatch(string batchId);
    }
}
