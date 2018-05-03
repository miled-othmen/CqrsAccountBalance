namespace AccountBalance.Domain.Tests.Common
{
    using Xunit;

    [CollectionDefinition("AggregateTest")]
    public sealed class AggregateTestCollection : ICollectionFixture<EventStoreFixture>
    {
    }
}
