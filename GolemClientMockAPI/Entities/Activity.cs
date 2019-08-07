using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public enum ActivityState
    {
        New,
        Deploying,
        Ready,
        Starting,
        Active,
        Unresponsive,
        Suspending,
        Resuming,
        Terminated
    }

    public class Activity
    {
        public string Id { get; set; }
        public string AgreementId { get; set; }
        public ActivityState State { get; set; }

        public string RequestorNodeId { get; set; }
        public string ProviderNodeId { get; set; }

    }
}
