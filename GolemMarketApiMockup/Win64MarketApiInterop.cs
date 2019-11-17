using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GolemMarketApiMockup
{
    class Win64MarketApiInterop : IMarketApiInterop
    {
        [DllImport("runtimes/win-x64/native/market_api.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 match_demand_offer(StringRef[] demand_props, uint demand_props_count,
                                               StringRef demand_constraints,
                                               StringRef[] offer_props, uint offer_props_count,
                                               StringRef offer_constraints);

        [DllImport("runtimes/win-x64/native/market_api.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 resolve_expression(StringRef expression, StringRef[] props, uint props_count);


        public Int32 MatchDemandOffer(StringRef[] demand_props, uint demand_props_count,
                                               StringRef demand_constraints,
                                               StringRef[] offer_props, uint offer_props_count,
                                               StringRef offer_constraints)
        {
            return match_demand_offer(demand_props, demand_props_count,
                                      demand_constraints,
                                      offer_props, offer_props_count,
                                      offer_constraints);
        }

        public Int32 ResolveExpression(StringRef expression, StringRef[] props, uint props_count)
        {
            return resolve_expression(expression, props, props_count);
        }

    }
}
