namespace AccountBalance.Domain.Tests
{
    using System;
    using CommandHandlers;
    using Common;

    public abstract class TestsBase : IDisposable
    {
        protected MockClock Clock { get; }

        protected Guid AccountId { get; }

        protected EmbeddedEventStoreScenarioRunnerX<Account> Runner { get; }

        protected TestsBase(EventStoreFixture fixture)
        {
            AccountId = Guid.NewGuid();
            Clock = new MockClock("Europe/London");
            Runner = new EmbeddedEventStoreScenarioRunnerX<Account>(
                AccountId,
                fixture,
                (repository, dispatcher) => new AccountCommandHandler(repository, dispatcher/*, Clock*/));
        }

        public void Dispose()
        {
            Runner.Dispose();
        }
    }
}
