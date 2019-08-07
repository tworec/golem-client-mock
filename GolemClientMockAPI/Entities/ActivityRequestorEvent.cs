using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class ActivityRequestorEvent
    {
        public enum ActivityRequestorEventType
        {
            ExecResult,
            StateDetails
        }

        public ActivityRequestorEventType EventType { get; set; }

        public string ActivityId { get; set; }

        public ActivityExecResult ExecResult { get; set; }

        public ActivityStateDetails Details { get; set; }
    }
}
