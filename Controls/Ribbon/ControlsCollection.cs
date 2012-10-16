using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a collection of framework elements in a ribbon.
    /// </summary>
    public class ControlsCollection : ObservableCollection<FrameworkElement>
    {
        /// <summary>
        /// Initializes a new instance of the ControlsCollection class.
        /// </summary>
        public ControlsCollection()
        {
            this.CollectionChanged += this.ControlsCollection_CollectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs that contains the event data.</param>
        private void ControlsCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    foreach (object item in e.NewItems)
                    {
                        MenuButton menubutton = item as MenuButton;
                        if (menubutton != null)
                        {
                            menubutton.Style = ResourceLocator.Get<Style>(ResourceLocator.MenuButton, "SmallIconWithTextStyle");
                        }

                        Button button = item as Button;
                        if (button != null)
                        {
                            string key = button.Tag == null ? "LargeIconWithTextStyle" : "SmallIconWithTextStyle";

                            button.Style = ResourceLocator.Get<Style>(ResourceLocator.Button, key);
 
                            if (button.Content is RibbonLabel)
                            {
                                string key2 = button.Tag == null ? "LargeIconWithTextDataTemplate" : "SmallIconWithTextDataTemplate";

                                DataTemplate dt = ResourceLocator.Get<DataTemplate>(ResourceLocator.Button, key2);
                                if (dt != null)
                                {
                                    button.ContentTemplate = dt;
                                }
                            }
                        }


                        CheckBox checkbox = item as CheckBox;
                        if (checkbox != null)
                        {
                            checkbox.Style = ResourceLocator.Get<Style>(ResourceLocator.CheckBox, "RibbonCheckBoxStyle");
                        }

                    }

                    break;
            }
        }
    }
}