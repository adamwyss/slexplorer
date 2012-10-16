using System;
using System.Globalization;
using System.Windows.Data;
using Ijv.Redstone.Explorer;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Converts a double to and from a boolean based on the provided parameter
    /// </summary>
    public class ExplorerViewsConverter : IValueConverter
    {
        /// <summary>
        /// Converts a ExplorerViews enum value to a boolean value.
        /// </summary>
        /// <param name="value">The value that is being converted.</param>
        /// <param name="targetType">The target type of the converter.</param>
        /// <param name="parameter">The parameter that provides the expected value (as a string).</param>
        /// <param name="culture">The culture info.</param>
        /// <returns>True if the value matches the parameter; otherwise false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return object.Equals(value, Enum.Parse(typeof(ExplorerViews), (string)parameter, false));
        }

        /// <summary>
        /// Converts a boolean value into a ExplorerViews enum value value.
        /// </summary>
        /// <param name="value">The boolean value being converted.</param>
        /// <param name="targetType">The target type of the converter.</param>
        /// <param name="parameter">The parameter value that provides the expected double value (as a string).</param>
        /// <param name="culture">The culture info</param>
        /// <returns>The parameter value as a double.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.Parse(typeof(ExplorerViews), (string)parameter, false);
        }
    }
}
