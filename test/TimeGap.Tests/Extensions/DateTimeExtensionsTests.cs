using System;
using System.Globalization;
using TimeGap.Extensions;
using Xunit;

namespace TimeGap.Tests.Extensions
{
    public class DateTimeExtensionsTests
    {
        private const DateTimeKind UtcDateTimeKind = DateTimeKind.Utc;
        private const DateTimeKind UnspecifiedDateTimeKind = DateTimeKind.Unspecified;
        private const DateTimeKind LocalDateTimeKind = DateTimeKind.Local;

        [Theory]
        [InlineData("2017-01-01 23:02:05", "2017-01-02 00:02:05")]
        [InlineData("2017-12-31 23:01:07", "2018-01-01 00:01:07")]
        [InlineData("2015-06-30 23:11:02", "2015-07-01 01:11:02")]
        [InlineData("2015-04-25 02:04:00", "2015-04-25 04:04:00")]
        [InlineData("2017-03-25 00:15:06", "2017-03-25 01:15:06")]
        [InlineData("2012-03-25 01:05:16", "2012-03-25 03:05:16")]
        [InlineData("2012-03-25 02:11:22", "2012-03-25 04:11:22")]
        [InlineData("2018-10-28 00:03:23", "2018-10-28 02:03:23")]
        [InlineData("2018-10-28 01:22:23", "2018-10-28 02:22:23")]
        [InlineData("2018-10-28 02:00:01", "2018-10-28 03:00:01")]
        [InlineData("2018-10-28 03:00:00", "2018-10-28 04:00:00")]
        [InlineData("2018-02-13 02:14:45", "2018-02-13 03:14:45")]
        public void ToNorwegianTime_ValidUtcDateTime_ReturnsExpectedNorwegianDateTime(string timeToConvert,
            string expectedTimeOutput)
        {
            // Arrange
            var dateTimeToConvert = ParseDate(timeToConvert);
            var expectedDateTime = ParseDate(expectedTimeOutput);

            // Act
            var convertedDateTime = dateTimeToConvert.ToNorwegianTime();

            // Assert
            Assert.Equal(expectedDateTime, convertedDateTime);
        }

        [Theory]
        [InlineData(LocalDateTimeKind)]
        [InlineData(UnspecifiedDateTimeKind)]
        public void ToNorwegianTime_InvalidDateTimeKind_ThrowsArgumentException(DateTimeKind kind)
        {
            // Arrange
            var dateTimeToConvert = ParseDate("2017-01-01 23:02:05", kind);

            // Act
            var exception = Record.Exception(() => dateTimeToConvert.ToNorwegianTime());

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);

            var exceptionMessage = exception.Message;
            Assert.Contains("Expected a UTC DateTimeKind.", exceptionMessage);
            Assert.Contains("utcTimeToConvert", exceptionMessage);
        }

        [Theory]
        [InlineData("2017-01-02 00:02:05", "2017-01-01 23:02:05")]
        [InlineData("2018-01-01 00:01:07", "2017-12-31 23:01:07")]
        [InlineData("2015-07-01 01:11:02", "2015-06-30 23:11:02")]
        [InlineData("2015-04-25 04:04:00", "2015-04-25 02:04:00")]
        [InlineData("2017-03-25 01:15:06", "2017-03-25 00:15:06")]
        [InlineData("2012-03-25 03:05:16", "2012-03-25 01:05:16")]
        [InlineData("2012-03-25 04:11:22", "2012-03-25 02:11:22")]
        [InlineData("2018-10-28 02:03:23", "2018-10-28 00:03:23")]
        [InlineData("2018-10-28 02:22:23", "2018-10-28 00:22:23")]
        [InlineData("2018-10-28 03:00:01", "2018-10-28 02:00:01")]
        [InlineData("2018-10-28 04:00:00", "2018-10-28 03:00:00")]
        [InlineData("2018-02-13 03:14:45", "2018-02-13 02:14:45")]
        public void ConvertToUtc_ValidDateTimeAndTimeZoneInputIsNorwegian_ReturnsExpectedUtcDateTime(
            string timeToConvert,
            string expectedTimeOutput)
        {
            // Arrange
            var dateTimeToConvert = ParseDate(timeToConvert);
            var expectedDateTime = ParseDate(expectedTimeOutput);

            // Act
            var convertedDateTime = dateTimeToConvert.ToUtc(DateTimeExtensions.NorwayTimeZone);

            // Assert
            Assert.Equal(expectedDateTime, convertedDateTime);
        }

        [Theory]
        [InlineData("2017-01-02 00:02:05", "2017-01-01 23:02:05")]
        [InlineData("2018-01-01 00:01:07", "2017-12-31 23:01:07")]
        [InlineData("2015-07-01 00:11:02", "2015-06-30 23:11:02")]
        [InlineData("2015-04-25 03:04:00", "2015-04-25 02:04:00")]
        [InlineData("2017-03-25 01:15:06", "2017-03-25 00:15:06")]
        [InlineData("2012-03-25 02:05:16", "2012-03-25 01:05:16")]
        [InlineData("2012-03-25 03:11:22", "2012-03-25 02:11:22")]
        [InlineData("2018-10-28 01:03:23", "2018-10-28 00:03:23")]
        [InlineData("2018-10-28 01:22:23", "2018-10-28 00:22:23")]
        [InlineData("2018-10-28 03:00:01", "2018-10-28 02:00:01")]
        [InlineData("2018-10-28 04:00:00", "2018-10-28 03:00:00")]
        [InlineData("2018-02-13 03:14:45", "2018-02-13 02:14:45")]
        public void ConvertToUtc_ValidDateTimeAndTimeZoneInputIsGmtPlusOne_ReturnsExpectedUtcDateTime(
            string timeToConvert,
            string expectedTimeOutput)
        {
            // Arrange
            var dateTimeToConvert = ParseDate(timeToConvert);
            var expectedDateTime = ParseDate(expectedTimeOutput);

            // Act
            var convertedDateTime = dateTimeToConvert.ToUtc(DateTimeExtensions.GmtPlusOneTimeZone);

            // Assert
            Assert.Equal(expectedDateTime, convertedDateTime);
        }

        [Theory]
        [InlineData("2017-01-01 23:02:05", "2017-01-02 00:02:05")]
        [InlineData("2017-12-31 23:01:07", "2018-01-01 00:01:07")]
        [InlineData("2015-06-30 23:11:02", "2015-07-01 00:11:02")]
        [InlineData("2015-04-25 02:04:00", "2015-04-25 03:04:00")]
        [InlineData("2017-03-25 00:15:06", "2017-03-25 01:15:06")]
        [InlineData("2012-03-25 01:05:16", "2012-03-25 02:05:16")]
        [InlineData("2012-03-25 02:11:22", "2012-03-25 03:11:22")]
        [InlineData("2018-10-28 00:03:23", "2018-10-28 01:03:23")]
        [InlineData("2018-10-28 01:22:23", "2018-10-28 02:22:23")]
        [InlineData("2018-10-28 02:00:01", "2018-10-28 03:00:01")]
        [InlineData("2018-10-28 03:00:00", "2018-10-28 04:00:00")]
        [InlineData("2018-02-13 02:14:45", "2018-02-13 03:14:45")]
        public void ToTimeZone_ValidUtcDateTimeWithTimeZoneOutputGmtPlusOne_ReturnsExpectedNorwegianDateTime(
            string timeToConvert,
            string expectedTimeOutput)
        {
            // Arrange
            var dateTimeToConvert = ParseDate(timeToConvert);
            var expectedDateTime = ParseDate(expectedTimeOutput);

            // Act
            var convertedDateTime = dateTimeToConvert.ToTimeZone(DateTimeExtensions.GmtPlusOneTimeZone);

            // Assert
            Assert.Equal(expectedDateTime, convertedDateTime);
        }

        [Theory]
        [InlineData(LocalDateTimeKind)]
        [InlineData(UnspecifiedDateTimeKind)]
        public void ToTimeZone_InvalidDateTimeKind_ThrowsArgumentException(DateTimeKind kind)
        {
            // Arrange
            var dateTimeToConvert = ParseDate("2017-01-01 23:02:05", kind);

            // Act
            var exception = Record.Exception(() => dateTimeToConvert.ToTimeZone(DateTimeExtensions.NorwayTimeZone));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);

            var exceptionMessage = exception.Message;
            Assert.Contains("Expected a UTC DateTimeKind.", exceptionMessage);
            Assert.Contains("utcTimeToConvert", exceptionMessage);
        }

        [Theory]
        [InlineData("2017-01-01 23:02:05")]
        [InlineData("2017-12-31 23:01:07")]
        public void SetUtcKind_ValidDateTimesWithUtcKind_ReturnsExpectedDate(string timeToConvert)
        {
            // Arrange
            var dateTimeToConvert = ParseDate(timeToConvert);

            // Act
            var convertedDateTime = dateTimeToConvert.SetUtcKind();

            // Assert
            Assert.Equal(dateTimeToConvert, convertedDateTime);
            Assert.Equal(UtcDateTimeKind, convertedDateTime.Kind);
        }

        [Theory]
        [InlineData("2017-04-03 11:02:05")]
        [InlineData("2017-11-11 07:02:05")]
        public void SetUtcKind_ValidDateTimesWithLocalKind_ReturnsExpectedDate(string timeToConvert)
        {
            // Arrange
            var dateTimeToConvert = ParseDate(timeToConvert, LocalDateTimeKind);
            var expectedDate = DateTime.SpecifyKind(dateTimeToConvert, UtcDateTimeKind);

            // Act
            var convertedDateTime = dateTimeToConvert.SetUtcKind();

            // Assert
            Assert.Equal(expectedDate, convertedDateTime);
            Assert.Equal(UtcDateTimeKind, convertedDateTime.Kind);
        }

        [Theory]
        [InlineData("2015-02-01 01:02:05")]
        [InlineData("2017-01-07 05:11:24")]
        public void SetUtcKind_ValidDateTimesWithUnspecifiedKind_ReturnsExpectedDate(string timeToConvert)
        {
            // Arrange
            var dateTimeToConvert = ParseDate(timeToConvert, UnspecifiedDateTimeKind);
            var expectedDate = DateTime.SpecifyKind(dateTimeToConvert, UtcDateTimeKind);

            // Act
            var convertedDateTime = dateTimeToConvert.SetUtcKind();

            // Assert
            Assert.Equal(expectedDate, convertedDateTime);
            Assert.Equal(UtcDateTimeKind, convertedDateTime.Kind);
        }

        [Fact]
        public void ToDuodecimDate_WhenConvertingDateTime_ReturnExpectedDuodecimDate()
        {
            // Arrange
            const int expectedYear = 2018;
            const int expectedMonth = 12;

            var dateTimeToConvert = new DateTime(expectedYear, expectedMonth, 1);

            // Act
            var actualDate = dateTimeToConvert.ToDuodecimDate();

            // Assert
            Assert.Equal(expectedYear, actualDate.Year);
            Assert.Equal(expectedMonth, actualDate.Month);
        }

        private static DateTime ParseDate(string dateString, DateTimeKind kind = UtcDateTimeKind)
        {
            return DateTime.SpecifyKind(DateTime.Parse(dateString, CultureInfo.GetCultureInfo("en-US")), kind);
        }
    }
}