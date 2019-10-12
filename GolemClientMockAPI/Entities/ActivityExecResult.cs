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
            OK = 1,
            Error = 2
        }

        /// <summary>
        /// Index of command for which this is the result
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Indicates this was the last command in the batch
        /// </summary>
        public bool IsBatchFinished { get; set; }

        public ActivityExecResultEnum Result { get; set; }

        /// <summary>
        /// Current state of the Activity after the command
        /// </summary>
        public ActivityState CurrentState { get; set; }
    }
}
