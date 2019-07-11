using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ...from Entities

            CreateMap<Entities.RequestorEvent, GolemMarketMockAPI.MarketAPI.Models.RequestorEvent>()
                    .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => EnumMappers.TranslateRequestorEventType(src.EventType)));

            CreateMap<Entities.Demand, GolemMarketMockAPI.MarketAPI.Models.Demand>();

            // ...to Entities

            CreateMap<GolemMarketMockAPI.MarketAPI.Models.Demand, Entities.Demand>();

        }
    }
}
