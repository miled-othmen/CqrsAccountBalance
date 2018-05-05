namespace AccountBalance.Domain
{
    using System;
    using NodaTime;

    public class Cheque
    {
        public Guid ChequeId { get; set; }
        public double Amount { get; set; }
        public Instant DepositAt { get; set; }
        public Instant ClearanceAt { get; set; }
        public bool IsCleared { get; set; }
    }
}
