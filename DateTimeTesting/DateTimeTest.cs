using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DateTimeTesting
{
    public class DateTimeTest
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public DateTimeOffset? NullableTimestamp { get; set; }

        public DateTimeOffset? FixedNullableTimestamp => NullableTimestamp.HasValue
            ? TimeZoneOffset == "Z"
                ? NullableTimestamp
                : NullableTimestamp.Value.ToOffset(new TimeSpan(
                    hours: int.Parse(TimeZoneOffset.Substring(0, 3)),
                    minutes: int.Parse(TimeZoneOffset.Substring(4, 2)),
                    seconds: 0
                ))
            : null;
        public string TimeZoneId { get; set; }
        public string TimeZoneOffset { get; set; }
    }

    public class DateTimeWithTzInfo
    {
        public DateTimeOffset Timestamp { get; set; }
        public string TimeZoneId { get; set; }
        public string TimeZoneOffset { get; set; }
    }
}
