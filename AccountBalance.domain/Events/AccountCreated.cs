namespace AccountBalance.Domain
{
    using System;
    using ReactiveDomain.Messaging;

    public class AccountCreated : Event
    {
        public AccountCreated(CorrelatedMessage source)
            : base(source)
        { }

        public Guid AccountId { get; set; }
        public string AccountHolderName { get; set; }
    }
}
