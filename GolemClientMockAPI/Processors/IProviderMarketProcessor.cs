using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Processors
{
    public interface IProviderMarketProcessor
    {
        OfferSubscription SubscribeOffer(Offer offer);

        Task<ICollection<MarketProviderEvent>> CollectProviderEventsAsync(string subscriptionId, float? timeout, int? maxEvents);

        OfferProposal CreateOfferProposal(string subscriptionId, string demandProposalId, Offer offer);

        void RejectAgreement(string agreementId);

        Agreement ApproveAgreement(string agreementId);

        void UnsubscribeOffer(string subscriptionId);


    }
}
