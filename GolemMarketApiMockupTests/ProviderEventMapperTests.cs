using AutoMapper;
using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Mappers;
using GolemMarketApiMockup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GolemMarketApiMockupTests
{
    [TestClass]
    public class ProviderEventMapperTests
    {

        public IMapper Mapper { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            this.Mapper = config.CreateMapper();
        }

        [TestMethod]
        public void Map_ProviderEvent_ToDemandEvent()
        {

            var mapper = new ProviderEventMapper(this.Mapper);

            var entity = new MarketProviderEvent()
            {
                EventType = MarketProviderEvent.MarketProviderEventType.Proposal,
                DemandProposal = new DemandProposal()
                {
                    OfferId = "PreviousOfferId",
                    InternalId = 2,
                    Demand = new Demand()
                    {
                        NodeId = "RequestorNodeId",
                        Constraints = "()",
                        Properties = new Dictionary<string, string>() { }
                    }
                }
            };

            var result = mapper.Map(entity) as GolemMarketMockAPI.MarketAPI.Models.DemandEvent;

            Assert.AreEqual(entity.DemandProposal.Demand.Constraints, result.Demand.Constraints);
            Assert.AreEqual(entity.DemandProposal.OfferId, result.Demand.PrevProposalId);
            Assert.AreEqual(entity.DemandProposal.Id, result.Demand.Id);
            Assert.AreEqual(entity.DemandProposal.Demand.NodeId, result.RequestorId);

        }



    }
}
