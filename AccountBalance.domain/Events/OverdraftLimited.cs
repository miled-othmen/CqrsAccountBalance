namespace AccountBalance.Domain
{
    using ReactiveDomain.Messaging;
    using Newtonsoft.Json;
    using System;

    public class OverdraftLimited : Event
    {
        public OverdraftLimited(CorrelatedMessage source) : base(source)
        { }

        [JsonConstructor]
        public OverdraftLimited(CorrelationId correlationId, SourceId sourceId)
            : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public int Limit { get; set; }
    }
}
