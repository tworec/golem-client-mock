using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public class Demand
    {
        public string NodeId { get; set; }
        public string Id { get; set; }
        public IDictionary<string, string> Properties { get; set; }
        public string Constraints { get; set; }
    }
}
