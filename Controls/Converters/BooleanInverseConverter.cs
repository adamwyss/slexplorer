using System;
using System.Globalization;
using System.Windows.Data;

namespace Ijv.Redstone.Controls
{
    /// <summary />
    public class BooleanInverseConverter : IValueConverter
    {
        /// <summary />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            return !boolValue;
        }

        /// <summary />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert(value, targetType, parameter, culture);
        }
    }
}
