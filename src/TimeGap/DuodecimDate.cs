using System;
using System.ComponentModel;
using System.Globalization;
using TimeGap.Converters;
using TimeGap.Exceptions;
using TimeGap.Extensions;

namespace TimeGap
{
    /// <summary>
    /// DuodecimDate:
    /// Like a date without the day number.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(DuodecimDateConverter))]
    public struct DuodecimDate : IComparable<DuodecimDate>, IEquatable<DuodecimDate>
    {
        private const int MonthsInYear = 12;
        private const int MinimumYear = 1;
        private const int MinimumMonth = 1;
        private const int MaximumYear = 9999;

        /// <summary>
        /// Create a DuodecimDate with the specified year and month.
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month</param>
        /// <exception cref="InvalidDuodecimDateException">Thrown when the combination of year and month
        /// is invalid.</exception>
        public DuodecimDate(int year, int month)
        {
            Year = year;
            Month = month;
            ValidateDuodecimDate();
        }

        /// <summary>
        /// The year.
        /// </summary>
        /// <exception cref="InvalidDuodecimDateException">Thrown when the combination of year and month
        /// is invalid.</exception>
        public int Year { get; }

        /// <summary>
        /// The Month.
        /// </summary>
        /// <exception cref="InvalidDuodecimDateException">Thrown when the combination of year and month
        /// is invalid.</exception>
        public int Month { get; }

        /// <summary>
        /// Add a specified number of <paramref name="years"/>.
        /// </summary>
        /// <param name="years">Number of years to add.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="years"/>
        /// parameter is negative.</exception>
        /// <returns>A DuodecimDate resulting from adding the number of years from this DuodecimDate.</returns>
        public DuodecimDate AddYears(int years)
        {
            ValidateValueNotLessThanZero(years, nameof(years));

            var newYear = Year + years;
            return new DuodecimDate(newYear, Month);
        }

        /// <summary>
        /// Subtract a specified number of <paramref name="years"/>.
        /// </summary>
        /// <param name="years">Number of years to subtract.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="years"/>
        /// parameter is negative.</exception>
        /// <returns>A DuodecimDate resulting from subtracting the number of years from this DuodecimDate.</returns>
        public DuodecimDate SubtractYears(int years)
        {
            ValidateValueNotLessThanZero(years, nameof(years));

            var newYear = Year - years;
            return new DuodecimDate(newYear, Month);
        }

        /// <summary>
        /// Add a specified number of <paramref name="months"/>.
        /// If the number of months to add exceed the number of months in a year, years is also added.
        /// </summary>
        /// <param name="months">Number of months to add.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="months"/>
        /// parameter is negative.</exception>
        /// <returns>A DuodecimDate resulting from adding the number of months from this DuodecimDate.</returns>
        public DuodecimDate AddMonths(int months)
        {
            ValidateValueNotLessThanZero(months, nameof(months));

            var newYear = Year;
            var newMonth = Month;
            var monthsToAdd = months;
            if (monthsToAdd >= MonthsInYear)
            {
                var yearsToAdd = monthsToAdd / MonthsInYear;
                newYear = Year + yearsToAdd;

                monthsToAdd = monthsToAdd % MonthsInYear;
            }

            if (Month + monthsToAdd > MonthsInYear)
            {
                newYear++;
                newMonth = Month + monthsToAdd - MonthsInYear;
            }
            else
            {
                newMonth = Month + monthsToAdd;
            }

            return new DuodecimDate(newYear, newMonth);
        }

        /// <summary>
        /// Subtract a specified number of <paramref name="months"/>.
        /// If the number of months to subtract plus the current month exceed the number of months in a year, years is also subtracted.
        /// </summary>
        /// <param name="months">Number of months to subtract.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the <paramref name="months"/>
        /// parameter is negative.</exception>
        /// <returns>A DuodecimDate resulting from subtracting the number of months from this DuodecimDate.</returns>
        public DuodecimDate SubtractMonths(int months)
        {
            ValidateValueNotLessThanZero(months, nameof(months));

            var newYear = Year;
            var newMonth = Month;
            var monthsToSubtract = months;
            if (monthsToSubtract >= MonthsInYear)
            {
                var yearsToSubtract = monthsToSubtract / MonthsInYear;
                newYear = Year - yearsToSubtract;

                monthsToSubtract = monthsToSubtract % MonthsInYear;
            }

            if (Month - monthsToSubtract <= 0)
            {
                newYear--;
                newMonth = MonthsInYear + Month - monthsToSubtract;
            }
            else
            {
                newMonth = Month - monthsToSubtract;
            }

            return new DuodecimDate(newYear, newMonth);
        }

        /// <summary>
        /// Check if <paramref name="date1"/> is a reference to a later point in time than <paramref name="date2"/>.
        /// </summary>
        /// <param name="date1">The lefthand side of the comparison.</param>
        /// <param name="date2">The righthand side of the comparison</param>
        /// <returns>A boolean comparison success status.</returns>
        public static bool operator >(DuodecimDate date1, DuodecimDate date2) => Compare(date1, date2) > 0;

        /// <summary>
        /// Check if <paramref name="date1"/> is a reference to a earlier point in time than <paramref name="date2"/>.
        /// </summary>
        /// <param name="date1">The lefthand side of the comparison.</param>
        /// <param name="date2">The righthand side of the comparison</param>
        /// <returns>A boolean comparison success status.</returns>
        public static bool operator <(DuodecimDate date1, DuodecimDate date2) => Compare(date1, date2) < 0;

        /// <summary>
        /// Check if <paramref name="date1"/> is a reference to a later than or same point in time as <paramref name="date2"/>.
        /// </summary>
        /// <param name="date1">The lefthand side of the comparison.</param>
        /// <param name="date2">The righthand side of the comparison</param>
        /// <returns>A boolean comparison success status.</returns>
        public static bool operator >=(DuodecimDate date1, DuodecimDate date2) => Compare(date1, date2) >= 0;

        /// <summary>
        /// Check if <paramref name="date1"/> is a reference to a earlier than or same point in time as <paramref name="date2"/>.
        /// </summary>
        /// <param name="date1">The lefthand side of the comparison.</param>
        /// <param name="date2">The righthand side of the comparison</param>
        /// <returns>A boolean comparison success status.</returns>
        public static bool operator <=(DuodecimDate date1, DuodecimDate date2) => Compare(date1, date2) <= 0;

        /// <summary>
        /// Check if <paramref name="date1"/> is a reference to the same point in time as <paramref name="date2"/>.
        /// </summary>
        /// <param name="date1">The lefthand side of the comparison.</param>
        /// <param name="date2">The righthand side of the comparison</param>
        /// <returns>A boolean comparison success status.</returns>
        public static bool operator ==(DuodecimDate date1, DuodecimDate date2) => Equals(date1, date2);

        /// <summary>
        /// Check if <paramref name="date1"/> is a reference to a different point in time than <paramref name="date2"/>.
        /// </summary>
        /// <param name="date1">The lefthand side of the comparison.</param>
        /// <param name="date2">The righthand side of the comparison</param>
        /// <returns>A boolean comparison success status.</returns>
        public static bool operator !=(DuodecimDate date1, DuodecimDate date2) => !(date1 == date2);

        /// <inheritdoc />
        /// <summary>
        /// Compare two DuodecimDates.
        /// </summary>
        /// <param name="other">Other date to compare against.</param>
        /// <returns>Integer value representing compare result.
        /// 0 = Same point in time as <paramref name="other"/>,
        /// 1 = Later point in time than <paramref name="other"/>,
        /// -1 = Earlier point in time than <paramref name="other"/>.</returns>
        public int CompareTo(DuodecimDate other) => Compare(this, other);

        /// <inheritdoc />
        /// <summary>
        /// Check this object against <paramref name="other" /> for value equality.
        /// </summary>
        /// <param name="other">DuodecimDate to compare equality to.</param>
        /// <returns>A boolean equality success status.</returns>
        public bool Equals(DuodecimDate other) => (Year, Month) == (other.Year, other.Month);

        /// <summary>
        /// Check this object against <paramref name="obj" /> for value equality.
        /// </summary>
        /// <param name="obj">Object to compare equality to.</param>
        /// <returns>A boolean equality success status.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is DuodecimDate date && Equals(date);
        }

        /// <summary>
        /// Compute a unique hashcode for this object.
        /// </summary>
        /// <returns>A computed integer value representing the computed hashcode.</returns>
        public override int GetHashCode() => HashCode.Combine(Year, Month);

        /// <summary>
        /// Get the current year and month as a DuodecimDate.
        /// </summary>
        /// <returns>Current DuodecimDate representation of the system local date and time.</returns>
        public static DuodecimDate Now => DateTime.Today.ToDuodecimDate();

        /// <summary>
        /// Get the current UTC year and month as a DuodecimDate.
        /// </summary>
        /// <returns>Current DuodecimDate representation of the UTC date and time.</returns>
        public static DuodecimDate UtcNow => DateTime.UtcNow.ToDuodecimDate();

        /// <summary>
        /// Get the minimum possible DuodecimDate.
        /// <returns>Minimum valid DuodecimDate.</returns>
        /// </summary>
        public static DuodecimDate MinValue => new DuodecimDate(MinimumYear, MinimumMonth);

        /// <summary>
        /// Get the maximum possible DuodecimDate.
        /// <returns>Maximum valid DuodecimDate.</returns>
        /// </summary>
        public static DuodecimDate MaxValue => new DuodecimDate(MaximumYear, MonthsInYear);

        /// <summary>
        /// Parse string to DuodecimDate using the current culture.
        /// Any valid DateTime format can be used for parsing.
        /// </summary>
        /// <param name="input">Input string to parse.</param>
        /// <returns>Parsed DuodecimDate.</returns>
        public static DuodecimDate Parse(string input)
        {
            return Parse(input, DateTimeFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Parse string to DoudecimDate using the culture specified.
        /// Any valid DateTime format can be used for parsing.
        /// </summary>
        /// <param name="input">Input string to parse.</param>
        /// <param name="provider">The IFormatProvider to be used during parsing.</param>
        /// <returns>Parsed DuodecimDate.</returns>
        public static DuodecimDate Parse(string input, IFormatProvider provider)
        {
            return Parse(input, DateTimeFormatInfo.GetInstance(provider), DateTimeStyles.AssumeUniversal);
        }

        /// <summary>
        /// Parse string to DoudecimDate using the culture and DateTimeStyle specified.
        /// Any valid DateTime format can be used for parsing.
        /// </summary>
        /// <param name="input">Input string to parse.</param>
        /// <param name="provider">The IFormatProvider to be used during parsing.</param>
        /// <param name="styles">The DateTimeStyles to be used during parsing.</param>
        /// <returns>Parsed DuodecimDate.</returns>
        public static DuodecimDate Parse(string input, IFormatProvider provider, DateTimeStyles styles)
        {
            var canParseToDateTimeOffset = DateTimeOffset.TryParse(input, provider, styles, out var parsedDateTimeOffset);

            if (canParseToDateTimeOffset)
            {
                return parsedDateTimeOffset.UtcDateTime.ToDuodecimDate();
            }

            throw new FormatException($"Cannot parse value '{input}' to a valid {nameof(DuodecimDate)}.");
        }

        /// <summary>
        /// Convert DuodecimDate to string.
        /// </summary>
        /// <returns>DuodecimDate string representation.</returns>
        public override string ToString()
        {
            return $"{Year:0000}-{Month:00}";
        }

        private static int Compare(DuodecimDate date1, DuodecimDate date2)
        {
            if (date1.Year > date2.Year)
            {
                return 1;
            }

            if (date1.Year < date2.Year)
            {
                return -1;
            }

            if (date1.Month > date2.Month)
            {
                return 1;
            }

            if (date1.Month < date2.Month)
            {
                return -1;
            }

            return 0;
        }

        private void ValidateDuodecimDate()
        {
            if (Year < MinimumYear || Month < MinimumMonth || Month > MonthsInYear)
            {
                throw new InvalidDuodecimDateException(
                    $"The combination of Year '{Year}' and Month '{Month}' does not represent a valid {nameof(DuodecimDate)}.");
            }
        }

        private static void ValidateValueNotLessThanZero(int value, string paramName)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "The value specified cannot be a negative number.");
            }
        }
    }
}