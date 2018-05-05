namespace AccountBalance.Domain.Tests
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using NodaTime;
    using ReactiveDomain.Messaging;
    using Xunit;
    using Xunit.ScenarioReporting;

    [Collection("AggregateTest")]
    public class DepositChequeTests: TestsBase
    {
        public DepositChequeTests(EventStoreFixture fixture) : base(fixture)
        {}

        //[Fact]
        //public async Task CanDepositCheque()
        //{
        //    var chequeId = Guid.NewGuid();
        //    var amount = 100;

        //    //Monday
        //    var depositedAt = new LocalDateTime(2018, 1, 1, 9, 0);
        //    //Tuesday
        //    var clearedAt = new LocalDateTime(2018, 1, 2, 9, 0);

        //    Clock.SetCurrent(depositedAt);

        //    var accountCreated = new AccountCreated(CorrelatedMessage.NewRoot())
        //    {
        //        AccountId = AccountId,
        //        AccountHolderName = "Yakoub Klai"
        //    };

        //    var cmd = new DepositCheque
        //    {
        //        AccountId = AccountId,
        //        ChequeId = chequeId,
        //        Amount = amount
        //    };

        //    var chequeDeposited = new ChequeDeposited(cmd)
        //    {
        //        AccountId = AccountId,
        //        ChequeId = chequeId,
        //        Amount = amount,
        //        DepositTime = Clock.Current,
        //        ClearanceTime = Clock.GetInstant(clearedAt)
        //    };

        //    await Runner.Run(
        //        def => def.Given(accountCreated).When(cmd).Then(chequeDeposited),
        //        title: "Should deposit a cheque"
        //    );
        //}
    }
}
