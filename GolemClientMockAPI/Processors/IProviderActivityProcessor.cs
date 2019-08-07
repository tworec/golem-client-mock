using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors
{
    public interface IProviderActivityProcessor
    {
        Task<ICollection<ActivityProviderEvent>> CollectActivityEventsAsync(string providerNodeId, int timeout); // TODO is the name correct???
        void SendActivityExecResult(string activityId, string exeScriptBatchId, ActivityExecResult execResult); // TODO should we create a generic "callback" method to return call results???
        void SendActivityStateDetails(string activityId, ActivityStateDetails stateDetails); 
    }
}
