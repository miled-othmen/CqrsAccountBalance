namespace AccountBalance.Domain
{
    using System;
    using Newtonsoft.Json;
    using NodaTime;
    using ReactiveDomain.Messaging;

    public class ChequeDeposited : Event
    {
        public ChequeDeposited(CorrelatedMessage source) : base(source)
        {}

        [JsonConstructor]
        public ChequeDeposited(CorrelationId correlationId, SourceId sourceId) : base(correlationId, sourceId)
        {}

        public Guid AccountId { get; set; }

        public Guid ChequeId { get; set; }

        public double Amount { get; set; }

        public Instant DepositTime { get; set; }

        public Instant ClearanceTime { get; set; }
    }
}
