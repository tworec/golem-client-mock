using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors
{
    public interface IRequestorMarketProcessor
    {
        DemandSubscription SubscribeDemand(Demand demand);

        Task<ICollection<RequestorEvent>> CollectRequestorEventsAsync(string subscriptionId, float? timeout, int? maxEvents);

        DemandProposal CreateDemandProposal(string subscriptionId, string offerProposalId, Demand demand);

        Agreement CreateAgreement(string offerProposalId);

        Task<AgreementResultEnum> ConfirmAgreementAsync(string agreementId, float? timeout);
        void SendConfirmAgreement(string agreementId);

        Task<AgreementResultEnum> WaitConfirmAgreementResponseAsync(string agreementId, float? timeout);

        void CancelAgreement(string agreementId);

        void UnsubscribeDemand(string subscriptionId);
    }
}
