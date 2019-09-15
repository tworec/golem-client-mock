using AutoMapper;
using GolemClientMockAPI.ActivityAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Mappers
{
    public class ExeScriptMapper
    {
        public IMapper Mapper { get; set; }

        public ExeScriptMapper(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        public Entities.ExeScript MapToEntity(ExeScriptRequest batch)
        {
            var result = new Entities.ExeScript()
            {
                Text = batch.Text
            };

            return result;
        }

        public Entities.ActivityExecResult MapResultToEntity(ExeScriptCommandResult commandResult)
        {
            return this.Mapper.Map<Entities.ActivityExecResult>(commandResult);

        }

        public List<ExeScriptCommandResult> MapResultsFromEntity(ICollection<Entities.ActivityRequestorEvent> results)
        {
            return this.Mapper.Map<List<ExeScriptCommandResult>>(results);
        }

    }
}
