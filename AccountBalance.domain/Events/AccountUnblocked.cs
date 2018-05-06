namespace AccountBalance.Domain
{
    using Newtonsoft.Json;
    using ReactiveDomain.Messaging;
    using System;

    public class AccountUnblocked : Event
    {
        public AccountUnblocked(CorrelatedMessage source) : base(source)
        { }

        [JsonConstructor]
        public AccountUnblocked(CorrelationId correlationId, SourceId sourceId) : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public string Raison { get; set; }
    }
}
