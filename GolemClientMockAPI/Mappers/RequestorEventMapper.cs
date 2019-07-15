using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class RequestorEventMapper
    {
        public IMapper Mapper { get; set; }

        public RequestorEventMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public GolemMarketMockAPI.MarketAPI.Models.RequestorEvent Map(Entities.RequestorEvent requestorEventEntity)
        {
            switch(requestorEventEntity.EventType)
            {
                case Entities.RequestorEvent.RequestorEventType.Proposal:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.OfferEvent>(requestorEventEntity);
                case Entities.RequestorEvent.RequestorEventType.PropertyQuery:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.RequestorEvent>(requestorEventEntity);
                default:
                    throw new Exception($"Unknown RequestorEventType {requestorEventEntity.EventType}");
            }

            
        }
    }
}
