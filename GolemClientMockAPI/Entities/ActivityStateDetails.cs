using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class ActivityStateDetails
    {
        public ActivityState State { get; set; }
        public decimal[] UsageVector { get; set; }
        public string CurrentCommand { get; set; }
    }
}
