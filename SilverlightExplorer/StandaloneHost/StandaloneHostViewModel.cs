using Ijv.Redstone.Design;
using Ijv.Redstone.Services;
using Ijv.Redstone.Explorer.Views;

namespace Ijv.Redstone.Explorer
{
    public class StandaloneHostViewModel : ViewModel
    {
        #region Static Bindable Property Definitions

        /// <summary>
        /// Identifies the AddressColumnWidth bindable property.
        /// </summary>
        public static readonly BindableProperty SearchColumnWidthProperty = BindableProperty.Register<double>(
            "SearchColumnWidth",
            typeof(StandaloneHostViewModel),
            225.0);

        #endregion

        /// <summary>
        /// Creates an instance of the ExplorerHostViewModel class.
        /// </summary>
        public StandaloneHostViewModel()
            : base(new StandaloneHostView())
        {
            this.ContentExplorer = new ExplorerViewModel();
        }

        #region Public Properties

        /// <summary>
        /// Gets the hosted explorer view model.
        /// </summary>
        public ExplorerViewModel ContentExplorer { get; private set; }

        /// <summary>
        /// Gets or sets the searchbox column width.
        /// </summary>
        [Persistable]
        public double SearchColumnWidth
        {
            get { return this.GetValue<double>(SearchColumnWidthProperty); }
            set { this.SetValue<double>(SearchColumnWidthProperty, value); }
        }

        #endregion
    }
}
