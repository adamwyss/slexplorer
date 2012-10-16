using System.Windows;
using Ijv.Redstone.Explorer;

namespace Ijv.Redstone
{
    /// <summary>
    /// The interaction logic for ExplorerApplication.xaml
    /// </summary>
    public partial class ExplorerApplication : Application
    {
        /// <summary>
        /// Initializes a new instance of the ExplorerApplication class.
        /// </summary>
        public ExplorerApplication()
        {
            new Bootstrapper().Run<StandaloneHostViewModel>();
            this.InitializeComponent();
        }
    }
}
