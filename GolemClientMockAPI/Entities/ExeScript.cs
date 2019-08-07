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
    }
}
