using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Models
{
    public class MarketStats
    {
        public IEnumerable<NodeStats> Requestors { get; set; }
        public IEnumerable<NodeStats> Providers { get; set; }
    }
}
