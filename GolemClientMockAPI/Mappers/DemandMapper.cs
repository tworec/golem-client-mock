using AutoMapper;
using GolemMarketMockAPI.MarketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class DemandMapper
    {
        public IMapper Mapper { get; set; }

        public DemandMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public Entities.Demand MapToEntity(Demand demand)
        {
            return this.Mapper.Map<Entities.Demand>(demand);
        }
    }
}
