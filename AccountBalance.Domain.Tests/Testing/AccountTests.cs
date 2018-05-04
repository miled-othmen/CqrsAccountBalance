namespace AccountBalance.Domain.Tests
{
    using System.Threading.Tasks;
    using Common;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class AccountTests : TestsBase
    {
        public AccountTests(EventStoreFixture fixture) : base(fixture)
        { }

        [Fact]
        public async Task CanCreateAccount()
        {
            var cmd = new CreateAccount
            {
                AccountId = AccountId,
                AccountHolderName = "NNN"
            };

            var ev = new AccountCreated(cmd)
            {
                AccountId = cmd.AccountId,
                AccountHolderName = cmd.AccountHolderName,
                OverdraftLimit = 0
            };

            await Runner.Run(
                def => def.Given().When(cmd).Then(ev)
            );
        }
    }
}
