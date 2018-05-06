namespace AccountBalance.Domain
{
    using ReactiveDomain.Messaging;
    using System;

    public class WireTransferCash : Command
    {
        public WireTransferCash() : base(NewRoot())
        { }

        public Guid AccountId { get; set; }
        public double Amount { get; set; }
    }
}
