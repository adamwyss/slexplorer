using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a button control.
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates"),
     TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"),
     TemplateVisualState(Name = "Pressed", GroupName = "CommonStates"),
     TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"),
     TemplateVisualState(Name = "Focused", GroupName = "FocusStates"),
     TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
    public class MenuButton : ContentControl
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the Content dependency property.
        /// </summary>
        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(
                "Menu",
                typeof(Menu),
                typeof(MenuButton),
                new PropertyMetadata(OnMenuPropertyChanged));

        /// <summary>
        /// Identifies the IsPressed dependency property.
        /// </summary>
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
                "IsPressed",
                typeof(bool),
                typeof(MenuButton),
                new PropertyMetadata(OnIsPressedPropertyChanged));
        
        /// <summary>
        /// Identifies the IsMouseOver dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseOverProperty = DependencyProperty.Register(
                "IsMouseOver",
                typeof(bool),
                typeof(MenuButton),
                new PropertyMetadata(OnIsMouseOverPropertyChanged));

        /// <summary>
        /// Identifies the IsFocused dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.Register(
                "IsFocused",
                typeof(bool),
                typeof(MenuButton),
                new PropertyMetadata(OnIsFocusedPropertyChanged));

        /// <summary>
        /// Occurs when the Menu dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        public static void OnMenuPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MenuButton control = o as MenuButton;
            if (control != null)
            {
// do nothing here right now.
            }
        }

        /// <summary>
        /// Occurs when the IsPressed dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        public static void OnIsPressedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MenuButton control = o as MenuButton;
            if (control != null)
            {
                control.ChangeVisualState(true);
            }
        }

        /// <summary>
        /// Occurs when the IsMouseOver dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        public static void OnIsMouseOverPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MenuButton control = o as MenuButton;
            if (control != null)
            {
                control.ChangeVisualState(true);
            }
        }

        /// <summary>
        /// Occurs when the IsFocused dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        public static void OnIsFocusedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MenuButton control = o as MenuButton;
            if (control != null)
            {
                control.ChangeVisualState(true);
            }
        }

        #endregion

        /// <summary>
        /// Creates an instance of the DropDownButton class.
        /// </summary>
        public MenuButton()
        {
            this.DefaultStyleKey = typeof(MenuButton);

            // assign the default values for menu placement.
            this.MenuPlacementTarget = this;
            this.MenuPlacement = PlacementMode.Bottom;

            this.IsEnabledChanged += delegate { this.ChangeVisualState(true); };
        }

        /// <summary>
        /// Gets the menu property that is displayed when the button is pressed.
        /// </summary>
        public Menu Menu
        {
            get { return (Menu)this.GetValue(MenuProperty); }
            set { this.SetValue(MenuProperty, value); }
        }

        /// <summary>
        /// Gets a value that indicates whether a ButtonBase is currently activated.
        /// </summary>
        public bool IsPressed
        {
            get { return (bool)this.GetValue(IsPressedProperty); }
            private set { this.SetValue(IsPressedProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is located over this element.
        /// </summary>
        public bool IsMouseOver
        {
            get { return (bool)this.GetValue(IsMouseOverProperty); }
            private set { this.SetValue(IsMouseOverProperty, value); }
        }

        /// <summary>
        /// Gets a value that determines whether the button has focus.
        /// </summary>
        public bool IsFocused
        {
            get { return (bool)this.GetValue(IsFocusedProperty); }
            private set { this.SetValue(IsFocusedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the placement target of the menu.
        /// </summary>
        public UIElement MenuPlacementTarget { get; set; }

        /// <summary>
        /// Gets or sets the placement of the menu.
        /// </summary>
        public PlacementMode MenuPlacement { get; set; }

        /// <summary>
        /// Gets or set a value that indicates if a mouse is captured.
        /// </summary>
        private bool IsMouseCaptured { get; set; }

        /// <summary>
        /// Gets or sets a value tha indicates if the left mouse button is currently down.
        /// </summary>
        private bool IsMouseLeftButtonDown { get; set; }
        
        /// <summary>
        /// Provides class handling that occurs when the mouse enters this control
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.IsMouseOver = true;
        }
        
        /// <summary>
        /// Provides class handling that occurs when the mouse leaves this control.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.IsMouseOver = false;
        }

        /// <summary>
        /// Provides class handling that occurs when the left mouse button is pressed while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs that contain the event data.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                this.IsMouseLeftButtonDown = true;
                if (this.IsEnabled)
                {
                    e.Handled = true;

                    try
                    {
                        this.Focus();
                        this.CaptureMouseInternal();
                        if (this.IsMouseCaptured)
                        {
                            this.IsPressed = true;
                        }
                    }
                    finally
                    {
                        this.ChangeVisualState(true);
                    }
                }
            }
        }

        /// <summary>
        /// Provides class handling that occurs when the left mouse button is released while the mouse pointer is over this control.
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs that contain the event data.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (!e.Handled)
            {
                this.IsMouseLeftButtonDown = false;
                if (this.IsEnabled)
                {
                    e.Handled = true;
                    if (this.IsPressed)
                    {
                        this.OnToggle();
                    }

                    this.IsPressed = false;
                }
            }
        }

        /// <summary>
        /// Provides class handling that occurs when the mouse pointer moves while over this control.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.IsMouseLeftButtonDown && this.IsEnabled && this.IsMouseCaptured)
            {
                Point pos = e.GetPosition(this);
                this.IsPressed = (pos.X >= 0 && pos.X <= this.ActualWidth &&
                                  pos.Y >= 0 && pos.Y <= this.ActualHeight);
            }
        }

        /// <summary>
        /// Provides handling when this control loses the mouse capture.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            base.OnLostMouseCapture(e);

            this.ReleaseMouseCaptureInternal();
            this.IsPressed = false;
        }

        /// <summary>
        /// Provides handling that occurs when this control gets focus.
        /// </summary>
        /// <param name="e">The RoutedEventArgs that contain the event data.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.IsFocused = true;
        }

        /// <summary>
        /// Provides handling that occurs when this control loses focus.
        /// </summary>
        /// <param name="e">The RoutedEventArgs that contain the event data.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            this.IsFocused = false;
        }

        /// <summary>
        /// Updates the visual state of the control. 
        /// </summary>
        /// <param name="useTransitions">Indicates whether transitions should be used.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            if (!this.IsEnabled)
            {
                VisualStateManager.GoToState(this, "Disabled", useTransitions);
            }
            else if (this.IsPressed)
            {
                VisualStateManager.GoToState(this, "Pressed", useTransitions);
            }
            else if (this.IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }

            if (this.IsFocused && this.IsEnabled)
            {
                VisualStateManager.GoToState(this, "Focused", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unfocused", useTransitions);
            }
        }

        /// <summary>
        /// Called by the OnClick method to implement toggle behavior. 
        /// </summary>
        protected virtual void OnToggle()
        {
            if (this.Menu != null)
            {
                if (this.Menu.DataContext == null)
                {
                    this.Menu.DataContext = this.DataContext;
                }

                this.Menu.PlacementTarget = this.MenuPlacementTarget;
                this.Menu.Placement = this.MenuPlacement;

                this.Menu.IsOpen = true;
            }
        }

        /// <summary>
        /// Enables mouse capture for the <see cref="RibbonButton"/>.
        /// </summary>
        private void CaptureMouseInternal()
        {
            if (!this.IsMouseCaptured)
            {
                this.IsMouseCaptured = this.CaptureMouse();
            }
        }

        /// <summary>
        /// Releases mouse capture for the <see cref="RibbonButton"/>.
        /// </summary>
        private void ReleaseMouseCaptureInternal()
        {
            this.ReleaseMouseCapture();
            this.IsMouseCaptured = false;
        }
    }
}
