namespace AccountBalance.Domain
{
    using Newtonsoft.Json;
    using NodaTime;
    using ReactiveDomain.Messaging;
    using System;

    public class CashWithdrawn : Event
    {
        public CashWithdrawn(CorrelatedMessage source) : base(source)
        { }

        [JsonConstructor]
        public CashWithdrawn(CorrelationId correlationId, SourceId sourceId) : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public double Amount { get; set; }
        public Instant WithdrawnAt { get; set; }

    }
}
