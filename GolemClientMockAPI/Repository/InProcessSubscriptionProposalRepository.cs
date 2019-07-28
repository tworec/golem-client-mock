using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Mappers;
using GolemClientMockAPI.Models;

namespace GolemClientMockAPI.Repository
{
    public class InProcessSubscriptionProposalRepository : ISubscriptionRepository, IProposalRepository, IStatsRepository
    {
        public object SubscriptionsLock { get; set; } = new object();

        /// <summary>
        /// Collection of subscriptions, indexed by SubscriptionId
        /// </summary>
        public IDictionary<string, Subscription> Subscriptions { get; set; } = new Dictionary<string, Subscription>();
        /// <summary>
        /// Collection of lists of proposals RECEIVED by subscriptions, indexed by SubscriptionId
        /// </summary>
        public IDictionary<string, IList<Proposal>> SubscriptionProposals { get; set; } = new Dictionary<string, IList<Proposal>>();

        /// <summary>
        /// Collection of Demand proposals, indexed by Demand Proposal Id
        /// </summary>
        public IDictionary<string, DemandProposal> DemandProposals { get; set; } = new Dictionary<string, DemandProposal>();

        /// <summary>
        /// Collection of Offer proposals, indexed by Offer Proposal Id
        /// </summary>
        public IDictionary<string, OfferProposal> OfferProposals { get; set; } = new Dictionary<string, OfferProposal>();


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
            var now = DateTime.Now;

            var result = new DemandSubscription()
            {
                Demand = demand,
                Id = newId,
                CreatedDate = now,
                LastActiveDate = now,
                LastReceivedProposalId = null
            };

            //demand.Id = newId;

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
            var now = DateTime.Now;

            var result = new OfferSubscription()
            {
                Offer = offer,
                Id = newId,
                CreatedDate = now,
                LastActiveDate = now,
                LastReceivedProposalId = null
            };

            //offer.Id = newId;

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

        public void UpdateLastActive(string subscriptionId, DateTime timestamp)
        {
            if (this.Subscriptions.ContainsKey(subscriptionId))
            {
                var subscr = this.Subscriptions[subscriptionId];
                subscr.LastActiveDate = timestamp;
            }
            else
            {
                throw new Exception($"Subscription Id {subscriptionId} not found!");
            }
        }


        protected void DeleteSubscription(string subscriptionId)
        {
            if (this.Subscriptions.ContainsKey(subscriptionId))
            {
                this.Subscriptions.Remove(subscriptionId);
                this.SubscriptionProposals.Remove(subscriptionId);
            }
        }

        public void DeleteDemandSubscription(string subscriptionId)
        {
            this.DeleteSubscription(subscriptionId);
        }

        public void DeleteOfferSubscription(string subscriptionId)
        {
            this.DeleteSubscription(subscriptionId);
        }

        #endregion

        #region IProposalRepository implementations

        public DemandProposal SaveDemandProposal(string receivingSubscriptionId, string sendingSubscriptionId, Demand demand, string offerProposalId = null)
        {
            if (this.SubscriptionProposals.ContainsKey(receivingSubscriptionId) && this.Subscriptions[receivingSubscriptionId] is OfferSubscription)
            {
                var demandProposal = new DemandProposal()
                {
                    Id = "" + Guid.NewGuid(),
                    InternalId = this.GetNextProposalInternalId(),
                    ReceivingSubscriptionId = receivingSubscriptionId,
                    SendingSubscriptionId = sendingSubscriptionId,
                    OfferId = offerProposalId,
                    Demand = demand
                };

                // TODO do we need a lock here???
                this.SubscriptionProposals[receivingSubscriptionId].Add(demandProposal);
                this.DemandProposals[demandProposal.Id] = demandProposal;

                return demandProposal;
            }
            else
            {
                throw new Exception($"Offer Subscription Id {receivingSubscriptionId} does not exist!");
            }
        }

        public OfferProposal SaveOfferProposal(string receivingSubscriptionId, string sendingSubscriptionId, Offer offer, string demandId = null)
        {
            if(this.SubscriptionProposals.ContainsKey(receivingSubscriptionId) && this.Subscriptions[receivingSubscriptionId] is DemandSubscription)
            {
                var offerProposal = new OfferProposal()
                {
                    Id = "" + Guid.NewGuid(),
                    InternalId = this.GetNextProposalInternalId(),
                    ReceivingSubscriptionId = receivingSubscriptionId,
                    SendingSubscriptionId = sendingSubscriptionId,
                    DemandId = demandId,
                    Offer = offer
                };

                this.SubscriptionProposals[receivingSubscriptionId].Add(offerProposal);
                this.OfferProposals[offerProposal.Id] = offerProposal;

                return offerProposal;
            }
            else
            {
                throw new Exception($"Demand Subscription Id {receivingSubscriptionId} does not exist!");
            }
        }

        public ICollection<OfferProposal> GetOfferProposals(string receivingSubscriptionId, long? lastReceivedProposalId = null)
        {
            if (this.SubscriptionProposals.ContainsKey(receivingSubscriptionId) && this.Subscriptions[receivingSubscriptionId] is DemandSubscription)
            {
                
                return this.SubscriptionProposals[receivingSubscriptionId].Where(prop => lastReceivedProposalId == null || prop.InternalId > lastReceivedProposalId)
                    .Select(prop => prop as OfferProposal).ToList();
            }
            else
            {
                throw new Exception($"Demand Subscription Id {receivingSubscriptionId} does not exist!");
            }
        }

        public ICollection<DemandProposal> GetDemandProposals(string receivingSubscriptionId, long? lastReceivedProposalId = null)
        {
            if (this.SubscriptionProposals.ContainsKey(receivingSubscriptionId) && this.Subscriptions[receivingSubscriptionId] is OfferSubscription)
            {

                return this.SubscriptionProposals[receivingSubscriptionId].Where(prop => lastReceivedProposalId == null || prop.InternalId > lastReceivedProposalId)
                    .Select(prop => prop as DemandProposal).ToList();
            }
            else
            {
                throw new Exception($"Offer Subscription Id {receivingSubscriptionId} does not exist!");
            }
        }

        /// <summary>
        /// OK, so here we're searching for offer proposal by offer Id - we need to look in Demand subscriptions!!!
        /// </summary>
        /// <param name="offerProposalId"></param>
        /// <returns></returns>
        public OfferProposal GetOfferProposal(string offerProposalId)
        {
            if (!this.OfferProposals.ContainsKey(offerProposalId))
            {
                throw new Exception($"Unknown Demand Proposal Id {offerProposalId}");
            }

            return this.OfferProposals[offerProposalId];
        }

        public DemandProposal GetDemandProposal(string demandProposalId)
        {
            if(!this.DemandProposals.ContainsKey(demandProposalId))
            {
                throw new Exception($"Unknown Demand Proposal Id {demandProposalId}");
            }

            return this.DemandProposals[demandProposalId];
        }

        #endregion

        #region IStatsRepository

        public MarketStats GetMarketStats()
        {
            var result = new MarketStats()
            {
                Requestors = this.Subscriptions.Values
                                 .Where(subs => subs is DemandSubscription)
                                 .Select(sub => sub as DemandSubscription)
                                 .GroupBy(sub => sub.Demand.NodeId)
                                 .Select(node =>
                                    new NodeStats()
                                    {
                                        NodeId = node.Key,
                                        SubscriptionCount = node.Count(),
                                        Connected = node.Min(sub => sub.CreatedDate),
                                        LastActive = node.Max(sub => sub.LastActiveDate)
                                    }
                                 ).ToList(),
                Providers = this.Subscriptions.Values
                                 .Where(subs => subs is OfferSubscription)
                                 .Select(sub => sub as OfferSubscription)
                                 .GroupBy(sub => sub.Offer.NodeId)
                                 .Select(node =>
                                    new NodeStats()
                                    {
                                        NodeId = node.Key,
                                        SubscriptionCount = node.Count(),
                                        Connected = node.Min(sub => sub.CreatedDate),
                                        LastActive = node.Max(sub => sub.LastActiveDate)
                                    }
                                 ).ToList(),

            };

            return result;
        }

        public NodeStats GetNodeDetails(string nodeId)
        {
            var result = this.Subscriptions.Values
                            .Where(sub =>
                            {
                                if (sub is OfferSubscription)
                                {
                                    return (sub as OfferSubscription).Offer.NodeId == nodeId;
                                }
                                else
                                {
                                    return (sub as DemandSubscription).Demand.NodeId == nodeId;
                                }
                            })
                            .GroupBy(sub =>
                            {
                                if (sub is OfferSubscription)
                                {
                                    return (sub as OfferSubscription).Offer.NodeId;
                                }
                                else
                                {
                                    return (sub as DemandSubscription).Demand.NodeId;
                                }

                            })
                            .Select(subGrp =>
                                new NodeStats()
                                {
                                    NodeId = nodeId,
                                    SubscriptionCount = subGrp.Count(),
                                    Subscriptions = subGrp.Select(sub =>
                                    {
                                        if (sub is OfferSubscription)
                                        {
                                            var off = sub as OfferSubscription;

                                            return new SubscriptionStats()
                                            {
                                                Constraints = off.Offer.Constraints,
                                                Properties = PropertyMappers.MapToJsonString(off.Offer.Properties),
                                                SubscriptionId = off.Id
                                            };
                                        }
                                        else
                                        {
                                            var dem = sub as DemandSubscription;

                                            return new SubscriptionStats()
                                            {
                                                Constraints = dem.Demand.Constraints,
                                                Properties = PropertyMappers.MapToJsonString(dem.Demand.Properties),
                                                SubscriptionId = dem.Id
                                            };
                                        }

                                    }),
                                    Connected = subGrp.Min(sub => sub.CreatedDate),
                                    LastActive = subGrp.Max(sub => sub.LastActiveDate)
                                }

                            )
                            .FirstOrDefault();
            return result;
                            
        }

        public SubscriptionStats GetSubscriptionDetails(string nodeId, string subscriptionId)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
