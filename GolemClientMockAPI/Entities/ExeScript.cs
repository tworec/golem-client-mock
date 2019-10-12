using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class ExeScript
    {
        public string BatchId { get; set; }
        
        /// <summary>
        /// The body of ExeScript
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Number of commands in batch script
        /// </summary>
        public int CommandCount { get; set; }
    }
}
