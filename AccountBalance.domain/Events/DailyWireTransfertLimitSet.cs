namespace AccountBalance.Domain
{
    using System;
    using Newtonsoft.Json;
    using ReactiveDomain.Messaging;

    public class DailyWireTransfertLimitSet : Event
    {
        public DailyWireTransfertLimitSet(CorrelatedMessage source) : base(source)
        { }

        [JsonConstructor]
        public DailyWireTransfertLimitSet(CorrelationId correlationId, SourceId sourceId)
            : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public int Limit { get; set; }
    }
}
