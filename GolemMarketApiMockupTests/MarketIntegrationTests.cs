using GolemClientMockAPI.Entities;
using GolemClientMockAPI.Processors;
using GolemClientMockAPI.Repository;
using GolemMarketApiMockup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GolemMarketApiMockupTests
{
    [TestClass]
    public class MarketIntegrationTests
    {
        [TestMethod]
        public async Task InMemoryMarketResolver_Integration_SimplePositiveScenario()
        {
            var subscriptionRepository = new InProcessSubscriptionProposalRepository();
            var proposalRepository = subscriptionRepository;
            var agreementRepository = new InProcessAgreementRepository();

            var marketProcessor = new InMemoryMarketProcessor(subscriptionRepository, proposalRepository, agreementRepository);

            IRequestorMarketProcessor requestorProcessor = marketProcessor;
            IProviderMarketProcessor providerProcessor = marketProcessor;

            // Test scenario

            // Subscribe Demand

            var demand = new Demand()
            {
                Constraints = @"(&" +
                    @"(golem.srv.comp.container.docker.image=golemfactory/ffmpeg)" +
                    @"(golem.srv.comp.container.docker.benchmark{golemfactory/ffmpeg}>300)" +
                    @"(golem.com.payment.scheme=after)" +
                    @"(golem.usage.vector=[golem.usage.duration_sec])" +
                    @"(golem.com.pricing.model=linear)" +
                    //@"(golem.com.pricing.est{[30]}<125)" +
                @")",
                Id = null,
                NodeId = "RequestorA", // must be assigned from the outside
                Properties = new Dictionary<string, string>()
            };

            var demandSubscription = requestorProcessor.SubscribeDemand(demand);


            Assert.IsNotNull(demandSubscription.Id);
            Assert.IsNotNull(demandSubscription.Demand);
            Assert.IsNull(demandSubscription.LastReceivedProposalId);

            // Subscribe Offers

            var offer1 = new Offer()
            {
                Constraints = "()",
                Id = null,
                NodeId = "ProviderA",
                Properties = new Dictionary<string, string>()
                {
                    { "golem.srv.comp.container.docker.image", @"[""golemfactory/ffmpeg""]" },
                    { "golem.srv.comp.container.docker.benchmark{golemfactory/ffmpeg}", @"7"},
                    { "golem.srv.comp.container.docker.benchmark{*}", null},
                    { "golem.inf.cpu.cores", @"4"},
                    { "golem.inf.cpu.threads", @"8"},
                    { "golem.inf.mem.gib", @"16"},
                    { "golem.inf.storage.gib", @"30"},
                    { "golem.usage.vector", @"[""golem.usage.duration_sec""]"},
                    { "golem.com.payment.scheme", @"""after"""},
                    { "golem.com.pricing.model", @"""linear"""},
                    { "golem.com.pricing.est{*}", null},
                }
            };

            var offer2 = new Offer()
            {
                Constraints = "()",
                Id = null,
                NodeId = "ProviderB",
                Properties = new Dictionary<string, string>()
                {
                    { "golem.srv.comp.container.docker.image", @"[""golemfactory/ffmpeg""]" },
                    { "golem.srv.comp.container.docker.benchmark{golemfactory/ffmpeg}", @"723"},
                    { "golem.srv.comp.container.docker.benchmark{*}", null},
                    { "golem.inf.cpu.cores", @"4"},
                    { "golem.inf.cpu.threads", @"8"},
                    { "golem.inf.mem.gib", @"16"},
                    { "golem.inf.storage.gib", @"30"},
                    { "golem.usage.vector", @"[""golem.usage.duration_sec""]"},
                    { "golem.com.payment.scheme", @"""after"""},
                    { "golem.com.pricing.model", @"""linear"""},
                    { "golem.com.pricing.est{*}", null},
                }
            };

            var offer1Subscription = providerProcessor.SubscribeOffer(offer1);
            var offer2Subscription = providerProcessor.SubscribeOffer(offer2);

            Assert.IsNotNull(offer1Subscription.Id);
            Assert.IsNotNull(offer1Subscription.Offer);
            Assert.IsNull(offer1Subscription.LastReceivedProposalId);

            Assert.IsNotNull(offer2Subscription.Id);
            Assert.IsNotNull(offer2Subscription.Offer);
            Assert.IsNull(offer2Subscription.LastReceivedProposalId);


            // ...at this point in scenario, the Collect on Requestor side should return an offer already...

            var reqEvents = await requestorProcessor.CollectRequestorEventsAsync(demandSubscription.Id, 1000, 10);

            var offerProposal = reqEvents.First();

            Assert.IsTrue(reqEvents.Any());

            Assert.AreEqual(offer2Subscription.Offer.Id, offerProposal.OfferProposal.Offer.Id);

            // OK, we got an offer proposal, we can now send a direct Demand counterproposal

            var counterDemand = new Demand()
            {
                Constraints = @"(&" +
                    @"(golem.srv.comp.container.docker.image=golemfactory/ffmpeg)" +
                    @"(golem.srv.comp.container.docker.benchmark{golemfactory/ffmpeg}>300)" +
                    @"(golem.com.payment.scheme=after)" +
                    @"(golem.usage.vector=[golem.usage.duration_sec])" +
                    @"(golem.com.pricing.model=linear)" +
                    @"(golem.com.pricing.model.linear.coeffs=*)" +
                //@"(golem.com.pricing.est{[30]}<125)" +
                @")",
                Id = null,
                NodeId = "RequestorA", // must be assigned from the outside
                Properties = new Dictionary<string, string>()
            };

            var counterDemandProposal = requestorProcessor.CreateDemandProposal(demandSubscription.Id, offerProposal.OfferProposal.Offer.Id, counterDemand);

            // At this point, the Provider should be able to read the Demand Proposal on the Offer subscription

            var provEvents = await providerProcessor.CollectProviderEventsAsync(offer2Subscription.Id, 1000, 10);

            Assert.IsTrue(provEvents.Any());
            Assert.AreEqual(counterDemandProposal.Demand.Id, provEvents.First().DemandProposal.Demand.Id);

            // ...if we call again - there should be no proposals, so a timeout is expected after 100ms
            var provEvents2 = await providerProcessor.CollectProviderEventsAsync(offer2Subscription.Id, 100, 10);

            Assert.IsFalse(provEvents2.Any());

            // Now test the blocking logic of Collect - hang on Collect on Requestor side, while Provider sends counter-offer

            ICollection<RequestorEvent> reqEvents2 = null;
            Task.Run(async () =>
            {
                reqEvents2 = await requestorProcessor.CollectRequestorEventsAsync(demandSubscription.Id, 5000, 10);

                Assert.IsTrue(reqEvents2.Any());
            });

            // ...and prepare counter-offer with price

            var counterOffer = new Offer()
            {
                Constraints = "()",
                Id = null,
                NodeId = "ProviderB",
                Properties = new Dictionary<string, string>()
                {
                    { "golem.srv.comp.container.docker.image", @"[""golemfactory/ffmpeg""]" },
                    { "golem.srv.comp.container.docker.benchmark{golemfactory/ffmpeg}", @"723"},
                    { "golem.srv.comp.container.docker.benchmark{*}", null},
                    { "golem.inf.cpu.cores", @"4"},
                    { "golem.inf.cpu.threads", @"8"},
                    { "golem.inf.mem.gib", @"16"},
                    { "golem.inf.storage.gib", @"30"},
                    { "golem.usage.vector", @"[""golem.usage.duration_sec""]"},
                    { "golem.com.payment.scheme", @"""after"""},
                    { "golem.com.pricing.model", @"""linear"""},
                    { "golem.com.pricing.model.linear.coeffs", @"[0, 0.2]"},
                    { "golem.com.pricing.est{*}", null},
                }
            };

            var counterOfferProposal = providerProcessor.CreateOfferProposal(offer2Subscription.Id, provEvents.First().DemandProposal.Demand.Id, counterOffer);

            Thread.Sleep(5000);


            // OK, so the reqEvents2 now contains an offer which is acceptable.
            // Requestor can move on to propose Agreement...

        }


    }
}
