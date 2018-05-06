namespace AccountBalance.Domain
{
    using Newtonsoft.Json;
    using ReactiveDomain.Messaging;
    using System;

    public class AccountBlocked : Event
    {
        public AccountBlocked(CorrelatedMessage source) : base(source)
        { }

        [JsonConstructor]
        public AccountBlocked(CorrelationId correlationId, SourceId sourceId) : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public string Raison { get; set; }
    }
}
