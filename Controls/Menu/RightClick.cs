using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents an context menu that can be attached to any ui element.
    /// </summary>
    public class RightClick
    {
        #region Attached Properties

        /// <summary>
        /// Identifies the Menu attached property.
        /// </summary>
        public static readonly DependencyProperty ContextMenuProperty = DependencyProperty.RegisterAttached(
            "ContextMenu",
            typeof(Menu),
            typeof(RightClick),
            new PropertyMetadata(OnMenuPropertyChanged));

        #region Command Get/Set Methods

        /// <summary>
        /// Gets the value of the Menu attached property from a given DependencyObject. 
        /// </summary>
        /// <param name="control">The element from which to read the property value.</param>
        /// <returns>The value of the Menu attached property.</returns>
        public static Menu GetContextMenu(UIElement control)
        {
            // preconditions

            Argument.IsNotNull("control", control);

            // implementation

            return control.GetValue(ContextMenuProperty) as Menu;
        }

        /// <summary>
        /// Sets the value of the Menu attached property to a given DependencyObject. 
        /// </summary>
        /// <param name="control">The element on which to set the property value.</param>
        /// <param name="menu">The property value to set.</param>
        public static void SetContextMenu(UIElement control, Menu menu)
        {
            // preconditions

            Argument.IsNotNull("control", control);

            // implementation

            control.SetValue(ContextMenuProperty, menu);
        }

        #endregion

        /// <summary>
        /// Occurs when the Menu property changes.
        /// </summary>
        /// <param name="sender">The dependency object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnMenuPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UIElement control = sender as UIElement;
            if (control != null)
            {
                if (e.NewValue == null)
                {
                    // remove the event handlers if the menu is nulled out for some reason.
                    control.MouseRightButtonDown -= OnMouseRightButtonDown;
                    control.MouseRightButtonUp -= OnMouseRightButtonUp;
                }
                else
                {
                    // attach our right click event handlers to the control so that we can
                    // display the menu when the item is clicked.
                    control.MouseRightButtonDown += OnMouseRightButtonDown;
                    control.MouseRightButtonUp += OnMouseRightButtonUp;
                }
            }
        }

        #endregion

        /// <summary>
        /// Occurs when the right mouse button is pressed.  We handle it so the default silverlight contextmenu does not appear.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The MouseButtonEventArgs that contains the event data.</param>
        private static void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = sender is UIElement;
        }

        /// <summary>
        /// Displays the context menu when the right mouse button is released.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The MouseButtonEventArgs that contains the event data.</param>
        private static void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement control = sender as UIElement;
            if (control != null)
            {
                Menu menu = GetContextMenu(control);

                if (menu == null)
                {
                    throw new NullReferenceException("The context menu must not be null.");
                }

                if (menu.DataContext == null)
                {
                    FrameworkElement element = control as FrameworkElement;
                    if (element != null)
                    {
                        menu.DataContext = element.DataContext;
                    }
                }

                // place our menu at the mouse loocation
                menu.Placement = PlacementMode.Mouse;
                menu.Offset = e.GetPosition(null);

                menu.IsOpen = true;
                e.Handled = true;
            }
        }
    }
}
