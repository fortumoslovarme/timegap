using System;
using NodaTime;
using NodaTime.Extensions;

namespace TimeGap.Extensions
{
    /// <summary>
    /// Helper methods for commonly used date and time operations.
    /// Handles conversion to and from different timezones.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get the Tzdb DateTimeZone representation for Norway (Europe/Oslo).
        /// </summary>
        public static readonly DateTimeZone NorwayTimeZone = DateTimeZoneProviders.Tzdb["Europe/Oslo"];
        /// <summary>
        /// Get the Tzdb DateTimeZone representation for Gmt/Utc + 1.
        /// </summary>
        public static readonly DateTimeZone GmtPlusOneTimeZone = DateTimeZoneProviders.Tzdb["Etc/GMT-1"];
        /// <summary>
        /// Get the UTC DateTimeZone representation.
        /// </summary>
        public static readonly DateTimeZone UtcTimeZone = DateTimeZone.Utc;

        /// <summary>
        /// Convert UTC datetime to the specified time zone.
        /// </summary>
        /// <param name="utcTimeToConvert">The UTC date and time to convert.</param>
        /// <param name="timeZoneForOutput">The target time zone.</param>
        /// <returns>Converted DateTime representation for the time zone specified.</returns>
        public static DateTime ToTimeZone(this DateTime utcTimeToConvert, DateTimeZone timeZoneForOutput)
        {
            if (utcTimeToConvert.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("Expected a UTC DateTimeKind.", nameof(utcTimeToConvert));
            }

            return utcTimeToConvert.ToInstant().InZone(timeZoneForOutput).LocalDateTime
                .ToDateTimeUnspecified();
        }

        /// <summary>
        /// Convert UTC date and time to the corresponding date and time local to the Norwegian time zone.
        /// </summary>
        /// <param name="utcTimeToConvert">The UTC date and time to convert.</param>
        /// <returns>Converted DateTime representation for the Norwegian time zone.</returns>
        public static DateTime ToNorwegianTime(this DateTime utcTimeToConvert)
        {
            return ToTimeZone(utcTimeToConvert, NorwayTimeZone);
        }

        /// <summary>
        /// Convert any date and time, from the specified time zone, to the equivalent date and time in UTC.
        /// </summary>
        /// <param name="timeToConvert">The date and time to convert.</param>
        /// <param name="timeZoneForInput">The time zone from which to convert to UTC from.</param>
        /// <returns>Converted DateTime representation for the UTC time zone.</returns>
        public static DateTime ToUtc(this DateTime timeToConvert, DateTimeZone timeZoneForInput)
        {
            return CreateInstant(timeToConvert, timeZoneForInput).InUtc().ToDateTimeUtc();
        }

        /// <summary>
        /// Set any local or unspecified DateTime to be of kind UTC.
        /// </summary>
        /// <param name="timeToConvert">The DateTime to specify DateTime kind for.</param>
        /// <returns>Input DateTime with a specified DateTime.Kind of UTC.</returns>
        public static DateTime SetUtcKind(this DateTime timeToConvert)
        {
            const DateTimeKind utcKind = DateTimeKind.Utc;
            return timeToConvert.Kind != utcKind ? DateTime.SpecifyKind(timeToConvert, utcKind) : timeToConvert;
        }

        /// <summary>
        /// Convert any date and time to the corresponding DuodecimDate.
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>DuodecimDate equivalent of the specified date and time.</returns>
        public static DuodecimDate ToDuodecimDate(this DateTime dateTime) => new DuodecimDate(dateTime.Year, dateTime.Month);

        private static Instant CreateInstant(DateTime timeToConvert, DateTimeZone timeZoneForInput)
        {
            return LocalDateTime.FromDateTime(timeToConvert).InZoneLeniently(timeZoneForInput).ToInstant();
        }
    }
}