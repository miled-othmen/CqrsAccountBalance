namespace AccountBalance.Domain
{
    using System;
    using ReactiveDomain.Messaging;
    using Newtonsoft.Json;


    public class AccountCreated : Event
    {
        public AccountCreated(CorrelatedMessage source)
            : base(source)
        { }

        //every event needs that JsonConstructor attributed constructor or it cannot be deserialized from eventstore
        [JsonConstructor]
        public AccountCreated(CorrelationId correlationId, SourceId sourceId)
                    : base(correlationId, sourceId)
        { }

        public Guid AccountId { get; set; }
        public string AccountHolderName { get; set; }
    }
}
