using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a ribbon control.
    /// </summary>
    [TemplatePart(Name = "RibbonTabPanel", Type = typeof(Panel)),
    ContentProperty("Tabs")]
    public class Ribbon : Control
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the Tabs dependency property.
        /// </summary>
        public static readonly DependencyProperty TabsProperty = DependencyProperty.Register(
            "Tabs",
            typeof(RibbonTabCollection),
            typeof(Ribbon),
            null);

        /// <summary>
        /// Identifies the SelectedTab dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedTabProperty = DependencyProperty.Register(
                "SelectedTab",
                typeof(RibbonTab),
                typeof(Ribbon),
                new PropertyMetadata(new PropertyChangedCallback(OnSelectedTabPropertyChanged)));

        /// <summary>
        /// Identifies the SelectedGroups dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedGroupsProperty = DependencyProperty.Register(
                "SelectedGroups",
                typeof(RibbonGroupCollection),
                typeof(Ribbon),
                null);

        /// <summary>
        /// Identifies the QuickAccessItems dependency property.
        /// </summary>
        public static readonly DependencyProperty QuickAccessItemsProperty = DependencyProperty.Register(
                "QuickAccessItems",
                typeof(ControlsCollection),
                typeof(Ribbon),
                new PropertyMetadata(null));

        #endregion

        /// <summary>
        /// The panel that contains the tabs.
        /// </summary>
        private Panel elementTabPanel;

        /// <summary>
        /// Initializes a new instance of the Ribbon class.
        /// </summary>
        public Ribbon()
            : base()
        {
            this.DefaultStyleKey = typeof(Ribbon);

            this.Tabs = new RibbonTabCollection();
            this.Tabs.CollectionChanged += this.OnTabCollectionChanged;
        }

        #region Properties

        /// <summary>
        /// Gets a collection of tabs that exist in this ribbon.
        /// </summary>
        public RibbonTabCollection Tabs
        {
            get { return (RibbonTabCollection)this.GetValue(TabsProperty); }
            private set { this.SetValue(TabsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the currently selected item. 
        /// </summary>
        public RibbonTab SelectedTab
        {
            get { return (RibbonTab)this.GetValue(SelectedTabProperty); }
            set { this.SetValue(SelectedTabProperty, value); }
        }

        /// <summary>
        /// Gets the groups associated to the selected tab.
        /// </summary>
        public RibbonGroupCollection SelectedGroups
        {
            get { return (RibbonGroupCollection)this.GetValue(SelectedGroupsProperty); }
            private set { this.SetValue(SelectedGroupsProperty, value); }
        }

        #endregion

        /// <summary>
        /// Is invoked whenever application code or internal processes call ApplyTemplate.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // clear the old tab panel
            if (this.elementTabPanel != null)
            {
                this.elementTabPanel.Children.Clear();
            }

            this.elementTabPanel = this.GetTemplateChild("RibbonTabPanel") as Panel;
            this.UpdateTabCollection(this.elementTabPanel);
        }

        /// <summary>
        /// Occur when the SelectedTab dependency property changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnSelectedTabPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Ribbon control = sender as Ribbon;
            if (control != null)
            {
                RibbonTab oldTab = e.OldValue as RibbonTab;
                RibbonTab newTab = e.NewValue as RibbonTab;

                if (oldTab != null)
                {
                    oldTab.IsSelected = false;
                }

                if (newTab != null)
                {
                    newTab.IsSelected = true;
                    control.SelectedGroups = newTab.Groups;
                }
            }
        }

        /// <summary>
        /// Updates the tab collection into the user interface.
        /// </summary>
        /// <param name="tabPanel">The panel that contains the tab controls.</param>
        private void UpdateTabCollection(Panel tabPanel)
        {
            if (tabPanel != null)
            {
                if (tabPanel.Children.Count > 0)
                {
                    tabPanel.Children.Clear();
                }

                RibbonTab selectedTab = null;

                foreach (RibbonTab tab in this.Tabs)
                {
                    tabPanel.Children.Add(tab);

                    if (tab.IsSelected)
                    {
                        selectedTab = tab;
                    }
                }

                if (selectedTab != null)
                {
                    this.SelectedTab = selectedTab;
                }
                else if (tabPanel.Children.Count > 0)
                {
                    // select the first tab
                    this.SelectedTab = tabPanel.Children[0] as RibbonTab;
                }
            }
        }

        /// <summary>
        /// Occurs when the tab collection changes
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs that contains the event data.</param>
        private void OnTabCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.UpdateTabCollection(this.elementTabPanel);
        }
    }
}
