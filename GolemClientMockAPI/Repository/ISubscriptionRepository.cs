using GolemClientMockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// Persist the Demand subscription.
        /// </summary>
        /// <param name="demand"></param>
        /// <returns></returns>
        DemandSubscription CreateDemandSubscription(Demand demand);

        /// <summary>
        /// Fetch the Subscription based on subscriptionId.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns>null if Demand Subscription not found.</returns>
        DemandSubscription GetDemandSubscription(string subscriptionId);

        /// <summary>
        /// Remove the Demand subscription.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        void DeleteDemandSubscription(string subscriptionId);

        /// <summary>
        /// Persist the most recently returned Proposal Id against Subscription, 
        /// so that the engine knows which Proposal should it return on next call. 
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="internalId"></param>
        void UpdateLastProposalId(string subscriptionId, int? internalId);

        /// <summary>
        /// Persist the LastActiveDate for a subscription. 
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="timestamp"></param>
        void UpdateLastActive(string subscriptionId, DateTime timestamp);

        /// <summary>
        /// Persist the Offer subscription.
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        OfferSubscription CreateOfferSubscription(Offer offer);
        
        /// <summary>
        /// Fetch the Subscription based on subscriptionId.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns>null if Offer Subscription not found.</returns>
        OfferSubscription GetOfferSubscription(string subscriptionId);

        /// <summary>
        /// Remove the Offer subscription.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        void DeleteOfferSubscription(string subscriptionId);

    }
}
