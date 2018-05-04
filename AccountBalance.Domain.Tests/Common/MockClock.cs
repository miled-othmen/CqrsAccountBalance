namespace AccountBalance.Domain.Tests
{
    using NodaTime;
    using NodaTime.TimeZones;

    public sealed class MockClock : IClock
    {
        readonly DateTimeZone _defaultTimeZone;
        Instant _currentTime;

        public MockClock(DateTimeZone defaultTimeZone)
        {
            _defaultTimeZone = defaultTimeZone;
        }

        public MockClock(string tzDbTimeZoneId)
        {
            _defaultTimeZone = TzdbDateTimeZoneSource.Default.ForId(tzDbTimeZoneId);
        }

        public void SetCurrent(Instant instant)
        {
            _currentTime = instant;
        }

        public void SetCurrent()
        {
            SetCurrent(SystemClock.Instance.GetCurrentInstant());
        }

        public void SetCurrent(LocalDateTime local, DateTimeZone timeZone = null)
        {
            SetCurrent(local.InZoneLeniently(timeZone ?? _defaultTimeZone));
        }

        public void SetCurrent(ZonedDateTime zoned)
        {
            SetCurrent(zoned.ToInstant());
        }

        public Instant GetInstant(LocalDateTime local) => local.InZoneLeniently(_defaultTimeZone).ToInstant();

        public Instant Current => _currentTime;

        public Instant GetCurrentInstant() => _currentTime;
    }
}
