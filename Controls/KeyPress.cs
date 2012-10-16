using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Allows the binding sources to be updated for each and every key press.
    /// </summary>
    public static class KeyPress
    {
        /// <summary>
        /// Identifies the IsBoundOnChange attached property
        /// </summary>
        public static readonly DependencyProperty IsBoundOnChangeProperty = DependencyProperty.RegisterAttached(
            "IsBoundOnChange",
            typeof(bool),
            typeof(KeyPress),
            new PropertyMetadata(false, OnIsBoundOnChangePropertyChanged));

        #region Command Get/Set Methods

        /// <summary>
        /// Gets the value of the Menu attached property from a given DependencyObject. 
        /// </summary>
        /// <param name="control">The element from which to read the property value.</param>
        /// <returns>The value of the Menu attached property.</returns>
        public static bool GetIsBoundOnChange(DependencyObject control)
        {
            // preconditions

            Argument.IsNotNull("control", control);

            // implementation

            return (bool)control.GetValue(IsBoundOnChangeProperty);
        }

        /// <summary>
        /// Sets the value of the Menu attached property to a given DependencyObject. 
        /// </summary>
        /// <param name="control">The element on which to set the property value.</param>
        /// <param name="command">The property value to set.</param>
        public static void SetIsBoundOnChange(DependencyObject control, bool value)
        {
            // preconditions

            Argument.IsNotNull("control", control);

            // implementation

            control.SetValue(IsBoundOnChangeProperty, value);
        }

        #endregion

        /// <summary>
        /// Occurs when the IsBoundOnChange property changes.
        /// </summary>
        /// <param name="sender">The dependency object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnIsBoundOnChangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (sender != null)
            {
                bool enabled = (bool)e.NewValue;

                if (enabled)
                {
                    textbox.TextChanged += OnTextChanged;
                }
                else
                {
                    textbox.TextChanged -= OnTextChanged;
                }
            }
        }

        /// <summary>
        /// Occurs when the text changes and updates any bindings that exist.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The TextChangedEventArgs that contain the event data.</param>
        private static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            if (textbox != null)
            {
                BindingExpression binding = textbox.GetBindingExpression(TextBox.TextProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                }
            }
        }
    }
}
