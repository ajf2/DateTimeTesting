using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DateTimeTesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DateTimeTestingController : ControllerBase
    {
        private readonly ILogger<DateTimeTestingController> _logger;
        private const string connectionString = "Server=(localdb)\\MSSQLLocalDB; Database=DateTimeTesting; Trusted_connection=true";

        public DateTimeTestingController(ILogger<DateTimeTestingController> logger)
        {
            _logger = logger;
        }

        [HttpGet, Route("setNow")]
        public DateTimeTest SetNow()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dtOffset = DateTimeOffset.UtcNow;
                var dt = DateTime.UtcNow;
                var dbNow = connection.QuerySingle<DateTimeTest>(@"
                    INSERT INTO DateTimeTesting ([Timestamp], NullableTimestamp, TimeZoneId, TimeZoneOffset) VALUES (@dtOffset, @dt, @id, @offset)
                    SELECT * FROM DateTimeTesting WHERE Id = SCOPE_IDENTITY()
                ", new { dtOffset, dt, id = "UTC", offset = "Z" });
                return dbNow;
            }
        }

        [HttpPost, Route("setDate")]
        public DateTimeTest SetDate([FromBody]DateTimeWithTzInfo dateTimeWithTzInfo)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dbdateTime = connection.QuerySingle<DateTimeTest>(@"
                    INSERT INTO DateTimeTesting ([Timestamp], NullableTimestamp, TimeZoneId, TimeZoneOffset) VALUES (@dtOffset, @dt, @id, @offset)
                    SELECT * FROM DateTimeTesting WHERE Id = SCOPE_IDENTITY()
                ", new
                {
                    dtOffset = dateTimeWithTzInfo.Timestamp,
                    dt = dateTimeWithTzInfo.Timestamp.UtcDateTime,
                    id = dateTimeWithTzInfo.TimeZoneId,
                    offset = dateTimeWithTzInfo.TimeZoneOffset
                });
                return dbdateTime;
            }
        }

        [HttpGet, Route("getDates")]
        public IEnumerable<DateTimeTest> GetDates()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dbdateTimes = connection.Query<DateTimeTest>(@"
                    SELECT * FROM DateTimeTesting ORDER BY Id
                ").ToList();
                return dbdateTimes;
            }
        }
    }
}
