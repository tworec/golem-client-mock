using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public interface IAgreementRepository
    {
        Agreement CreateAgreement(Demand demand, OfferProposal offer);
        void UpdateAgreementState(string agreementId, AgreementState state);
        Agreement GetAgreement(string agreementId);

    }
}
