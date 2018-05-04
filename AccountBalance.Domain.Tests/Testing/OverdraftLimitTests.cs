namespace AccountBalance.Domain.Tests
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using ReactiveDomain.Messaging;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class OverdraftLimitTests : TestsBase
    {
        public OverdraftLimitTests(EventStoreFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task CanLimitOverdraft()
        {
            var created = new AccountCreated(CorrelatedMessage.NewRoot())
            {
                AccountId = AccountId,
                AccountHolderName = "Miled",
                OverdraftLimit = 0
            };

            var cmd = new LimitOverdraft
            {
                AccountId = AccountId,
                Limit = 350
            };

            var ev = new OverdraftLimited(cmd)
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

            var cmd = new LimitOverdraft
            {
                AccountId = AccountId,
                Limit = -1
            };

            await Runner.Run(
                def => def.Given(created).When(cmd).Throws(new InvalidOperationException("Overdraft limit should be positve"))
            );
        }

        [Fact]
        public async Task CannotSetLimitOnInvalidAccount()
        {
            var cmd = new LimitOverdraft
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