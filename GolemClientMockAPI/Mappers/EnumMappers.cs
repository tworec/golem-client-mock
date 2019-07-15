using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GolemClientMockAPI.Entities.ProviderEvent;
using static GolemClientMockAPI.Entities.RequestorEvent;

namespace GolemClientMockAPI.Mappers
{
    public static class EnumMappers
    {
        public static string TranslateRequestorEventType(RequestorEventType eventType)
        {
            switch(eventType)
            {
                case RequestorEventType.Proposal:
                    return "offer";
                case RequestorEventType.PropertyQuery:
                    return "propertyQuery";
                default:
                    throw new Exception($"Unknown RequestorEventType {eventType}");
            }
        }
        public static string TranslateProviderEventType(ProviderEventType eventType)
        {
            switch (eventType)
            {
                case ProviderEventType.Proposal:
                    return "demand";
                case ProviderEventType.PropertyQuery:
                    return "propertyQuery";
                case ProviderEventType.AgreementProposal:
                    return "newAgreement";
                default:
                    throw new Exception($"Unknown ProviderEventType {eventType}");
            }
        }

    }
}
