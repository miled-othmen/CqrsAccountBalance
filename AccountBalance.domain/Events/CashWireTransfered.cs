namespace AccountBalance.Domain
{
    using Newtonsoft.Json;
    using NodaTime;
    using ReactiveDomain.Messaging;
    using System;

    public class CashWireTransfered : Event
    {
        public CashWireTransfered(CorrelatedMessage source) : base(source)
        { }

        [JsonConstructor]
        public CashWireTransfered(CorrelationId correlationId, SourceId sourceId) : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public double Amount { get; set; }
        public Instant WireTransferedAt { get; set; }
    }
}
