namespace AccountBalance.Domain.Tests
{
    using AccountBalance.Domain.Tests.Common;
    using NodaTime;
    using ReactiveDomain.Messaging;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class WithdrawCashTests : TestsBase
    {
        public WithdrawCashTests(EventStoreFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task CanWithdrawCash()
        {
            Clock.SetCurrent(new LocalDateTime(2018, 1, 1, 9, 0));

            var accountCreated = new AccountCreated(CorrelatedMessage.NewRoot())
            {
                AccountId = AccountId,
                AccountHolderName = "NNN"
            };

            var cashDeposited = new CashDeposited(accountCreated)
            {
                AccountId = AccountId,
                Amount = 350,
                DepositedAt = Clock.Current
            };

            var cmd = new WithdrawCash()
            {
                AccountId = AccountId,
                Amount = 200
            };

            var ev = new CashWithdrawn(cmd)
            {
                AccountId = AccountId,
                Amount = 200,
                WithdrawnAt = Clock.Current
            };

            await Runner.Run(
                def => def.Given(accountCreated, cashDeposited).When(cmd).Then(ev)
            );
        }

    }
}
