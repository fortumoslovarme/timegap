using System;
using System.ComponentModel;
using System.Globalization;

namespace TimeGap.Converters
{
    public class DuodecimDateConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is null || value is string)
            {
                var text = ((string) value)?.Trim() ?? string.Empty;
                if (text.Length == 0)
                {
                    return DuodecimDate.MinValue;
                }

                DateTimeFormatInfo formatInfo = null;

                if (culture != null)
                {
                    formatInfo = (DateTimeFormatInfo) culture.GetFormat(typeof(DateTimeFormatInfo));
                }

                if (formatInfo != null)
                {
                    return DuodecimDate.Parse(text, formatInfo);
                }

                return DuodecimDate.Parse(text);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof(string) && value is DuodecimDate)
            {
                var duodecimDate = (DuodecimDate) value;
                return Convert.ToString(duodecimDate);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}