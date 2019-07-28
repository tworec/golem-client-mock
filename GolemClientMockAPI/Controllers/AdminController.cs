using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Models;
using GolemClientMockAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GolemClientMockAPI.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private IList<NodeStats> sampleNodeStats = new List<NodeStats>()
        {
            new NodeStats()
                {
                    NodeId = "DummyRequestor_1",
                    Connected = DateTime.Now.AddMinutes(-73),
                    LastActive = DateTime.Now.AddMinutes(-1),
                    SubscriptionCount = 3,
                    Subscriptions = new List<SubscriptionStats>()
                    {
                        new SubscriptionStats() { SubscriptionId = "Subscription_1", Constraints = "()", Properties = "golem.prop=12"}
                    }
                },
                new NodeStats()
                {
                    NodeId = "DummyRequestor_2",
                    Connected = DateTime.Now.AddMinutes(-18),
                    LastActive = DateTime.Now.AddMinutes(-1.6),
                    SubscriptionCount = 1
                },
                new NodeStats()
                {
                    NodeId = "DummyProvider_1",
                    Connected = DateTime.Now.AddMinutes(-73),
                    LastActive = DateTime.Now.AddMinutes(-1),
                    SubscriptionCount = 3
                },
                new NodeStats()
                {
                    NodeId = "DummyProvider_2",
                    Connected = DateTime.Now.AddMinutes(-138),
                    LastActive = DateTime.Now.AddMinutes(-35),
                    SubscriptionCount = 1
                },
                new NodeStats()
                {
                    NodeId = "DummyProvider_3",
                    Connected = DateTime.Now.AddMinutes(-13),
                    LastActive = DateTime.Now.AddMinutes(-0.5),
                    SubscriptionCount = 1
                }
        };

        private MarketStats sampleStats = new MarketStats()
        {
            Requestors = new List<NodeStats>()
            {
                new NodeStats()
                {
                    NodeId = "DummyRequestor_1",
                    Connected = DateTime.Now.AddMinutes(-73),
                    LastActive = DateTime.Now.AddMinutes(-1),
                    SubscriptionCount = 3,
                    Subscriptions = new List<SubscriptionStats>()
                    {
                        new SubscriptionStats() { SubscriptionId = "Subscription_1", Constraints = "()", Properties = "golem.prop=12"}
                    }
                },
                new NodeStats()
                {
                    NodeId = "DummyRequestor_2",
                    Connected = DateTime.Now.AddMinutes(-18),
                    LastActive = DateTime.Now.AddMinutes(-1.6),
                    SubscriptionCount = 1
                }
            },
            Providers = new List<NodeStats>()
            {
                new NodeStats()
                {
                    NodeId = "DummyProvider_1",
                    Connected = DateTime.Now.AddMinutes(-73),
                    LastActive = DateTime.Now.AddMinutes(-1),
                    SubscriptionCount = 3
                },
                new NodeStats()
                {
                    NodeId = "DummyProvider_2",
                    Connected = DateTime.Now.AddMinutes(-138),
                    LastActive = DateTime.Now.AddMinutes(-35),
                    SubscriptionCount = 1
                },
                new NodeStats()
                {
                    NodeId = "DummyProvider_3",
                    Connected = DateTime.Now.AddMinutes(-13),
                    LastActive = DateTime.Now.AddMinutes(-0.5),
                    SubscriptionCount = 1
                }
            }
        };

        public IStatsRepository StatsRepository { get; set; }

        public AdminController(IStatsRepository statsRepo)
        {
            this.StatsRepository = statsRepo;
        }


        [HttpGet("marketStats")]
        public MarketStats GetMarketStats()
        {
            return this.StatsRepository.GetMarketStats();
        }

        [HttpGet("marketStats/{nodeId}")]
        public NodeStats GetNodeDetails(string nodeId)
        {
            return this.StatsRepository.GetNodeDetails(nodeId);

            //var result = this.sampleNodeStats.FirstOrDefault(node => node.NodeId == nodeId);

            //result.Subscriptions = new List<SubscriptionStats>()
            //{
            //    new SubscriptionStats()
            //    {
            //        SubscriptionId = "Subscription_1",
            //        ProposalCount = 2,
            //        Properties = "golem.property=\"someValue\"",
            //        Constraints = "()"
            //    }
            //};

            //return result;
        }

        [HttpGet("marketStats/{nodeId}/{subscriptionId}")]
        public SubscriptionStats GetSubscriptionDetails(string nodeId, string subscriptionId)
        {
            return this.StatsRepository.GetSubscriptionDetails(nodeId, subscriptionId);
        }

    }


}