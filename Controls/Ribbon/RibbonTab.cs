using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a tab that is hosted in the ribbon control.
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates"),
     TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"),
     TemplateVisualState(Name = "Selected", GroupName = "SelectionStates"),
     TemplateVisualState(Name = "Unselected", GroupName = "SelectionStates"),
     ContentProperty("Groups")]
    public class RibbonTab : Control
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the Groups dependency property.
        /// </summary>
        public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register(
                "Groups",
                typeof(RibbonGroupCollection),
                typeof(RibbonTab),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
                "IsSelected",
                typeof(bool),
                typeof(RibbonTab),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedPropertyChanged)));

        /// <summary>
        /// Identifies the IsMouseOver dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseOverProperty = DependencyProperty.Register(
                "IsMouseOver",
                typeof(bool),
                typeof(RibbonTab),
                new PropertyMetadata(OnIsMouseOverPropertyChanged));

        /// <summary>
        /// Identifies the Label dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(RibbonTab),
                new PropertyMetadata(string.Empty));

        #endregion

        /// <summary>
        /// Initializes a new instance of the RibbonTab class.
        /// </summary>
        public RibbonTab()
            : base()
        {
            this.DefaultStyleKey = typeof(RibbonTab);

            this.Groups = new RibbonGroupCollection();

            this.Loaded += delegate(object sender, RoutedEventArgs e) { this.ChangeVisualState(); };
        }

        #region Properties

        /// <summary>
        /// Gets the items of the <see cref="RibbonTabGroup"/>
        /// </summary>
        public RibbonGroupCollection Groups
        {
            get { return (RibbonGroupCollection)this.GetValue(GroupsProperty); }
            private set { this.SetValue(GroupsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tab is selected or not.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is located over this element (including child elements in the visual tree).
        /// </summary>
        public bool IsMouseOver
        {
            get { return (bool)this.GetValue(IsMouseOverProperty); }
            private set { this.SetValue(IsMouseOverProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text to display on the tab.
        /// </summary>
        public string Label
        {
            get { return (string)this.GetValue(LabelProperty); }
            set { this.SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets the ribbon that this tab is hosted in.
        /// </summary>
        private Ribbon RibbonParent
        {
            get
            {
                // check our parent
                Ribbon parent = this.Parent as Ribbon;
                if (parent != null)
                {
                    return parent;
                }

                // decend the visual tree looking for the ribbon we are hosted in.
                for (DependencyObject obj = this; obj != null; obj = VisualTreeHelper.GetParent(obj))
                {
                    parent = obj as Ribbon;
                    if (parent != null)
                    {
                        return parent;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the visual state of the control. 
        /// </summary>
        internal void ChangeVisualState()
        {
            if (this.IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }

            if (this.IsSelected)
            {
                VisualStateManager.GoToState(this, "Selected", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unselected", true);
            }

            // DEVELOPER NOTE: This is a workaround to move the tab down one unit in order
            // to hide the line that exists below the tab, to make the tab appear selected.
            // Ideally, this would be in the xaml, but I cant figure out how to set the 
            // property in the xaml visual state manager storyboard.
            if (this.IsSelected)
            {
                this.SetValue(FrameworkElement.MarginProperty, new Thickness(2, 1, 2, 0));
            }
            else
            {
                this.ClearValue(FrameworkElement.MarginProperty);
            }
        }

        /// <summary>
        /// Occurs when the mouse enters the control surface.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.IsMouseOver = true;
        }

        /// <summary>
        /// Occurs when the mouse leaves the control surface.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.IsMouseOver = false;
        }

        /// <summary>
        /// Occurs when the left mouse button is pressed and selects the current tab.
        /// </summary>
        /// <param name="e">The MouseButtonEventArgs that contain the event data.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (this.RibbonParent != null && !e.Handled)
            {
                e.Handled = true;
                this.RibbonParent.SelectedTab = this;
            }
        }

        /// <summary>
        /// Occurs when the IsSelected property changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnIsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RibbonTab tab = sender as RibbonTab;
            if (tab != null)
            {
                Ribbon ribbon = tab.RibbonParent;
                if (ribbon != null)
                {
                    bool isselected = (bool)e.NewValue;
                    if (isselected)
                    {
                        ribbon.SelectedTab = tab;
                    }
                    else if (ribbon.SelectedTab == tab)
                    {
                        ribbon.SelectedTab = null;
                    }

                    tab.ChangeVisualState();
                }
            }
        }

        /// <summary>
        /// Occurs when the IsMouseOver property changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnIsMouseOverPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RibbonTab tab = sender as RibbonTab;
            if (tab != null)
            {
                tab.ChangeVisualState();
            }
        }

        #endregion
    }
}