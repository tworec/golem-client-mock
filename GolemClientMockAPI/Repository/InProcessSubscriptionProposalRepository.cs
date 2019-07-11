using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;

namespace GolemClientMockAPI.Repository
{
    public class InProcessSubscriptionProposalRepository : ISubscriptionRepository, IProposalRepository
    {
        public object SubscriptionsLock { get; set; } = new object();

        public IDictionary<string, Subscription> Subscriptions { get; set; } = new Dictionary<string, Subscription>();
        public IDictionary<string, IList<Proposal>> SubscriptionProposals { get; set; } = new Dictionary<string, IList<Proposal>>();

        public object SubscriptionSeqLock { get; set; } = new object();

        public int SubscriptionSeq { get; set; } = 0;

        public object ProposalSeqLock { get; set; } = new object();

        public int ProposalSeq { get; set; } = 0;


        private int GetNextSubscriptionInternalId()
        {
            lock(SubscriptionSeqLock)
            {
                this.SubscriptionSeq++;
                return this.SubscriptionSeq;
            }
        }

        private int GetNextProposalInternalId()
        {
            lock (ProposalSeqLock)
            {
                this.ProposalSeq++;
                return this.ProposalSeq;
            }
        }

        #region ISubscriptionRepository implementations

        public DemandSubscription CreateDemandSubscription(Demand demand)
        {
            var newInternalId = GetNextSubscriptionInternalId();
            var newId = "" + Guid.NewGuid();

            var result = new DemandSubscription()
            {
                Demand = demand,
                Id = newId,
                LastReceivedProposalId = null
            };

            demand.Id = newId;

            lock(SubscriptionsLock)
            {
                this.Subscriptions.Add(newId, result);
                this.SubscriptionProposals.Add(newId, new List<Proposal>());
            }

            return result;
        }

        public OfferSubscription CreateOfferSubscription(Offer offer)
        {
            var newInternalId = GetNextSubscriptionInternalId();
            var newId = "" + Guid.NewGuid();

            var result = new OfferSubscription()
            {
                Offer = offer,
                Id = newId,
                LastReceivedProposalId = null
            };

            offer.Id = newId;

            lock (SubscriptionsLock)
            {
                this.Subscriptions.Add(newId, result);
                this.SubscriptionProposals.Add(newId, new List<Proposal>());
            }

            return result;
        }

        public DemandSubscription GetDemandSubscription(string subscriptionId)
        {
            if(this.Subscriptions.ContainsKey(subscriptionId))
            {
                return this.Subscriptions[subscriptionId] as DemandSubscription;
            }
            else
            {
                return null;
            }
            
        }

        public OfferSubscription GetOfferSubscription(string subscriptionId)
        {
            if (this.Subscriptions.ContainsKey(subscriptionId))
            {
                return this.Subscriptions[subscriptionId] as OfferSubscription;
            }
            else
            {
                return null;
            }
        }

        public void UpdateLastProposalId(string subscriptionId, int? internalId)
        {
            if (this.Subscriptions.ContainsKey(subscriptionId))
            {
                var subscr = this.Subscriptions[subscriptionId];
                subscr.LastReceivedProposalId = internalId;
            }
            else
            {
                throw new Exception($"Subscription Id {subscriptionId} not found!");
            }
        }

        #endregion

        #region IProposalRepository implementations

        public DemandProposal SaveDemandProposal(string subscriptionId, Demand demand, string offerId = null)
        {
            if (this.SubscriptionProposals.ContainsKey(subscriptionId) && this.Subscriptions[subscriptionId] is OfferSubscription)
            {
                // If this is the first appearance of this Demand - assign Id
                if (demand.Id == null)
                {
                    demand.Id = "" + Guid.NewGuid();
                }

                var demandProposal = new DemandProposal()
                {
                    InternalId = this.GetNextProposalInternalId(),
                    OfferId = offerId,
                    Demand = demand
                };

                this.SubscriptionProposals[subscriptionId].Add(demandProposal);

                return demandProposal;
            }
            else
            {
                throw new Exception($"Offer Subscription Id {subscriptionId} does not exist!");
            }
        }

        public OfferProposal SaveOfferProposal(string subscriptionId, Offer offer, string demandId = null)
        {
            if(this.SubscriptionProposals.ContainsKey(subscriptionId) && this.Subscriptions[subscriptionId] is DemandSubscription)
            {
                // If this is the first appearance of this Offer - assign Id
                if (offer.Id == null)
                {
                    offer.Id = "" + Guid.NewGuid();
                }

                var offerProposal = new OfferProposal()
                {
                    InternalId = this.GetNextProposalInternalId(),
                    DemandId = demandId,
                    Offer = offer
                };

                this.SubscriptionProposals[subscriptionId].Add(offerProposal);

                return offerProposal;
            }
            else
            {
                throw new Exception($"Demand Subscription Id {subscriptionId} does not exist!");
            }
        }

        public ICollection<OfferProposal> GetOfferProposals(string subscriptionId, long? lastReceivedProposalId = null)
        {
            if (this.SubscriptionProposals.ContainsKey(subscriptionId) && this.Subscriptions[subscriptionId] is DemandSubscription)
            {
                
                return this.SubscriptionProposals[subscriptionId].Where(prop => lastReceivedProposalId == null || prop.InternalId > lastReceivedProposalId)
                    .Select(prop => prop as OfferProposal).ToList();
            }
            else
            {
                throw new Exception($"Demand Subscription Id {subscriptionId} does not exist!");
            }
        }

        public ICollection<DemandProposal> GetDemandProposals(string subscriptionId, long? lastReceivedProposalId = null)
        {
            if (this.SubscriptionProposals.ContainsKey(subscriptionId) && this.Subscriptions[subscriptionId] is OfferSubscription)
            {

                return this.SubscriptionProposals[subscriptionId].Where(prop => lastReceivedProposalId == null || prop.InternalId > lastReceivedProposalId)
                    .Select(prop => prop as DemandProposal).ToList();
            }
            else
            {
                throw new Exception($"Offer Subscription Id {subscriptionId} does not exist!");
            }
        }

        #endregion


    }
}
