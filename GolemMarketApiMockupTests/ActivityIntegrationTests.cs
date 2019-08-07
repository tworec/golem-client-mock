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
    public class ActivityIntegrationTests
    {
        public IAgreementRepository AgreementRepository { get; set; }
        public IActivityRepository ActivityRepository { get; set; }

        public IRequestorActivityProcessor RequestorProcessor { get; set; }
        public IProviderActivityProcessor ProviderProcessor { get; set; }

        [TestInitialize]
        public void InitializeTests()
        {
            this.AgreementRepository = new InProcessAgreementRepository();
            this.ActivityRepository = new InProcessActivityRepository();

            var activityProcessor = new InMemoryActivityProcessor(
                this.AgreementRepository,
                this.ActivityRepository);

            this.RequestorProcessor = activityProcessor;
            this.ProviderProcessor = activityProcessor;
        }


        [TestMethod]
        public async Task InMemoryActivityProcessor_Integration_SimplePositiveScenario()
        {
            // Test scenario

            var demand = new Demand() { Constraints = "", NodeId = "DummyRequestorNode1", Properties = new Dictionary<string, string>() };
            var offer = new OfferProposal() {
                            DemandId = "dummyDemandId",
                            Id = "DummyProposalId",
                            InternalId = 1,
                            Offer = new Offer() {
                                Constraints = "",
                                NodeId = "DummyProviderNode1",
                                Properties = new Dictionary<string, string>()
                            }
            };
            var agreement = this.AgreementRepository.CreateAgreement(demand, offer);

            // 1. Create Activity in a known, negotiated agreement

            var activity = this.RequestorProcessor.CreateActivity(agreement.Id);

            var provEvents = await this.ProviderProcessor.CollectActivityEventsAsync(offer.Offer.NodeId, 1000);

            // 2. Send a sample ExeScript command batch

            var batchId = this.RequestorProcessor.ExecAsync(activity.Id, new ExeScript() { Text = "DEPLOY\nSTART\n" });

            // 2.1. Simulate ExeScript execution on Provider side

            Task.Run(async () =>
            {
                bool isActivityDestroyed = false;

                do
                {
                    provEvents = await this.ProviderProcessor.CollectActivityEventsAsync(offer.Offer.NodeId, 1000);

                    foreach(var provEvent in provEvents)
                    {
                        switch(provEvent.EventType)
                        {
                            case ActivityProviderEvent.ActivityProviderEventType.Exec:
                                var commands = provEvent.ExeScript.Text.Split('\n');
                                foreach(var command in commands)
                                {
                                    if(string.IsNullOrWhiteSpace(command))
                                        continue;

                                    if(command.Contains("DEPLOY"))
                                    {
                                        this.ActivityRepository.SetActivityState(activity.Id, ActivityState.Deploying);
                                        Thread.Sleep(1000);
                                        this.ActivityRepository.SetActivityState(activity.Id, ActivityState.Ready);
                                    }

                                    if (command.Contains("START"))
                                    {
                                        this.ActivityRepository.SetActivityState(activity.Id, ActivityState.Starting);
                                        Thread.Sleep(1000);
                                        this.ActivityRepository.SetActivityState(activity.Id, ActivityState.Terminated);
                                    }

                                    activity = this.ActivityRepository.GetActivity(activity.Id);

                                    this.ProviderProcessor.SendActivityExecResult(activity.Id, batchId, new ActivityExecResult()
                                    {
                                        CurrentState = activity.State,
                                        Result = ActivityExecResult.ActivityExecResultEnum.OK
                                    });
                                }
                                break;
                            case ActivityProviderEvent.ActivityProviderEventType.DestroyActivity:
                                isActivityDestroyed = true;
                                break;
                        }
                    }
                }
                while (isActivityDestroyed != true);

            });


            // 2.2. Process the ExeScript results on Requestor side

            bool isActivityFinished = false;
            do
            {
                var reqEvents = await this.RequestorProcessor.GetExecBatchResultsAsync(batchId, 1000);

                foreach(var reqEvent in reqEvents)
                {
                    Assert.AreEqual(ActivityExecResult.ActivityExecResultEnum.OK, reqEvent.ExecResult.Result);

                    if (reqEvent.ExecResult.CurrentState == ActivityState.Terminated)
                        isActivityFinished = true;
                }

            }
            while (!isActivityFinished);

            // 3. Destroy Activity

            this.RequestorProcessor.DestroyActivity(activity.Id);

        }



    }
}
