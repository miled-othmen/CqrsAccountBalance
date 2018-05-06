namespace AccountBalance.Domain
{
    using ReactiveDomain.Messaging;
    using System;

    public class DepositCash : Command
    {
        public DepositCash() : base(NewRoot())
        { }

        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}
