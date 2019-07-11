using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Entities
{
    public enum AgreementState
    {
        Proposed,
        Approved,
        Rejected
    }

    public class Agreement
    {
        public string Id { get; set; }

        public AgreementState State { get; set; }

        public Demand Demand { get; set; }

        public Offer Offer { get; set; }
    }
}
