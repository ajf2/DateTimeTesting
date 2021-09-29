using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DateTimeTesting
{
    // This class has to be registered to be functional. Do so with:
    /*
            SqlMapper.RemoveTypeMap(typeof(DateTime));
            SqlMapper.AddTypeHandler(new DateTimeHandler());
     */
    // Note that without the call to SqlMapper.RemoveTypeMap, the SetValue method will never be called.
    public class UtcDateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            // This prevents Local DateTimes being written to the database.
            if (value.Kind == DateTimeKind.Local) throw new ArgumentException("DateTimes must be written to the database as UTC. DateTimeKind.Local is not supported.");
            // Unspecified DateTimes are set as UTC before being written to the database, which is probably useless since the offset will be lost anyway.
            parameter.Value = value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value, DateTimeKind.Utc) : value;
        }

        public override DateTime Parse(object value)
        {
            // DateTime values are stored in the database as UTC but without a UTC offset.
            // This results in them being retrieved from the database with Unspecified kind.
            // This TypeHandler simply sets the DateTime kind to UTC as soon as it comes out of the database.
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
        }
    }

    // This isn't needed as it'll use the DateTimeHandler if the date has a value
    public class NullableUtcDateTimeHandler : SqlMapper.TypeHandler<DateTime?>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime? value)
        {
            // This prevents Local DateTimes being written to the database.
            if (value?.Kind == DateTimeKind.Local) throw new ArgumentException("DateTimes must be written to the database as UTC. DateTimeKind.Local is not supported.");
            // Unspecified DateTimes are set as UTC before being written to the database, which is probably useless since the offset will be lost anyway.
            parameter.Value = value?.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : value;
        }

        public override DateTime? Parse(object value)
        {
            // DateTime values are stored in the database as UTC but without a UTC offset.
            // This results in them being retrieved from the database with Unspecified kind.
            // This TypeHandler simply sets the DateTime kind to UTC as soon as it comes out of the database.
            if (value == null) return null;
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
        }
    }
}
