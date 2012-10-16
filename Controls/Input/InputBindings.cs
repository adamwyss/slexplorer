using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Ijv.Redstone.Input
{
    /// <summary>
    /// Allows the input bindings to be attached to any ui element.
    /// </summary>
    public static class InputBindings
    {
        /// <summary>
        /// Identifies the InputBindingsProperty attached property
        /// </summary>
        public static readonly DependencyProperty InputBindingsProperty = DependencyProperty.RegisterAttached(
            "InputBindings",
            typeof(InputBindingCollection),
            typeof(InputBindings),
            new PropertyMetadata(OnInputBindingsPropertyChanged));

        #region Command Get/Set Methods

        /// <summary>
        /// Gets the value of the Menu attached property from a given DependencyObject. 
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>The value of the Menu attached property.</returns>
        public static InputBindingCollection GetInputBindings(UIElement control)
        {
            // preconditions

            Argument.IsNotNull("control", control);

            // implementation

            return (InputBindingCollection)control.GetValue(InputBindingsProperty);
        }

        /// <summary>
        /// Sets the value of the Menu attached property to a given DependencyObject. 
        /// </summary>
        /// <param name="element">The element on which to set the property value.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetInputBindings(UIElement control, InputBindingCollection value)
        {
            // preconditions

            Argument.IsNotNull("control", control);

            // implementation

            control.SetValue(InputBindingsProperty, value);
        }

        #endregion

        /// <summary>
        /// Occurs when the InputBindings property changes.
        /// </summary>
        /// <param name="sender">The dependency object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnInputBindingsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement control = sender as UIElement;
            if (control != null)
            {
                control.KeyDown += OnKeyPressed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The KeyEventArgs that contains the event data.</param>
        private static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            UIElement control = sender as UIElement;
            if (control != null)
            {
                InputBindingCollection bindingCollection = InputBindings.GetInputBindings(control);
                if (bindingCollection.Count > 0)
                {
                    foreach (InputBinding binding in bindingCollection)
                    {
                        KeyBinding keyBinding = binding as KeyBinding;
                        if (keyBinding != null &&
                            keyBinding.Key == e.Key &&
                            keyBinding.Modifier == (Keyboard.Modifiers & keyBinding.Modifier))
                        {
                            ExecuteCommand(binding.Command, binding.CommandParameter);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to executes a command command.
        /// </summary>
        /// <param name="cmd">The command to be executed.</param>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command was executed; otherwise false.</returns>
        private static bool ExecuteCommand(ICommand cmd, object parameter)
        {
            if (cmd != null && cmd.CanExecute(parameter))
            {
                cmd.Execute(parameter);
                return true;
            }

            return false;
        }
    }
}
