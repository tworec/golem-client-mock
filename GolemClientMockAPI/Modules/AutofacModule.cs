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

            builder.RegisterType<MarketRequestorEventMapper>().AsSelf()
                 .SingleInstance();
            builder.RegisterType<MarketProviderEventMapper>().AsSelf()
                 .SingleInstance();
            builder.RegisterType<DemandMapper>().AsSelf()
                 .SingleInstance();
            builder.RegisterType<OfferMapper>().AsSelf()
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
                .As<IStatsRepository>()
                .SingleInstance();
            builder.RegisterType<InProcessActivityRepository>()
                .As<IActivityRepository>()
                .SingleInstance();

            builder.RegisterType<ExeScriptMapper>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<ActivityProviderEventMapper>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<InMemoryActivityProcessor>()
                .As<IRequestorActivityProcessor>()
                .As<IProviderActivityProcessor>()
                .SingleInstance();
        }
    }
}
