using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GolemClientMockAPI.Entities.MarketProviderEvent;
using static GolemClientMockAPI.Entities.MarketRequestorEvent;

namespace GolemClientMockAPI.Mappers
{
    public static class EnumMappers
    {
        public static string TranslateRequestorEventType(MarketRequestorEventType eventType)
        {
            switch(eventType)
            {
                case MarketRequestorEventType.Proposal:
                    return "offer";
                case MarketRequestorEventType.PropertyQuery:
                    return "propertyQuery";
                default:
                    throw new Exception($"Unknown RequestorEventType {eventType}");
            }
        }
        public static string TranslateProviderEventType(MarketProviderEventType eventType)
        {
            switch (eventType)
            {
                case MarketProviderEventType.Proposal:
                    return "demand";
                case MarketProviderEventType.PropertyQuery:
                    return "propertyQuery";
                case MarketProviderEventType.AgreementProposal:
                    return "newAgreement";
                default:
                    throw new Exception($"Unknown ProviderEventType {eventType}");
            }
        }

    }
}
