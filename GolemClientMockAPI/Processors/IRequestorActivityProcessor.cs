using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors
{
    public interface IRequestorActivityProcessor
    {
        #region Control

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns>Created Activity</returns>
        Activity CreateActivity(string agreementId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="exeScript"></param>
        /// <returns>exeScript batch id</returns>
        string ExecAsync(string activityId, ExeScript exeScript);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exeScriptBatchId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ICollection<ActivityRequestorEvent>> GetExecBatchResultsAsync(string exeScriptBatchId, int timeout); 


        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        void DestroyActivity(string activityId);

        #endregion

        #region State

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        ActivityStateDetails GetStateDetails(string activityId, int timeout);

        #endregion
    }
}
