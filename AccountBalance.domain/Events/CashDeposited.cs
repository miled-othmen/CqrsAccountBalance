namespace AccountBalance.Domain
{
    using Newtonsoft.Json;
    using NodaTime;
    using ReactiveDomain.Messaging;
    using System;

    public class CashDeposited : Event
    {
        public CashDeposited(CorrelatedMessage source) : base(source)
        { }

        [JsonConstructor]
        protected CashDeposited(CorrelationId correlationId, SourceId sourceId) : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public double Amount { get; set; }
        public Instant DepositedAt { get; set; }
    }
}
