using System;
using System.Globalization;
using System.Reflection;
using TimeGap.Converters;
using TimeGap.Tests.TestUtilities;
using Xunit;

namespace TimeGap.Tests.Converters
{
    public class DuodecimDateConverterTests
    {
        [Fact]
        public void CanConvertFrom_WhenSourceString_ReturnTrue()
        {
            // Arrange
            var sourceType = typeof(string);

            var converter = new DuodecimDateConverter();

            // Act
            var canConvert = converter.CanConvertFrom(null, sourceType);

            // Assert
            Assert.True(canConvert);
        }

        [Fact]
        public void CanConvertFrom_WhenSourceTypeNotString_ReturnFalse()
        {
            // Arrange
            var sourceType = typeof(SomeTypeThatCannotConvertToDuodecimDate);
            var converter = new DuodecimDateConverter();

            // Act
            var canConvert = converter.CanConvertFrom(null, sourceType);

            // Assert
            Assert.False(canConvert);
        }

        [Fact]
        public void CanConvertFrom_WhenSourceTypeNotInstanceDescriptor_ReturnFalse()
        {
            // Arrange
            var destinationType = typeof(SomeTypeThatCannotConvertToDuodecimDate);
            var converter = new DuodecimDateConverter();

            // Act
            var canConvert = converter.CanConvertFrom(null, destinationType);

            // Assert
            Assert.False(canConvert);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ConvertFrom_WhenConvertingNullOrEmptyString_ReturnDuodecimDateMinValue(string convertValue)
        {
            // Arrange
            var expectedCovertedValue = DuodecimDate.MinValue;
            var converter = new DuodecimDateConverter();

            // Act
            var convertedValue = converter.ConvertFrom(null, CultureInfo.CurrentCulture, convertValue);

            // Assert
            Assert.Equal(expectedCovertedValue, convertedValue);
        }

        [Fact]
        public void ConvertFrom_WhenConvertingWithCultureInfo_ReturnExpectedDuodecimDate()
        {
            // Arrange
            const int year = 2018;
            const int month = 2;
            var convertValue = $"20.{month}.{year}";

            var expectedCovertedValue = new DuodecimDate(year, month);
            var converter = new DuodecimDateConverter();

            // Act
            var convertedValue = converter.ConvertFrom(null, new CultureInfo("nb-NO"), convertValue);

            // Assert
            Assert.Equal(expectedCovertedValue, convertedValue);
        }

        [Fact]
        public void ConvertFrom_WhenConvertingWithoutCultureInfo_ReturnExpectedDuodecimDate()
        {
            // Arrange
            const int year = 2018;
            const int month = 2;
            var dateTime = new DateTime(year, month, 22);
            var convertValue = dateTime.ToString(CultureInfo.CurrentCulture);
            CultureInfo converterCulture = null;

            var expectedCovertedValue = new DuodecimDate(year, month);
            var converter = new DuodecimDateConverter();

            // Act
            var convertedValue = converter.ConvertFrom(null, converterCulture, convertValue);

            // Assert
            Assert.Equal(expectedCovertedValue, convertedValue);
        }

        [Fact]
        public void ConvertFrom_WhenConvertingFromInvalidType_ThrowNotSupportedException()
        {
            // Arrange 
            var expectedExceptionMessage =
                $"{nameof(DuodecimDateConverter)} cannot convert from {typeof(DuodecimDateConverterTests).FullName}+" +
                $"{nameof(SomeTypeThatCannotConvertToDuodecimDate)}.";
            var invalidConvertFromValue = new SomeTypeThatCannotConvertToDuodecimDate();
            var converter = new DuodecimDateConverter();

            // Act
            var exception = Record.Exception(() => converter.ConvertFrom(null, null, invalidConvertFromValue));

            // Assert
            exception.Verify<NotSupportedException>(expectedExceptionMessage);
        }

        [Fact]
        public void ConvertTo_WhenSourceTypeStringAndValueDuodecimDate_ReturnExpectedValue()
        {
            // Arrange
            var destinationType = typeof(string);
            var dateToConvert = new DuodecimDate(2018, 11);
            var exectedString = dateToConvert.ToString();

            var converter = new DuodecimDateConverter();

            // Act
            var actualString = converter.ConvertTo(null, null, dateToConvert, destinationType);

            // Assert
            Assert.Equal(exectedString, actualString);
        }

        [Fact]
        public void ConvertTo_WhenSourceTypeStringAndValueNotDuodecimDate_ReturnTypeToString()
        {
            // Arrange
            var destinationType = typeof(string);
            var dateToConvert = new SomeTypeThatCannotConvertToDuodecimDate();
            var exectedString = dateToConvert.ToString();

            var converter = new DuodecimDateConverter();

            // Act
            var actualString = converter.ConvertTo(null, null, dateToConvert, destinationType);

            // Assert
            Assert.Equal(exectedString, actualString);
        }

        private class SomeTypeThatCannotConvertToDuodecimDate
        {
        }
    }
}