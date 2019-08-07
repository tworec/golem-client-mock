using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class ActivityExecResult
    {
        public enum ActivityExecResultEnum
        {
            OK,
            Error
        }

        public ActivityExecResultEnum Result { get; set; }

        /// <summary>
        /// Current state of the Activity after the 
        /// </summary>
        public ActivityState CurrentState { get; set; }
    }
}
