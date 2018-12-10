using System;
using System.ComponentModel;
using System.Globalization;
using TimeGap.Converters;
using TimeGap.Exceptions;
using TimeGap.Tests.TestUtilities;
using Xunit;

namespace TimeGap.Tests
{
    public class DuodecimDateTests
    {
        private const string ExpectedInvalidDuodecimDateExceptionMessage =
            "The combination of Year '{0}' and Month '{1}' does not represent a valid DuodecimDate.";

        private static readonly string ExpectedYearOrMonthIsNegativeExceptionMessage =
            $"The value specified cannot be a negative number.{Environment.NewLine}Parameter name: {{0}}";

        [Fact]
        public void DuodecimDate_WhenConstructedWithArguments_PropertiesSet()
        {
            // Arrange
            const int expectedYear = 2018;
            const int expectedMonth = 10;

            // Act
            var date = new DuodecimDate(expectedYear, expectedMonth);

            // Assert
            Assert.Equal(expectedYear, date.Year);
            Assert.Equal(expectedMonth, date.Month);
        }

        [Fact]
        public void DuodecimDate_WhenConstructedWithInvalidYear0_ThrowInvalidDuodecimDateException()
        {
            // Arrange
            const int invalidYear = 0;
            const int someValidMonth = 1;

            // Act
            var exception = Record.Exception(() => new DuodecimDate(invalidYear, someValidMonth));

            // Assert
            exception.Verify<InvalidDuodecimDateException>(string.Format(ExpectedInvalidDuodecimDateExceptionMessage,
                invalidYear, someValidMonth));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void DuodecimDate_WhenConstructedWithInvalidMonth_ThrowInvalidDuodecimDateException(int invalidMonth)
        {
            // Arrange
            const int someValidYear = 1;

            // Act
            var exception = Record.Exception(() => new DuodecimDate(someValidYear, invalidMonth));

            // Assert
            exception.Verify<InvalidDuodecimDateException>(string.Format(ExpectedInvalidDuodecimDateExceptionMessage,
                someValidYear, invalidMonth));
        }

        [Fact]
        public void DuodecimDate_WhenRequestingTypeConverter_ReturnDuodecimDateConverter()
        {
            // Arrange
            var duodecimDateType = typeof(DuodecimDate);

            // Act
            var typeConverter = TypeDescriptor.GetConverter(duodecimDateType);

            // Assert
            Assert.IsType<DuodecimDateConverter>(typeConverter);
        }

        [Fact]
        public void AddYears_WhenAddingYears_YearsAdded()
        {
            // Arrange
            const int startYear = 2018;
            const int yearsToAdd = 13;
            const int expectedYear = startYear + yearsToAdd;

            var date = new DuodecimDate(startYear, 6);

            // Act
            var newDate = date.AddYears(yearsToAdd);

            // Assert
            Assert.Equal(expectedYear, newDate.Year);
        }

        [Fact]
        public void AddYears_WhenAddingYears_ReturnNewInstance()
        {
            // Arrange
            var date = new DuodecimDate(2018, 6);

            // Act
            var newDate = date.AddYears(1);

            // Assert
            Assert.False(Equals(date, newDate));
        }

        [Fact]
        public void AddYears_WhenAddingNegativeYears_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int yearsToAdd = -1;

            var date = new DuodecimDate(2018, 6);

            // Act
            var exception = Record.Exception(() => date.AddYears(yearsToAdd));

            // Assert
            exception.Verify<ArgumentOutOfRangeException>(string.Format(ExpectedYearOrMonthIsNegativeExceptionMessage,
                "years"));
        }

        [Fact]
        public void SubtractYears_WhenSubtractingYears_YearsSubtracted()
        {
            // Arrange
            const int startYear = 2018;
            const int yearsToSubtract = 13;
            const int expectedYear = startYear - yearsToSubtract;

            var date = new DuodecimDate(startYear, 6);

            // Act
            var newDate = date.SubtractYears(yearsToSubtract);

            // Assert
            Assert.Equal(expectedYear, newDate.Year);
        }

        [Fact]
        public void SubtractYears_WhenSubtractingYears_ReturnNewInstance()
        {
            // Arrange
            var date = new DuodecimDate(2018, 6);

            // Act
            var newDate = date.SubtractYears(1);

            // Assert
            Assert.False(Equals(date, newDate));
        }

        [Fact]
        public void SubtractYears_WhenSubtractingYearsSoTheResultYieldsAnInvalidYear_ThrowInvalidDuodecimDateException()
        {
            // Arrange
            const int startYear = 2018;
            const int yearsToSubtract = startYear;
            const int expectedErrorMessageYear = startYear - yearsToSubtract;
            const int month = 6;

            var date = new DuodecimDate(startYear, 6);

            // Act
            var exception = Record.Exception(() => date.SubtractYears(yearsToSubtract));

            // Assert
            exception.Verify<InvalidDuodecimDateException>(string.Format(ExpectedInvalidDuodecimDateExceptionMessage,
                expectedErrorMessageYear, month));
        }

        [Fact]
        public void SubtractYears_WhenSubtractingNegativeYears_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int yearsToSubtract = -1;

            var date = new DuodecimDate(2018, 6);

            // Act
            var exception = Record.Exception(() => date.SubtractYears(yearsToSubtract));

            // Assert
            exception.Verify<ArgumentOutOfRangeException>(string.Format(ExpectedYearOrMonthIsNegativeExceptionMessage,
                "years"));
        }

        [Fact]
        public void AddMonths_WhenAddingMonths_MonthsAdded()
        {
            // Arrange
            const int startMonth = 1;
            const int monthsToAdd = 2;
            const int expectedMonth = startMonth + monthsToAdd;

            var date = new DuodecimDate(2018, startMonth);

            // Act
            var newDate = date.AddMonths(monthsToAdd);

            // Assert
            Assert.Equal(expectedMonth, newDate.Month);
        }

        [Fact]
        public void AddMonths_WhenAddingMoreMonthsThanExistInAYear_YearAndMonthsAdded()
        {
            // Arrange
            const int startyear = 2018;
            const int startMonth = 1;
            const int monthsToAdd = 27;

            const int expectedYear = 2020;
            const int expectedMonth = 4;

            var date = new DuodecimDate(startyear, startMonth);

            // Act
            var newDate = date.AddMonths(monthsToAdd);

            // Assert
            Assert.Equal(expectedMonth, newDate.Month);
            Assert.Equal(expectedYear, newDate.Year);
        }

        [Fact]
        public void AddMonths_WhenMonthAndAddedMonthExceedMonthsInYear_OneYearAddedAndExpectedNewMonthSet()
        {
            // Arrange
            const int totalMonthsInYear = 12;
            const int startyear = 2018;
            const int startMonth = 12;
            const int monthsToAdd = 1;

            const int expectedYear = startyear + 1;
            const int expectedMonth = startMonth + monthsToAdd - totalMonthsInYear;

            var date = new DuodecimDate(startyear, startMonth);

            // Act
            var newDate = date.AddMonths(monthsToAdd);

            // Assert
            Assert.Equal(expectedMonth, newDate.Month);
            Assert.Equal(expectedYear, newDate.Year);
        }

        [Fact]
        public void AddMonths_WhenAddingMonths_ReturnNewInstance()
        {
            // Arrange

            var date = new DuodecimDate(2018, 6);

            // Act
            var newDate = date.AddMonths(1);

            // Assert
            Assert.False(Equals(date, newDate));
        }

        [Fact]
        public void SubtractMonths_WhenSubtractingMonths_MonthsSubtracted()
        {
            // Arrange
            const int startMonth = 8;
            const int monthsToSubtract = 3;
            const int expectedMonth = startMonth - monthsToSubtract;

            var date = new DuodecimDate(2018, startMonth);

            // Act
            var newDate = date.SubtractMonths(monthsToSubtract);

            // Assert
            Assert.Equal(expectedMonth, newDate.Month);
        }

        [Fact]
        public void SubtractMonths_WhenSubtractingMoreMonthsThanExistInAYear_YearAndMonthsSubtracted()
        {
            // Arrange
            const int totalMonthsInYear = 12;
            const int startyear = 2018;
            const int startMonth = 10;
            const int monthsToSubtract = 29;

            const int expectedYear = 2016;
            const int expectedMonth = startMonth - (monthsToSubtract % totalMonthsInYear);

            var date = new DuodecimDate(startyear, startMonth);

            // Act
            var newDate = date.SubtractMonths(monthsToSubtract);

            // Assert
            Assert.Equal(expectedMonth, newDate.Month);
            Assert.Equal(expectedYear, newDate.Year);
        }

        [Fact]
        public void
            SubtractMonths_WhenMonthAndSunbtracteddMonthExceedMonthsInYear_OneYearSubtractedAndExpectedNewMonthSet()
        {
            // Arrange
            const int totalMonthsInYear = 12;
            const int startyear = 2018;
            const int startMonth = 1;
            const int monthsToSubtract = 1;

            const int expectedYear = startyear - 1;
            const int expectedMonth = totalMonthsInYear + startMonth - monthsToSubtract;

            var date = new DuodecimDate(startyear, startMonth);

            // Act
            var newDate = date.SubtractMonths(monthsToSubtract);

            // Assert
            Assert.Equal(expectedMonth, newDate.Month);
            Assert.Equal(expectedYear, newDate.Year);
        }

        [Fact]
        public void SubtractMonths_WhenSubtractingMonths_ReturnNewInstance()
        {
            // Arrange

            var date = new DuodecimDate(2018, 6);

            // Act
            var newDate = date.SubtractMonths(1);

            // Assert
            Assert.False(Equals(date, newDate));
        }

        [Fact]
        public void
            SubtractMonths_WhenSubtractingMonthsSoTheResultYieldsAnInvalidDate_ThrowInvalidDuodecimDateException()
        {
            // Arrange
            const int startYear = 1;
            const int startMonth = 6;
            const int monthsToSubtract = startMonth + 1;
            const int expectedErrorMessageYear = 0;
            const int expectedErrorMessageMonth = 11;

            var date = new DuodecimDate(startYear, startMonth);

            // Act
            var exception = Record.Exception(() => date.SubtractMonths(monthsToSubtract));

            // Assert
            exception.Verify<InvalidDuodecimDateException>(string.Format(ExpectedInvalidDuodecimDateExceptionMessage,
                expectedErrorMessageYear, expectedErrorMessageMonth));
        }

        [Fact]
        public void GreaterThanOperator_WhenDate1YearGreaterThanDate1Year_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2017, 6);

            // Act
            var comparisonResult = date1 > date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void GreaterThanOperator_WhenSameYearAndDate1MonthGreaterThanDate2Month_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 5);

            // Act
            var comparisonResult = date1 > date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void GreaterThanOperator_WhenSameYearAndDate1MonthLessThanDate2Month_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 5);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 > date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void GreaterThanOperator_WhenSameYearAndSameMonth_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 > date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void GreaterThanOperator_WhenDate1YearLessThanDate1Year_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2017, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 > date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void LessThanOperator_WhenDate1YearLessThanDate1Year_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2017, 6);
            var date2 = new DuodecimDate(2018, 6);

            var eq = date1.Equals(date2);

            // Act
            var comparisonResult = date1 < date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void LessThanOperator_WhenSameYearAndDate1MonthLessThanDate2Month_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 5);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 < date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void LessThanOperator_WhenSameYearAndDate1MonthGreaterThanDate2Month_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 5);

            // Act
            var comparisonResult = date1 < date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void LessThanOperator_WhenSameYearAndSameMonth_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 < date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void LessThanOperator_WhenDate1YearGreaterThanDate1Year_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2017, 6);

            // Act
            var comparisonResult = date1 < date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void GreaterThanOrEqualOperator_WhenDate1YearGreaterThanDate1Year_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2017, 6);

            // Act
            var comparisonResult = date1 >= date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void GreaterThanOrEqualOperator_WhenSameYearAndDate1MonthGreaterThanDate2Month_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 5);
            // Act
            var comparisonResult = date1 >= date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void GreaterThanOrEqualOperator_WhenSameYearAndSameMonth_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 >= date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void GreaterThanOrEqualOperator_WhenSameYearAndDate1MonthLessThanDate2Month_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 5);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 >= date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void GreaterThanOrEqualOperator_WhenDate1YearLessThanDate1Year_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2017, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 >= date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void LessThanOrEqual_WhenDate1YearLessThanDate1Year_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2017, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 <= date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void LessThanOrEqual_WhenSameYearAndDate1MonthLessThanDate2Month_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 5);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 <= date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void LessThanOrEqual_WhenSameYearAndSameMonth_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 <= date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void LessThanOrEqual_WhenSameYearAndDate1MonthGreaterThanDate2Month_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 5);

            // Act
            var comparisonResult = date1 <= date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void LessThanOrEqualOperator_WhenDate1YearGreaterThanDate1Year_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2017, 6);

            // Act
            var comparisonResult = date1 <= date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void EqualOperator_WhenSameYearAndSameMonth_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 == date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void EqualOperator_WhenDifferentYearAndSameMonth_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2019, 6);

            // Act
            var comparisonResult = date1 == date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void EqualOperator_WhenSameYearAndDifferentMonth_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 7);

            // Act
            var comparisonResult = date1 == date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void NotEqualOperator_WhenDifferentYearAndSameMonth_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2019, 6);

            // Act
            var comparisonResult = date1 != date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void EqualOperator_WhenSameYearAndDifferentMonth_ReturnTrue()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 7);

            // Act
            var comparisonResult = date1 != date2;

            // Assert
            Assert.True(comparisonResult);
        }

        [Fact]
        public void EqualOperator_WhenSameYearAndSameMonth_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var comparisonResult = date1 != date2;

            // Assert
            Assert.False(comparisonResult);
        }

        [Fact]
        public void Compare_WhenBothPointToSameReference_Return0()
        {
            // Arrange
            const int expectedCompareResult = 0;
            var date = new DuodecimDate(2018, 6);
            var otherDate = date;

            // Act
            var actualCompareResult = date.CompareTo(otherDate);

            // Assert
            Assert.Equal(expectedCompareResult, actualCompareResult);
        }

        [Fact]
        public void Compare_WhenYearGreaterThanOtherYear_Return1()
        {
            // Arrange
            const int expectedCompareResult = 1;
            var date = new DuodecimDate(2018, 1);
            var otherDate = new DuodecimDate(2017, 12);

            // Act
            var actualCompareResult = date.CompareTo(otherDate);

            // Assert
            Assert.Equal(expectedCompareResult, actualCompareResult);
        }

        [Fact]
        public void Compare_WhenYearLessThanOtherYear_ReturnMinus1()
        {
            // Arrange
            const int expectedCompareResult = -1;
            var date = new DuodecimDate(2017, 12);
            var otherDate = new DuodecimDate(2018, 1);

            // Act
            var actualCompareResult = date.CompareTo(otherDate);

            // Assert
            Assert.Equal(expectedCompareResult, actualCompareResult);
        }

        [Fact]
        public void Compare_WhenSameYearsAndMonths_Return0()
        {
            // Arrange
            const int expectedCompareResult = 0;
            var date = new DuodecimDate(2018, 1);
            var otherDate = new DuodecimDate(2018, 1);

            // Act
            var actualCompareResult = date.CompareTo(otherDate);

            // Assert
            Assert.Equal(expectedCompareResult, actualCompareResult);
        }

        [Fact]
        public void Compare_WhenSameYearsAndMonthGreaterThanOtherMonth_Return1()
        {
            // Arrange
            const int expectedCompareResult = 1;
            var date = new DuodecimDate(2018, 2);
            var otherDate = new DuodecimDate(2018, 1);

            // Act
            var actualCompareResult = date.CompareTo(otherDate);

            // Assert
            Assert.Equal(expectedCompareResult, actualCompareResult);
        }

        [Fact]
        public void Compare_WhenSameYearsAndMonthLessThanOtherMonth_ReturnMinus1()
        {
            // Arrange
            const int expectedCompareResult = -1;
            var date = new DuodecimDate(2018, 1);
            var otherDate = new DuodecimDate(2018, 2);

            // Act
            var actualCompareResult = date.CompareTo(otherDate);

            // Assert
            Assert.Equal(expectedCompareResult, actualCompareResult);
        }

        [Fact]
        public void Equals_WhenDuodecimDateAndYearMonthSame_ReturnTrue()
        {
            // Arrange
            const int year = 2018;
            const int month = 11;

            var date1 = new DuodecimDate(year, month);
            var date2 = new DuodecimDate(year, month);

            // Act
            var areEqual = date1.Equals(date2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_WhenDuodecimDateAndDifferentYear_ReturnFalse()
        {
            // Arrange
            const int year1 = 2018;
            const int year2 = 2019;
            const int month = 11;

            var date1 = new DuodecimDate(year1, month);
            var date2 = new DuodecimDate(year2, month);

            // Act
            var areEqual = date1.Equals(date2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_WhenDuodecimDateAndDifferentMonth_ReturnFalse()
        {
            // Arrange
            const int year = 2018;
            const int month1 = 11;
            const int month2 = 12;

            var date1 = new DuodecimDate(year, month1);
            var date2 = new DuodecimDate(year, month2);

            // Act
            var areEqual = date1.Equals(date2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_WhenDuodecimDateObjectAndYearMonthSame_ReturnTrue()
        {
            // Arrange
            const int year = 2018;
            const int month = 11;

            var date1 = new DuodecimDate(year, month);
            var date2 = new DuodecimDate(year, month);

            // Act
            var areEqual = date1.Equals((object) date2);

            // Assert
            Assert.True(areEqual);
        }

        [Fact]
        public void Equals_WhenDuodecimDateObjectAndDifferentYear_ReturnFalse()
        {
            // Arrange
            const int year1 = 2018;
            const int year2 = 2019;
            const int month = 11;

            var date1 = new DuodecimDate(year1, month);
            var date2 = new DuodecimDate(year2, month);

            // Act
            var areEqual = date1.Equals((object) date2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_WhenDuodecimDateObjectAndDifferentMonth_ReturnFalse()
        {
            // Arrange
            const int year = 2018;
            const int month1 = 11;
            const int month2 = 12;

            var date1 = new DuodecimDate(year, month1);
            var date2 = new DuodecimDate(year, month2);

            // Act
            var areEqual = date1.Equals((object) date2);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_WhenSomeRandomObjectThatIsNotDuodecimDate_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var randomObject = new SomeRandomType();

            // Act
            // ReSharper disable once SuspiciousTypeConversion.Global
            var areEqual = date1.Equals(randomObject);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void Equals_WhenObjectAndObjectIsNull_ReturnFalse()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            object nullObject = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            var areEqual = date1.Equals(nullObject);

            // Assert
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCode_WhenTwoObjectHasSameProperties_ReturnSameHashCode()
        {
            // Arrange
            var date1 = new DuodecimDate(2018, 6);
            var date2 = new DuodecimDate(2018, 6);

            // Act
            var hash1 = date1.GetHashCode();
            var hash2 = date2.GetHashCode();

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_WhenTwoObjectHasDifferentYear_ReturnDifferentHashCode()
        {
            // Arrange
            const int year1 = 2018;
            const int year2 = 2019;
            const int month = 2;
            var date1 = new DuodecimDate(year1, month);
            var date2 = new DuodecimDate(year2, month);

            // Act
            var hash1 = date1.GetHashCode();
            var hash2 = date2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void GetHashCode_WhenTwoObjectHasDifferentMonth_ReturnDifferentHashCode()
        {
            // Arrange
            const int year = 2018;
            const int month1 = 11;
            const int month2 = 12;
            var date1 = new DuodecimDate(year, month1);
            var date2 = new DuodecimDate(year, month2);

            // Act
            var hash1 = date1.GetHashCode();
            var hash2 = date2.GetHashCode();

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void Now_WhenRequested_ReturnDateTimeTodayRepresentedAsDuodecimDate()
        {
            // Arrange
            var today = DateTime.Today;
            var expectedYear = today.Year;
            var expectedMonth = today.Month;

            // Act
            var duodecimDateNow = DuodecimDate.Now;

            // Assert
            Assert.Equal(expectedYear, duodecimDateNow.Year);
            Assert.Equal(expectedMonth, duodecimDateNow.Month);
        }

        [Fact]
        public void UtcNow_WhenRequested_ReturnDateTimeUtcNowRepresentedAsDuodecimDate()
        {
            // Arrange
            var utcNow = DateTime.UtcNow;
            var expectedYear = utcNow.Year;
            var expectedMonth = utcNow.Month;

            // Act
            var duodecimDateUtcNow = DuodecimDate.UtcNow;

            // Assert
            Assert.Equal(expectedYear, duodecimDateUtcNow.Year);
            Assert.Equal(expectedMonth, duodecimDateUtcNow.Month);
        }

        [Fact]
        public void MinValue_WhenRequested_ReturnDuodecimDateWithYear1AndMonth1()
        {
            // Arrange
            const int expectedYear = 1;
            const int expectedMonth = 1;

            // Act
            var actualDuodecimDate = DuodecimDate.MinValue;

            // Assert
            Assert.Equal(expectedYear, actualDuodecimDate.Year);
            Assert.Equal(expectedMonth, actualDuodecimDate.Month);
        }

        [Fact]
        public void MaxValue_WhenRequested_ReturnDuodecimDateWithYear9999AndMonth12()
        {
            // Arrange
            const int expectedYear = 9999;
            const int expectedMonth = 12;

            // Act
            var actualDuodecimDate = DuodecimDate.MaxValue;

            // Assert
            Assert.Equal(expectedYear, actualDuodecimDate.Year);
            Assert.Equal(expectedMonth, actualDuodecimDate.Month);
        }

        [Theory]
        [InlineData(2018, 6, 5)]
        [InlineData(2018, 12, 31)]
        [InlineData(2018, 1, 1)]
        public void
            Parse_WhenParsingCommonDateTimeFormatsAndParsingWithoutCulture_AssumeCurrentCultureAndReturnExpectedDuodecimDate(
                int year, int month, int day)
        {
            // Arrange
            var dateTime = new DateTime(year, month, day);
            var dateTimeAsString = dateTime.ToString(DateTimeFormatInfo.CurrentInfo);
            var expectedDuodecimDate = new DuodecimDate(year, month);

            // Act
            var parsedDuodecimDate = DuodecimDate.Parse(dateTimeAsString);

            // Assert
            Assert.Equal(expectedDuodecimDate, parsedDuodecimDate);
        }

        [Theory]
        [InlineData("20.11.2018", 2018, 11)]
        [InlineData("05.06.2018", 2018, 06)]
        [InlineData("23.9.2018", 2018, 9)]
        [InlineData("23.09.2018", 2018, 9)]
        [InlineData("20-10-2018", 2018, 10)]
        [InlineData("11-2018", 2018, 11)]
        [InlineData("4.2018", 2018, 4)]
        [InlineData("04.2018", 2018, 4)]
        [InlineData("29/08/2018", 2018, 8)]
        [InlineData("2018/08/18", 2018, 8)]
        [InlineData("2018-08-18", 2018, 8)]
        [InlineData("27-12.2017 14:59:32", 2017, 12)]
        public void Parse_WhenParsingDateAndTimeWithoutTimeOrTimeOffsetWithNorwegianCulture_ReturnExpectedDuodecimDate(
            string dateString, int expectedYear, int expectedMonth)
        {
            // Arrange
            var expectedDuodecimDate = new DuodecimDate(expectedYear, expectedMonth);

            // Act
            var parsedDuodecimDate = DuodecimDate.Parse(dateString, new CultureInfo("nb-NO"));

            // Assert
            Assert.Equal(expectedDuodecimDate, parsedDuodecimDate);
        }

        [Theory]
        [InlineData("11.20.2018")]
        [InlineData("9.23.2018")]
        [InlineData("09.23.2018")]
        [InlineData("10-20-2018")]
        [InlineData("08/29/2018")]
        [InlineData("12-27.2017 14:59:32")]
        public void Parse_WhenParsingInvalidCultureDateFormat_ThrowFormatException(string dateString)
        {
            // Arrange
            var expectedExceptionMessage = $"Cannot parse value '{dateString}' to a valid {nameof(DuodecimDate)}.";

            // Act
            var exception = Record.Exception(() => DuodecimDate.Parse(dateString, new CultureInfo("nb-NO")));

            // Assert
            exception.Verify<FormatException>(expectedExceptionMessage);
        }

        [Theory]
        [InlineData("11.20.2018", 2018, 11)]
        [InlineData("05.06.2018", 2018, 5)]
        [InlineData("9.23.2018", 2018, 9)]
        [InlineData("09.23.2018", 2018, 9)]
        [InlineData("10-20-2018", 2018, 10)]
        [InlineData("11-2018", 2018, 11)]
        [InlineData("4.2018", 2018, 4)]
        [InlineData("04.2018", 2018, 4)]
        [InlineData("08/29/2018", 2018, 8)]
        [InlineData("2018/08/18", 2018, 8)]
        [InlineData("2018-08-18", 2018, 8)]
        [InlineData("12-27.2017 14:59:32", 2017, 12)]
        public void Parse_WhenParsingDateAndTimeWithoutTimeOrTimeOffsetWithUSCulture_ReturnExpectedDuodecimDate(
            string dateString, int expectedYear, int expectedMonth)
        {
            // Arrange
            var expectedDuodecimDate = new DuodecimDate(expectedYear, expectedMonth);

            // Act
            var parsedDuodecimDate = DuodecimDate.Parse(dateString, new CultureInfo("en-US"));

            // Assert
            Assert.Equal(expectedDuodecimDate, parsedDuodecimDate);
        }

        [Theory]
        [InlineData("2009-07-01T00:01:01+01:00", 2009, 06)]
        [InlineData("2009-07-01T00:01:01+00:00", 2009, 07)]
        [InlineData("2009-07-01T00:01:01-00:00", 2009, 07)]
        [InlineData("2009-07-01T00:01:01-01:00", 2009, 07)]
        public void Parse_WhenParsingDateAndTimeWithOffsetAndNorwegianCulture_ReturnExpectedDuodecimDate(
            string dateString, int expectedYear, int expectedMonth)
        {
            // Arrange
            var expectedDuodecimDate = new DuodecimDate(expectedYear, expectedMonth);

            // Act
            var parsedDuodecimDate = DuodecimDate.Parse(dateString, new CultureInfo("nb-NO"));

            // Assert
            Assert.Equal(expectedDuodecimDate, parsedDuodecimDate);
        }

        [Theory]
        [InlineData("2009-07-01T00:01:01+01:00", 2009, 06)]
        [InlineData("2009-07-01T00:01:01+00:00", 2009, 07)]
        [InlineData("2009-07-01T00:01:01-00:00", 2009, 07)]
        [InlineData("2009-07-01T00:01:01-01:00", 2009, 07)]
        public void Parse_WhenParsingDateAndTimeWithOffsetWithUsCulture_ReturnExpectedDuodecimDate(
            string dateString, int expectedYear, int expectedMonth)
        {
            // Arrange
            var expectedDuodecimDate = new DuodecimDate(expectedYear, expectedMonth);

            // Act
            var parsedDuodecimDate = DuodecimDate.Parse(dateString, new CultureInfo("en-US"));

            // Assert
            Assert.Equal(expectedDuodecimDate, parsedDuodecimDate);
        }

        [Theory]
        [InlineData("2009-07-01T00:01:01+01:00", 2009, 06)]
        [InlineData("2009-07-01T00:01:01+00:00", 2009, 07)]
        [InlineData("2009-07-01T00:01:01-00:00", 2009, 07)]
        [InlineData("2009-07-01T00:01:01-01:00", 2009, 07)]
        public void
            Parse_WhenParsingDateAndTimeWithOffsetAndNorwegianCultureAndDateTimeStyle_ReturnExpectedDuodecimDate(
                string dateString, int expectedYear, int expectedMonth)
        {
            // Arrange
            var expectedDuodecimDate = new DuodecimDate(expectedYear, expectedMonth);

            // Act
            var parsedDuodecimDate = DuodecimDate.Parse(dateString, new CultureInfo("nb-NO"), DateTimeStyles.None);

            // Assert
            Assert.Equal(expectedDuodecimDate, parsedDuodecimDate);
        }

        [Theory]
        [InlineData(2018, 11, 1)]
        [InlineData(2018, 1, 1)]
        [InlineData(2018, 11, 30)]
        public void
            Parse_WhenParsingDateAndTimeWithoutTimeOrTimeOffsetWithCultureAndDateTimeStyle_ReturnExpectedDuodecimDate(
                int year, int month, int day)
        {
            // Arrange
            var dateTime = new DateTime(year, month, day);
            var dateTimeAsString = dateTime.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
            var utcDateTime = dateTime.ToUniversalTime();
            var expectedDuodecimDate = new DuodecimDate(utcDateTime.Year, utcDateTime.Month);

            // Act
            var parsedDuodecimDate =
                DuodecimDate.Parse(dateTimeAsString, new CultureInfo("en-US"), DateTimeStyles.AssumeLocal);

            // Assert
            Assert.Equal(expectedDuodecimDate, parsedDuodecimDate);
        }

        [Fact]
        public void ToString_WhenValidDuodecimDate_ReturnExpectedValueWithPaddedZeros()
        {
            // Arrange
            const int year = 997;
            const int month = 7;
            var expectedDuodecimDateString = $"{year:0000}-{month:00}";

            var duodecimDate = new DuodecimDate(year, month);

            // Act
            var actualDuodecimDateString = duodecimDate.ToString();

            // Assert
            Assert.Equal(expectedDuodecimDateString, actualDuodecimDateString);
        }

        private sealed class SomeRandomType
        {
        }
    }
}