namespace AccountBalance.Domain.Tests
{
    using System;
    using System.Threading.Tasks;
    using CommandHandlers;
    using Common;
    using ReactiveDomain.Messaging;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class AccountTests : IDisposable
    {
        readonly Guid _accountId;
        readonly EmbeddedEventStoreScenarioRunnerX<Account> _runner;

        public void Dispose()
        {
            _runner.Dispose();
        }

        public AccountTests(EventStoreFixture fixture)
        {
            _accountId = Guid.NewGuid();
            _runner = new EmbeddedEventStoreScenarioRunnerX<Account>(
                _accountId,
                fixture,
                (repository, dispatcher) => new AccountCommandHandler(repository, dispatcher));
        }

        [Fact]
        public async Task CanCreateAccount()
        {
            var cmd = new CreateAccount
            {
                AccountId = _accountId,
                AccountHolderName = "NNN"
            };

            var ev = new AccountCreated(cmd)
            {
                AccountId = cmd.AccountId,
                AccountHolderName = cmd.AccountHolderName,
                OverdraftLimit = 0
            };

            await _runner.Run(
                def => def.Given().When(cmd).Then(ev)
            );
        }
    }
}
