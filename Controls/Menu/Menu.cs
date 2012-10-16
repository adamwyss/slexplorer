using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a Windows menu control that enables you to hierarchically organize elements associated with commands and event handlers.
    /// </summary>
    [TemplateVisualState(Name = "Open", GroupName = "MenuStates"),
     TemplateVisualState(Name = "Closed", GroupName = "MenuStates")]
    public class Menu : ItemsControl
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the IsOpen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
                "IsOpen",
                typeof(bool),
                typeof(Menu),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenPropertyChanged)));

        /// <summary>
        /// Identifies the Placement dependency property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register(
                "Placement",
                typeof(PlacementMode),
                typeof(Menu),
                null);

        /// <summary>
        /// Identifies the PlacementTarget dependency property.
        /// </summary>
        public static readonly DependencyProperty PlacementTargetProperty = DependencyProperty.Register(
                "PlacementTarget",
                typeof(UIElement),
                typeof(Menu),
                null);

        /// <summary>
        /// Identifies the Offset dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
                "Offset",
                typeof(Point),
                typeof(Menu),
                null);

        /// <summary>
        /// Occurs when the IsOpened dependency property value changes.
        /// </summary>
        /// <param name="o">The DependencyObject that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        public static void OnIsOpenPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Menu control = o as Menu;
            if (control != null)
            {
                bool isMenuOpen = (bool)e.NewValue;

                if (isMenuOpen)
                {
                    control.OpenPopup();
                }
                else
                {
                    control.ClosePopup();
                }

                control.ChangeVisualState(true);
            }
        }

        #endregion

        /// <summary>
        /// The popup control that hosts the menu.
        /// </summary>
        private Popup parentPopup;

        /// <summary>
        /// The parent canvas that eats the dismiss click.
        /// </summary>
        private Canvas dismissCanvas;

        /// <summary>
        /// True if this item is a submenu.
        /// </summary>
        private bool noDismiss = false;

        /// <summary>
        /// Creates an instance of the Menu class.
        /// </summary>
        public Menu()
        {
            this.DefaultStyleKey = typeof(Menu);

            this.SizeChanged += this.OnMenuSizeChanged;
        }

        /// <summary>
        /// Creates an instance of the Menu class.
        /// </summary>
        /// <param name="parent">The parent menu of this menu.</param>
        internal Menu(Menu parent)
            : this()
        {
            this.noDismiss = true;

            // watch our parent and when they close, we should close as well.
            parent.Closed += delegate(object sender, RoutedEventArgs e) { this.IsOpen = false; };
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value that indicates whether the menu is visible.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)this.GetValue(IsOpenProperty); }
            set { this.SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the placement location of a menu.
        /// </summary>
        public PlacementMode Placement
        {
            get { return (PlacementMode)this.GetValue(PlacementProperty); }
            set { this.SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the UIElement relative to which the menu is positioned when it opens.
        /// </summary>
        public UIElement PlacementTarget
        {
            get { return (UIElement)this.GetValue(PlacementTargetProperty); }
            set { this.SetValue(PlacementTargetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the visual offset of the menu in absolute coordinates.
        /// </summary>
        public Point Offset
        {
            get { return (Point)this.GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a particular instance of a menu opens.
        /// </summary>
        public event RoutedEventHandler Opened;

        /// <summary>
        /// Occurs when a particular instance of a menu closes. 
        /// </summary>
        public event RoutedEventHandler Closed;

        #endregion

        /// <summary>
        /// Creates or identifies the element used to display the specified item.
        /// </summary>
        /// <returns>A menu item.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MenuItem();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own item container. 
        /// </summary>
        /// <param name="item">Specified item.</param>
        /// <returns>true if the item is its own item container; otherwise, false.</returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MenuItem || item is MenuSeparator;
        }

        /// <summary>
        /// Ensure that the pop up control is created and initialized.
        /// </summary>
        private void EnsureParentPopup()
        {
            if (this.parentPopup == null)
            {
                this.IsTabStop = false;

                this.parentPopup = new Popup();
                this.parentPopup.Opened += new EventHandler(OnPopupOpened);
                this.parentPopup.Closed += new EventHandler(OnPopupClosed);

                if (this.noDismiss)
                {
                    this.parentPopup.Child = this;
                }
                else
                {
                    this.dismissCanvas = new Canvas();
                    this.dismissCanvas.Background = new SolidColorBrush(Colors.Transparent);
                    this.dismissCanvas.MouseLeftButtonDown += this.OnDismissMenuMouseClicked;
                    this.dismissCanvas.MouseRightButtonDown += this.OnDismissMenuMouseClicked;

                    this.parentPopup.Child = this.dismissCanvas;
                    this.dismissCanvas.Children.Add(this);
                }
            }
        }

        /// <summary>
        /// Performs the action to open the menu.
        /// </summary>
        private void OpenPopup()
        {
            this.ApplyTemplate();

            this.EnsureParentPopup();
            this.parentPopup.IsOpen = true;
        }

        /// <summary>
        /// Performs the action to close the menu.
        /// </summary>
        private void ClosePopup()
        {
            if (this.parentPopup != null)
            {
                this.parentPopup.IsOpen = false;
            }
        }

        /// <summary>
        /// Places the popup at the specified position.
        /// </summary>
        private void PerformPlacement()
        {
            Rect plugin = new Rect(0, 0, Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight);
            Point[] menuPoint = PopupPlacementHelper.GetTranslatedPoints(this);

            Point location = new Point();

            switch (this.Placement)
            {
                case PlacementMode.Mouse:
                    {
                        if (this.Offset == null)
                        {
                            throw new NullReferenceException("PlacementEventData can not be null.");
                        }

                        location = this.Offset;

                        if (this.Offset.X + this.ActualWidth > plugin.Width)
                        {
                            location.X -= this.ActualWidth;
                        }

                        if (this.Offset.Y + this.ActualHeight > plugin.Height)
                        {
                            location.Y -= this.ActualHeight;
                        }
                    }
                    break;

                case PlacementMode.Left:
                case PlacementMode.Top:
                case PlacementMode.Right:
                case PlacementMode.Bottom:
                    {
                        if (this.PlacementTarget == null)
                        {
                            throw new NullReferenceException("PlacementTarget can not be null.");
                        }

                        // places the menu at the specified location around the garget.
                        Point[] translatedPoints = PopupPlacementHelper.GetTranslatedPoints((FrameworkElement)this.PlacementTarget);
                        location = PopupPlacementHelper.PlacePopup(plugin, translatedPoints, menuPoint, this.Placement);
                    }
                    break;

                default:
                    throw new InvalidOperationException("Invalid placement mode specified.");
            }


            if (this.dismissCanvas == null)
            {
                this.parentPopup.VerticalOffset = location.Y;
                this.parentPopup.HorizontalOffset = location.X;
            }
            else
            {
                this.dismissCanvas.Width = plugin.Width;
                this.dismissCanvas.Height = plugin.Height;

                Canvas.SetTop(this, location.Y);
                Canvas.SetLeft(this, location.X);
            }
        }

        /// <summary>
        /// Updates the visual state of the control. 
        /// </summary>
        /// <param name="useTransitions">Indicates whether transitions should be used.</param>
        private void ChangeVisualState(bool useTransitions)
        {
            if (this.IsOpen)
            {
                VisualStateManager.GoToState(this, "Opened", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Closed", useTransitions);
            }
        }

        /// <summary>
        /// Occurs when the size of the menu changes when it opens.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The SizeChangedEventArgs that contain the event data.</param>
        private void OnMenuSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.IsOpen)
            {
                this.PerformPlacement();
            }
        }

        /// <summary>
        /// Occurs the mouse is clicked outside of the menu, in an attempt to dismiss it.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The MouseButtonEventArgs that contain the event data.</param>
        private void OnDismissMenuMouseClicked(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.IsOpen = false;
        }

        /// <summary>
        /// Raise the event indicating the popup has been opened.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The EventArgs that contain the event data.</param>
        private void OnPopupOpened(object sender, EventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs();

            // do we need to make this a protected virtual method?
            RoutedEventHandler handler = this.Opened;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Raise the event indicating the popup has been closed.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The EventArgs that contain the event data.</param>
        private void OnPopupClosed(object sender, EventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs();

            // do we need to make this a protected virtual method?
            RoutedEventHandler handler = this.Closed;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}
