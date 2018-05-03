namespace AccountBalance.Domain.Tests.Testing
{
    using System;
    using System.Threading.Tasks;
    using CommandHandlers;
    using Common;
    using ReactiveDomain.Messaging;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class DailyWireTransfertLimitTests : IDisposable
    {
        readonly Guid _accountId;
        readonly EmbeddedEventStoreScenarioRunnerX<Account> _runner;

        public void Dispose()
        {
            _runner.Dispose();
        }

        public DailyWireTransfertLimitTests(EventStoreFixture fixture)
        {
            _accountId = Guid.NewGuid();
            _runner = new EmbeddedEventStoreScenarioRunnerX<Account>(
                _accountId,
                fixture,
                (repository, dispatcher) => new AccountCommandHandler(repository, dispatcher));
        }

        [Fact]
        public async Task CanSetDailyTransfertLimit()
        {
            var created = new AccountCreated(CorrelatedMessage.NewRoot())
            {
                AccountId = _accountId,
                AccountHolderName = "Miled",
                OverdraftLimit = 0
            };

            var cmd = new SetDailyWireTransfertLimit
            {
                AccountId = _accountId,
                Limit = 350
            };

            var ev = new DailyWireTransfertLimitSet(cmd)
            {
                AccountId = _accountId,
                Limit = 350
            };

            await _runner.Run(
                def => def.Given(created).When(cmd).Then(ev)
            );
        }

        [Fact]
        public async Task CannotSetNegativeLimit()
        {
            var created = new AccountCreated(CorrelatedMessage.NewRoot())
            {
                AccountId = _accountId,
                AccountHolderName = "NNN"
            };

            var cmd = new SetDailyWireTransfertLimit
            {
                AccountId = _accountId,
                Limit = -1
            };

            await _runner.Run(
                def => def.Given(created).When(cmd).Throws(new InvalidOperationException("Daily transfert limit should be positve"))
            );
        }

        [Fact]
        public async Task CannotSetLimitOnInvalidAccount()
        {
            var cmd = new SetDailyWireTransfertLimit
            {
                AccountId = _accountId,
                Limit = Convert.ToInt32(100m)
            };

            await _runner.Run(
                def => def.Given().When(cmd).Throws(new InvalidOperationException("No Account is already exist"))
            );
        }
    }
}