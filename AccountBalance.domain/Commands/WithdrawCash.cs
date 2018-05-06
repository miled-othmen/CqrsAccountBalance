namespace AccountBalance.Domain
{
    using NodaTime;
    using ReactiveDomain.Messaging;
    using System;

    public class WithdrawCash : Command
    {
        public WithdrawCash() : base(NewRoot())
        { }

        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}
