using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;

namespace GolemClientMockAPI.Repository
{
    public class InProcessAgreementRepository : IAgreementRepository
    {
        public Agreement CreateAgreement(Demand demand, Offer offer)
        {
            throw new NotImplementedException();
        }

        public void UpdateAgreementState(string agreementId, AgreementState state)
        {
            throw new NotImplementedException();
        }
    }
}
