using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a selectable item inside a Menu. 
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates"),
     TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"),
     TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"),
     TemplateVisualState(Name = "Checked", GroupName = "CheckedStates"),
     TemplateVisualState(Name = "Unchecked", GroupName = "CheckedStates")]
    public partial class MenuItem : HeaderedItemsControl
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the Command dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(MenuItem),
            new PropertyMetadata(new PropertyChangedCallback(OnCommandPropertyChanged)));

        /// <summary>
        /// Identifies the CommandProperty dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(MenuItem),
            null);

        /// <summary>
        /// Identifies the IsMouseOver dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseOverProperty = DependencyProperty.Register(
            "IsMouseOver",
            typeof(bool),
            typeof(MenuItem),
            new PropertyMetadata(OnIsMouseOverPropertyChanged));

        /// <summary>
        /// Identifies the IsCheckable dependency property
        /// </summary>
        public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(
            "IsCheckable",
            typeof(bool),
            typeof(MenuItem),
            new PropertyMetadata(false, OnIsCheckablePropertyChanged));

        /// <summary>
        /// Identifies the IsChecked dependency property
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(
            "IsChecked",
            typeof(bool),
            typeof(MenuItem),
            new PropertyMetadata(OnIsCheckedPropertyChanged));

        /// <summary>
        /// Identifies the Icon dependency property
        /// </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(BitmapSource),
            typeof(MenuItem),
            null);

        /// <summary>
        /// Occurs when the Command property changes.
        /// </summary>
        /// <param name="sender">The DependencyObject that that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MenuItem control = sender as MenuItem;
            if (control != null)
            {
                control.UnhookCommand(e.OldValue as ICommand);
                control.HookCommand(e.NewValue as ICommand);
            }
        }

        /// <summary>
        /// Occurs when the IsMouseOver dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        public static void OnIsMouseOverPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MenuItem control = o as MenuItem;
            if (control != null)
            {
                control.ChangeVisualState(true);

                if (control.Items.Count > 0 && (bool)e.NewValue)
                {
                    control.OpenSubmenu();
                }
            }
        }

        /// <summary>
        /// Occurs when the IsCheckable dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnIsCheckablePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MenuItem control = sender as MenuItem;
            if (control != null)
            {
                control.ChangeVisualState(true);
            }
        }

        /// <summary>
        /// Occurs when the IsChecked dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnIsCheckedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MenuItem control = sender as MenuItem;
            if (control != null && control.IsCheckable)
            {
                RoutedEventArgs args = new RoutedEventArgs();

                bool ischecked = (bool)e.NewValue;
                if (ischecked)
                {
                    control.OnChecked(args);
                }
                else
                {
                    control.OnUnchecked(args);
                }

                control.ChangeVisualState(true);
            }
        }

        #endregion

        /// <summary>
        /// Creates an instance of the MenuItem control.
        /// </summary>
        public MenuItem()
        {
            this.DefaultStyleKey = typeof(MenuItem);
            this.IsEnabledChanged += OnIsEnabledChanged;
            this.Loaded += OnControlLoaded;
        }

        /// <summary>
        /// Occurs when a menu item is clicked. 
        /// </summary>
        public event RoutedEventHandler Click;

        /// <summary>
        /// Occurs when a menu item is checked.
        /// </summary>
        public event RoutedEventHandler Checked;

        /// <summary>
        /// Occurs when a menu item is unchecked.
        /// </summary>
        public event RoutedEventHandler Unchecked;

        /// <summary>
        /// Gets or sets the command to invoke when the dialog button is pressed.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the Command property.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
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
        /// Gets a value that indicates whether a menu item can be checked.
        /// </summary>
        public bool IsCheckable
        {
            get { return (bool)this.GetValue(IsCheckableProperty); }
            set { this.SetValue(IsCheckableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the menu item is checked.
        /// </summary>
        public bool IsChecked
        {
            get { return (bool)this.GetValue(IsCheckedProperty); }
            set { this.SetValue(IsCheckedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the icon that appears in a menu item.
        /// </summary>
        public BitmapSource Icon
        {
            get { return (BitmapSource)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value tha indicates if the left mouse button is currently down.
        /// </summary>
        private bool IsMouseLeftButtonDown { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the menu item is currently activated.
        /// </summary>
        private bool IsPressed { get; set; }

        /// <summary>
        /// Gets or set a value that indicates if a mouse is captured.
        /// </summary>
        private bool IsMouseCaptured { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Path glyph = this.GetTemplateChild("ArrowGlyph") as Path;
            if (glyph != null)
            {
                glyph.Visibility = this.Items.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Provides class handling that occurs when the mouse enters this control.
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
            // we will handle the mouse down when this menu item has a submenu to display.
            e.Handled = this.Items.Count > 0;

            base.OnMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                this.IsMouseLeftButtonDown = true;
                if (this.IsEnabled)
                {
                    e.Handled = true;

                    this.CaptureMouseInternal();
                    if (this.IsMouseCaptured)
                    {
                        this.IsPressed = true;
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
            // we will handle the mouse up when this menu item has a submenu to display.
            e.Handled = this.Items.Count > 0;

            base.OnMouseLeftButtonUp(e);
            if (!e.Handled)
            {
                this.IsMouseLeftButtonDown = false;
                if (this.IsEnabled)
                {
                    e.Handled = true;
                    if (this.IsPressed)
                    {
                        this.CloseRootMenu();
                        this.OnClick();

                        // if the menu item is checkable, update
                        if (this.IsCheckable)
                        {
                            this.OnToggle();
                        }
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
        /// Raises the click routed event. 
        /// </summary>
        protected virtual void OnClick()
        {
            RoutedEventHandler handler = this.Click;
            if (handler != null)
            {
                RoutedEventArgs e = new RoutedEventArgs();
                handler(this, e);
            }

            // execute the command
            if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
            {
                this.Command.Execute(this.CommandParameter);
            }
        }

        /// <summary>
        /// Called when the menu item raises a checked event.
        /// </summary>
        /// <param name="e">The RoutedEventArgs that contain the event data.</param>
        protected virtual void OnChecked(RoutedEventArgs e)
        {
            RoutedEventHandler handler = this.Checked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called when the menu item raises a unchecked event.
        /// </summary>
        /// <param name="e">The RoutedEventArgs that contain the event data.</param>
        protected virtual void OnUnchecked(RoutedEventArgs e)
        {
            RoutedEventHandler handler = this.Unchecked;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Called by the OnClick method to implement toggle behavior. 
        /// </summary>
        protected virtual void OnToggle()
        {
            this.IsChecked = !this.IsChecked;
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

                // when the control is disabled, the mouse leave event never fires.
                this.IsMouseOver = false;
            }
            else if (this.IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }

            if (this.IsCheckable && this.IsChecked)
            {
                VisualStateManager.GoToState(this, "Checked", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unchecked", useTransitions);
            }
        }

        /// <summary>
        /// Enables mouse capture for the menu item.
        /// </summary>
        private void CaptureMouseInternal()
        {
            if (!this.IsMouseCaptured)
            {
                this.IsMouseCaptured = this.CaptureMouse();
            }
        }

        /// <summary>
        /// Releases mouse capture for the menu item.
        /// </summary>
        private void ReleaseMouseCaptureInternal()
        {
            this.ReleaseMouseCapture();
            this.IsMouseCaptured = false;
        }

        /// <summary>
        /// Walks the logical tree looking for the root menu that hosts this menu item and closes it.
        /// </summary>
        /// <remarks>
        /// We rely on the fact that the menuitems can host other menu items and that the only ribbon in the 
        /// logical tree is the root menu.
        /// 
        /// If no menu can be found in the logical tree, then this method no-ops.
        /// </remarks>
        private void CloseRootMenu()
        {
            // decend the logical tree looking for the menu we are hosted in.
            for (DependencyObject obj = this.Parent; obj != null && obj is FrameworkElement; obj = ((FrameworkElement)obj).Parent)
            {
                Menu menu = obj as Menu;
                if (menu != null)
                {
                    menu.IsOpen = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Update the visual state when the IsEnabled dependency property value changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ChangeVisualState(true);
        }

        /// <summary>
        /// Updates the visual state when the control loads.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnControlLoaded(object sender, RoutedEventArgs e)
        {
            this.ChangeVisualState(true);
        }

        /// <summary>
        /// Start monitoring the command for CanExecute changes.
        /// </summary>
        /// <param name="cmd">The command.</param>
        private void HookCommand(ICommand cmd)
        {
            if (cmd != null)
            {
                cmd.CanExecuteChanged += this.OnCommandCanExecuteChanged;
                this.IsEnabled = cmd.CanExecute(this.CommandParameter);
                this.ChangeVisualState(false);
            }
        }

        /// <summary>
        /// Stops monitoring the command for CanExecute changes.
        /// </summary>
        /// <param name="cmd">The command.</param>
        private void UnhookCommand(ICommand cmd)
        {
            if (cmd != null)
            {
                cmd.CanExecuteChanged -= this.OnCommandCanExecuteChanged;
            }
        }

        /// <summary>
        /// Occurs when the ICommand.CanExecuteChanged event fires.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The EventArgs that contain the event data.</param>
        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            ICommand cmd = sender as ICommand;
            if (cmd != null)
            {
                this.IsEnabled = cmd.CanExecute(this.CommandParameter);
                this.ChangeVisualState(false);
            }
        }
    }
}
