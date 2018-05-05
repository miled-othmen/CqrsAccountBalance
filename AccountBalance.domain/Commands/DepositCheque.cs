namespace AccountBalance.Domain
{
    using System;
    using ReactiveDomain.Messaging;

    public class DepositCheque : Command
    {
        public DepositCheque() : base(NewRoot()) { }


        public Guid AccountId { get; set; }

        public Guid ChequeId { get; set; }

        public double Amount { get; set; }
    }
}
