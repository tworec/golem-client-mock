using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class ActivityProviderEvent
    {
        // NOTE These enum values must match with Provider EventTypeEnum values
        public enum ActivityProviderEventType
        {
            CreateActivity = 1,
            Exec = 2,
            DestroyActivity = 3,
            GetState = 4
        }

        public ActivityProviderEventType EventType { get; set; }

        public string ActivityId { get; set; }

        public string AgreementId { get; set; }

        /// <summary>
        /// ExeScript batch - relevant for Exec events.
        /// </summary>
        public ExeScript ExeScript { get; set; }
    }
}
