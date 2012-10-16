using Ijv.Redstone.Design;
using Ijv.Redstone.Input;
using Ijv.Redstone.Explorer.Views;

namespace Ijv.Redstone.Explorer
{
    /// <summary>
    /// Represents a view model that hosts the explorer.
    /// </summary>
    public class OpenExplorerHostViewModel : ViewModel
    {
        /// <summary>
        /// Creates an instance of the OpenExplorerHostViewModel class.
        /// </summary>
        public OpenExplorerHostViewModel()
            : base(new OpenHostView())
        {
            this.Explorer = new ExplorerViewModel();
            this.Explorer.IsDetailsVisible = false;

            this.AcceptCommand = new DelegateCommand(this.AcceptExecute, this.AcceptCanExecute);
            this.CancelCommand = new DelegateCommand(this.CancelExecute);
        }

        /// <summary>
        /// Gets the dialog title
        /// </summary>
        public string DialogTitle
        {
            get { return "Open"; }
        }

        /// <summary>
        /// Gets the hosted explorer view model.
        /// </summary>
        public ExplorerViewModel Explorer { get; private set; }

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        private bool AcceptCanExecute(object parameter)
        {
            return true;
        }

        private void AcceptExecute(object parameter)
        {
        }

        private void CancelExecute(object parameter)
        {
        }
    }
}
