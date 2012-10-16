using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ijv.Redstone.Controls
{
    /// <summary />
    public class TextWrappingConverter : IValueConverter
    {
        /// <summary />
        public bool WrapIs { get; set; }

        /// <summary />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            return this.WrapIs == boolValue ? TextWrapping.Wrap : TextWrapping.NoWrap;
        }

        /// <summary />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextWrapping wrapping = (TextWrapping)value;

            switch (wrapping)
            {
                case TextWrapping.Wrap:
                    return this.WrapIs;

                case TextWrapping.NoWrap:
                    return !this.WrapIs;
            }

            throw new InvalidOperationException("The TextWrapping value provided was unable to be converted back.");
        }
    }
}
