using GolemMarketApiMockupTests;
using System;

namespace GolemMarketApiMockupRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var resolver = new GolemMarketApiMockup.GolemMarketResolver();

            var result2 = resolver.MatchDemandOffer(new string[] { "d1=\"v1\"" }, "(o1=v2)",
                                                   new string[] { "o1=\"v2\"" }, "(d1=v1)");


            var test = new MarketIntegrationTests();

            //test.InMemoryMarketResolver_Integration_SimplePositiveScenario();

            var test2 = new GolemMarketResolverTests();

            test2.MatchDemandOffer_ShouldReturnTrue_ForMatchingOneSidedDemandOffer_Complex();


            Console.ReadKey();
        }
    }
}
