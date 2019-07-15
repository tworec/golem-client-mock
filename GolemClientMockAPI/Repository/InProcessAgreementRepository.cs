using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;

namespace GolemClientMockAPI.Repository
{
    public class InProcessAgreementRepository : IAgreementRepository
    {
        public IDictionary<string, Agreement> Agreements { get; set; } = new Dictionary<string, Agreement>();

        public Agreement CreateAgreement(Demand demand, OfferProposal offerProposal)
        {
            if(this.Agreements.ContainsKey(offerProposal.Id))
            {
                throw new Exception($"Agreement Id {offerProposal.Id} already exists!");
            }

            var agreement = new Agreement()
            {
                Id = offerProposal.Id,
                Offer = offerProposal.Offer,
                Demand = demand,
                State = AgreementState.New
            };

            this.Agreements[agreement.Id] = agreement;

            return agreement;
        }

        public Agreement GetAgreement(string agreementId)
        {
            if (this.Agreements.ContainsKey(agreementId))
            {
                return this.Agreements[agreementId];
            }
            else
            {
                throw new Exception($"Agreement Id {agreementId} does not exist!");
            }
        }

        public void UpdateAgreementState(string agreementId, AgreementState state)
        {
            if (this.Agreements.ContainsKey(agreementId))
            {
                this.Agreements[agreementId].State = state;
            }
            else
            {
                throw new Exception($"Agreement Id {agreementId} does not exist!");
            }
        }
    }
}
