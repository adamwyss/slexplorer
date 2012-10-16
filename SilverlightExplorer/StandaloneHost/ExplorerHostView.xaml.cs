using System.Windows.Controls;
using System.Windows;
using Ijv.Redstone.Controls;

namespace Ijv.Redstone.Explorer.Views
{
    /// <summary>
    /// The interaction logic for StandaloneHostView.xaml
    /// </summary>
    public partial class StandaloneHostView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the ExplorerHostView class.
        /// </summary>
        public StandaloneHostView()
        {
            this.InitializeComponent();

            this.Loaded += this.WhenControlLoadedInitializeBindings;
        }

        /// <summary />
        private void WhenControlLoadedInitializeBindings(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.WhenControlLoadedInitializeBindings;

            // binds properties to the grid definination (not supported in silverlight)
            new GridDefinitionBindingHelper(this.HACK_BINDING_AddressColumnWidth,
                                          this.HACK_BINDING_AddressColumnSplitter,
                                          this.HACK_BINDING_AddressColumnGridSplitter,
                                          (StandaloneHostViewModel)this.DataContext,
                                          StandaloneHostViewModel.SearchColumnWidthProperty,
                                          null);
        }
    }
}
