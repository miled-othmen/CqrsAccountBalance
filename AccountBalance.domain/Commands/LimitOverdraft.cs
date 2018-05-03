namespace AccountBalance.Domain
{
    using System;
    using ReactiveDomain.Messaging;

    public class LimitOverdraft : Command
    {
        public LimitOverdraft() : base(NewRoot())
        { }

        public Guid AccountId { get; set; }
        public int Limit { get; set; }
    }
}
