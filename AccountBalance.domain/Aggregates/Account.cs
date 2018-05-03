namespace AccountBalance.Domain
{
    using System;
    using ReactiveDomain.Messaging;
    using ReactiveDomain;

    public class Account : EventDrivenStateMachine
    {
        Account()
        {
            Register<AccountCreated>(
                e => { Id = e.AccountId; }
            );
        }

        public static Account Create(Guid id, string name, CorrelatedMessage source)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidOperationException("A valid account owner name must be provided");

            var account = new Account();
            account.Raise(new AccountCreated(source)
            {
                AccountId = id,
                AccountHolderName = name
            });
            return account;
        }
        public void SetOverdraftLimit(Guid id, int limit, CorrelatedMessage source)
        {
            if (limit < 0)
                throw new InvalidOperationException("Overdraft limit should be positve");

            Raise(new OverdraftLimited(source)
            {
                AccountId = id,
                Limit = limit
            });
        }

        public void SetDailyWireTransfertLimit(Guid id, int limit, CorrelatedMessage source)
        {
            if (limit < 0)
                throw new InvalidOperationException("Daily transfert limit should be positve");

            Raise(new DailyWireTransfertLimitSet(source)
            {
                AccountId = id,
                Limit = limit
            });
        }
    }
}
