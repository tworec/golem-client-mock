using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GolemMarketApiMockup
{
    interface IMarketApiInterop
    {
        Int32 MatchDemandOffer(StringRef[] demand_props, uint demand_props_count,
                                               StringRef demand_constraints,
                                               StringRef[] offer_props, uint offer_props_count,
                                               StringRef offer_constraints);

        Int32 ResolveExpression(StringRef expression, StringRef[] props, uint props_count);

    }
}
