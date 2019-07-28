using GolemClientMockAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Repository
{
    public interface IStatsRepository
    {
        MarketStats GetMarketStats();

        NodeStats GetNodeDetails(string nodeId);

        SubscriptionStats GetSubscriptionDetails(string nodeId, string subscriptionId);
    }
}
