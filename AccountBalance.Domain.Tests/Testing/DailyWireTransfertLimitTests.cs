namespace AccountBalance.Domain.Tests.Testing
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using ReactiveDomain.Messaging;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class DailyWireTransfertLimitTests : TestsBase
    {
        public DailyWireTransfertLimitTests(EventStoreFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task CanSetDailyTransfertLimit()
        {
            var created = new AccountCreated(CorrelatedMessage.NewRoot())
            {
                AccountId = AccountId,
                AccountHolderName = "Miled",
                OverdraftLimit = 0
            };

            var cmd = new SetDailyWireTransfertLimit
            {
                AccountId = AccountId,
                Limit = 350
            };

            var ev = new DailyWireTransfertLimitSet(cmd)
            {
                AccountId = AccountId,
                Limit = 350
            };

            await Runner.Run(
                def => def.Given(created).When(cmd).Then(ev)
            );
        }

        [Fact]
        public async Task CannotSetNegativeLimit()
        {
            var created = new AccountCreated(CorrelatedMessage.NewRoot())
            {
                AccountId = AccountId,
                AccountHolderName = "NNN"
            };

            var cmd = new SetDailyWireTransfertLimit
            {
                AccountId = AccountId,
                Limit = -1
            };

            await Runner.Run(
                def => def.Given(created).When(cmd).Throws(new InvalidOperationException("Daily transfert limit should be positve"))
            );
        }

        [Fact]
        public async Task CannotSetLimitOnInvalidAccount()
        {
            var cmd = new SetDailyWireTransfertLimit
            {
                AccountId = AccountId,
                Limit = Convert.ToInt32(100m)
            };

            await Runner.Run(
                def => def.Given().When(cmd).Throws(new InvalidOperationException("No Account is already exist"))
            );
        }


    }
}