using AutoMapper;
using GolemMarketMockAPI.MarketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class OfferMapper
    {
        public IMapper Mapper { get; set; }

        public OfferMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public Entities.Offer MapToEntity(Offer offer)
        {
            return this.Mapper.Map<Entities.Offer>(offer);
        }
        public Proposal MapEntityToProposal(Entities.OfferProposal offerProposal)
        {
            return this.Mapper.Map<Proposal>(offerProposal);
        }



    }
}
