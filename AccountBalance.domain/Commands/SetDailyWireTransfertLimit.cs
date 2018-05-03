namespace AccountBalance.Domain
{
    using System;
    using ReactiveDomain.Messaging;

    public class SetDailyWireTransfertLimit : Command
    {
        public SetDailyWireTransfertLimit() : base(NewRoot())
        { }

        public Guid AccountId { get; set; }
        public int Limit { get; set; }
    }
}
