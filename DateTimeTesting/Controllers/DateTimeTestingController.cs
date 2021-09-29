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
            var now = DateTime.UtcNow;
            using (var connection = new SqlConnection(connectionString))
            {
                var dbNow = connection.QuerySingle<DateTimeTest>(@"
                    INSERT INTO DateTimeTesting ([Timestamp], SecondDateTime) VALUES (@now, @now)
                    SELECT TOP 1 Id, [Timestamp], SecondDateTime FROM DateTimeTesting
                ", new {now});
                return dbNow;
            }
        }

        [HttpGet, Route("setDate")]
        public DateTimeTest SetDate(DateTime dateTime)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dbdateTime = connection.QuerySingle<DateTimeTest>(@"
                    INSERT INTO DateTimeTesting ([Timestamp]) VALUES (@dateTime)
                    SELECT TOP 1 Id, [Timestamp], SecondDateTime FROM DateTimeTesting
                ", new { dateTime });
                return dbdateTime;
            }
        }

        [HttpGet, Route("getDate")]
        public DateTimeTest GetDate()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dbdateTime = connection.QuerySingle<DateTimeTest>(@"
                    SELECT TOP 1 Id, [Timestamp], SecondDateTime FROM DateTimeTesting
                ");
                return dbdateTime;
            }
        }

        [HttpGet, Route("getDates")]
        public IEnumerable<DateTimeTest> GetDates()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var dbdateTimes = connection.Query<DateTimeTest>(@"
                    SELECT Id, [Timestamp], SecondDateTime FROM DateTimeTesting ORDER BY Id
                ");
                return dbdateTimes;
            }
        }
    }
}
