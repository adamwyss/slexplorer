using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a grouping of ribbon controls
    /// </summary>
    [ContentProperty("Items")]
    public class RibbonControlGroup : Control
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the Items dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
                "Items",
                typeof(ControlsCollection),
                typeof(RibbonControlGroup),
                null);

        #endregion

        /// <summary>
        /// Initializes a new instance of the RibbonControlGroup class.
        /// </summary>
        public RibbonControlGroup()
        {
            this.DefaultStyleKey = typeof(RibbonControlGroup);
            this.Items = new ControlsCollection();
        }

        /// <summary>
        /// Gets the items of this group.
        /// </summary>
        public ControlsCollection Items
        {
            get { return (ControlsCollection)this.GetValue(ItemsProperty); }
            private set { this.SetValue(ItemsProperty, value); }
        }
    }
}
