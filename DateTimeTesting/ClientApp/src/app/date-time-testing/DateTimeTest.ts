export interface DateTimeTest {
  id: number;
  timestamp: Date;
  nullableTimestamp?: Date;
  fixedNullableTimestamp?: Date;
  timeZoneId: string;
  timeZoneOffset: string;
}

export interface DateTimeWithTzInfo {
  timestamp: Date;
  nullableTimestamp?: Date;
  timeZoneId: string;
  timeZoneOffset: string;
}
