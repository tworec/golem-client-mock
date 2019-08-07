using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class ActivityProviderEvent
    {
        public enum ActivityProviderEventType
        {
            CreateActivity,
            Exec,
            DestroyActivity,
            GetState
        }

        public ActivityProviderEventType EventType { get; set; }

        public string ActivityId { get; set; }

        /// <summary>
        /// ExeScript batch - relevant for Exec events.
        /// </summary>
        public ExeScript ExeScript { get; set; }
    }
}
