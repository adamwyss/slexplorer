using System.Windows;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Provides a way to choose a DataTemplate based on the data object and the data-bound element.
    /// </summary>
    public class DataTemplateSelector
    {
        /// <summary>
        /// Creates a new instance of the DataTemplateSelector class.
        /// </summary>
        public DataTemplateSelector()
        {
        }

        /// <summary>
        /// When overridden in a derived class, returns a DataTemplate based on custom logic.
        /// </summary>
        /// <param name="item">The data object for which to select the template.</param>
        /// <param name="container">The data-bound object.</param>
        /// <returns>Returns a DataTemplate or null reference.</returns>
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }
    }
}
