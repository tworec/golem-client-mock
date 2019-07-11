using Autofac;
using GolemClientMockAPI.Mappers;
using GolemClientMockAPI.Processors;
using GolemClientMockAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolemClientMockAPI.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<RequestorEventMapper>().AsSelf()
                 .SingleInstance();
            builder.RegisterType<DemandMapper>().AsSelf()
                 .SingleInstance();

            builder.RegisterType<InMemoryMarketProcessor>()
                .As<IRequestorMarketProcessor>()
                .As<IProviderMarketProcessor>()
                .SingleInstance();

            builder.RegisterType<InProcessAgreementRepository>()
                .As<IAgreementRepository>()
                .SingleInstance();
            builder.RegisterType<InProcessSubscriptionProposalRepository>()
                .As<ISubscriptionRepository>()
                .As<IProposalRepository>()
                .SingleInstance();
        }
    }
}
