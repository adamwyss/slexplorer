using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Ijv.Redstone.Controls
{
    /// <summary />
    public class VisibilityConverter : IValueConverter
    {
        /// <summary />
        public bool VisibleIs { get; set; }

        /// <summary />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            return this.VisibleIs == boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            switch (visibility)
            {
                case Visibility.Visible:
                    return this.VisibleIs;

                case Visibility.Collapsed:
                    return !this.VisibleIs;
            }

            throw new InvalidOperationException("The Visibility value provided was unable to be converted back.");
        }
    }
}
