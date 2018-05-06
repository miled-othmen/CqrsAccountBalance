namespace AccountBalance.Domain.Tests
{
    using AccountBalance.Domain.Tests.Common;
    using NodaTime;
    using ReactiveDomain.Messaging;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class CashDepositTests : TestsBase
    {
        public CashDepositTests(EventStoreFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task CanDepositCash()
        {
            Clock.SetCurrent(new LocalDateTime(2018, 1, 1, 9, 0));

            var created = new AccountCreated(CorrelatedMessage.NewRoot())
            {
                AccountId = AccountId,
                AccountHolderName = "NNN"
            };

            var cmd = new DepositCash
            {
                AccountId = AccountId,
                Amount = 350
            };

            var ev = new CashDeposited(cmd)
            {
                AccountId = AccountId,
                Amount = cmd.Amount,
                DepositedAt = Clock.Current
            };

            await Runner.Run(
                def => def.Given(created).When(cmd).Then(ev)
            );
        }
    }
}
