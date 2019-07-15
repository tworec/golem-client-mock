using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class ProviderEventMapper
    {
        public IMapper Mapper { get; set; }

        public ProviderEventMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public GolemMarketMockAPI.MarketAPI.Models.ProviderEvent Map(Entities.ProviderEvent providerEventEntity)
        {
            switch (providerEventEntity.EventType)
            {
                case Entities.ProviderEvent.ProviderEventType.Proposal:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.DemandEvent>(providerEventEntity);
                case Entities.ProviderEvent.ProviderEventType.PropertyQuery:
                    return this.Mapper.Map<GolemMarketMockAPI.MarketAPI.Models.ProviderEvent>(providerEventEntity);
                default:
                    throw new Exception($"Unknown ProviderEventType {providerEventEntity.EventType} ");
            }
        }
    }
}
