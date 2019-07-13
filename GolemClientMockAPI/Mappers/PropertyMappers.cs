using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public static class PropertyMappers
    {
        public static IDictionary<string, string> MapFromJsonString(string propertiesJson)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(propertiesJson);
        }
    }
}
