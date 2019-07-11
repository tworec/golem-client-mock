using GolemMarketApiMockup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GolemMarketApiMockupTests
{
    [TestClass]
    public class GolemMarketResolverTests
    {
        [TestMethod]
        public void MatchDemandOffer_ShouldReturnTrue_ForMatchingDemandOffer()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.True, resolver.MatchDemandOffer(new string[] { "d1=\"v1\"" }, "(o1=v2)",
                                                   new string[] { "o1=\"v2\"" }, "(d1=v1)"));

        }

        [TestMethod]
        public void MatchDemandOffer_ShouldReturnTrue_ForMatchingOneSidedDemandOffer()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.True, resolver.MatchDemandOffer(new string[] { }, "(o1=v2)",
                                                   new string[] { "o1=\"v2\"" }, "()"));

        }

        [TestMethod]
        public void MatchDemandOffer_ShouldReturnTrue_ForMatchingOneSidedDemandOffer_Complex()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.True, resolver.MatchDemandOffer(new string[] { }, @"(&" +
                            @"(golem.srv.comp.container.docker.image=golemfactory/ffmpeg)" +
                            @"(golem.srv.comp.container.docker.benchmark{golemfactory/ffmpeg}>300)" +
                            @"(golem.com.payment.scheme=after)" +
                            @"(golem.usage.vector=[golem.usage.duration_sec])" +
                            @"(golem.com.pricing.model=linear)" +
                            ")",
                        new string[] {
                            @"golem.srv.comp.container.docker.image=[""golemfactory/ffmpeg""]",
                            @"golem.srv.comp.container.docker.benchmark{golemfactory/ffmpeg}=760",
                            @"golem.srv.comp.container.docker.benchmark{*}",
                            @"golem.inf.cpu.cores=4",
                            @"golem.inf.cpu.threads=8",
                            @"golem.inf.mem.gib=16",
                            @"golem.inf.storage.gib=30",
                            @"golem.usage.vector=[""golem.usage.duration_sec""]",
                            @"golem.com.payment.scheme=""after""",
                            @"golem.com.pricing.model=""linear""",
                            @"golem.com.pricing.est{*}"
                        }, "()"));

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MatchDemandOffer_ShouldReturnFalse_ForNonMatchingDemandOffer()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.False, resolver.MatchDemandOffer(new string[] { "d1=\"v3\"" }, "(o1=v2)",
                                                   new string[] { "o1=\"v2\"" }, "(d1=v1)"));

        }

        [TestMethod]
        public void MatchDemandOffer_ShouldReturnUndefined_ForDemandOfferWithMissingProperty()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.Undefined, resolver.MatchDemandOffer(new string[] { "d5=\"v1\"" }, "(o1=v2)",
                                                   new string[] { "o1=\"v2\"" }, "(d1=v1)"));

        }

        [TestMethod]
        public void MatchDemandOffer_ShouldReturnError_ForDemandOfferWithInvalidConstraints()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.Error, resolver.MatchDemandOffer(new string[] { "d5=\"v1\"" }, "werwer(werwerewro1=v2)",
                                                   new string[] { "o1=\"v2\"" }, "(d1=v1)"));

        }

        [TestMethod]
        public void ResolveExpression_ShouldReturnTrue_ForMatchingProps()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.True, resolver.ResolveExpression("(d1=v1)", new string[] { "d1=\"v1\"" }));

        }

        [TestMethod]
        public void ResolveExpression_ShouldReturnFalse_ForNonMatchingProps()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.False, resolver.ResolveExpression("(d1=v1)", new string[] { "d1=\"v3\"" }));

        }

        [TestMethod]
        public void ResolveExpression_ShouldReturnUndefined_ForMissingMatchingProps()
        {
            var resolver = new GolemMarketResolver();

            Assert.AreEqual(GolemMarketResolver.ResultEnum.Undefined, resolver.ResolveExpression("(d1=v1)", new string[] { "d2=\"v3\"" }));

        }


    }
}
