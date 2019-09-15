using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GolemClientMockAPI.Extensions;

namespace GolemClientMockAPI.Mappers
{
    public class ActivityProviderEventMapper
    {
        public IMapper Mapper { get; set; }

        public ActivityProviderEventMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public GolemClientMockAPI.ActivityAPI.Models.ProviderEvent Map(Entities.ActivityProviderEvent providerEventEntity)
        {
            switch (providerEventEntity.EventType)
            {
                case Entities.ActivityProviderEvent.ActivityProviderEventType.CreateActivity:
                    return this.Mapper.Map<GolemClientMockAPI.ActivityAPI.Models.CreateActivityProviderEvent>(providerEventEntity);
                case Entities.ActivityProviderEvent.ActivityProviderEventType.DestroyActivity:
                    return this.Mapper.Map<GolemClientMockAPI.ActivityAPI.Models.ProviderEvent>(providerEventEntity);
                case Entities.ActivityProviderEvent.ActivityProviderEventType.Exec:
                    return this.ParseExecEvent(providerEventEntity);
                case Entities.ActivityProviderEvent.ActivityProviderEventType.GetState:
                    return this.Mapper.Map<GolemClientMockAPI.ActivityAPI.Models.ProviderEvent>(providerEventEntity);
                default:
                    throw new Exception($"Unknown ProviderEventType {providerEventEntity.EventType} ");
            }
        }

        protected ActivityAPI.Models.ProviderEvent ParseExecEvent(Entities.ActivityProviderEvent providerEventEntity)
        {
            var result = this.Mapper.Map<GolemClientMockAPI.ActivityAPI.Models.ExecProviderEvent>(providerEventEntity);

            result.ExeScript = new ActivityAPI.Models.ExeScriptBatch()
            {
                Commands = new List<ActivityAPI.Models.ExeScriptCommand>()
            };


            var lines = providerEventEntity.ExeScript.Text.Split(Environment.NewLine);

            foreach(var line in lines)
            {
                var tokens = SplitCommandLine(line);

                if (tokens.Count() > 0)
                {
                    result.ExeScript.Commands.Add(new ActivityAPI.Models.ExeScriptCommand()
                    {
                        Command = tokens.First(),
                        Params = tokens.Skip(1).ToList()
                    });
                }
            }


            return result;
        }

        public static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            bool inQuotes = false;

            return commandLine.Split(c =>
            {
                if (c == '\"')
                    inQuotes = !inQuotes;

                return !inQuotes && c == ' ';
            })
            .Select(arg => arg.Trim().TrimMatchingQuotes('\"'))
            .Where(arg => !string.IsNullOrEmpty(arg));
        }
    }
}
