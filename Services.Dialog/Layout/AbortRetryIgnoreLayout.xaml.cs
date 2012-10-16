using System.Windows;
using System.Windows.Controls;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Interaction logic for AbortRetryIgnoreLayout.xaml
    /// </summary>
    public partial class AbortRetryIgnoreLayout : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the AbortRetryIgnoreLayout class.
        /// </summary>
        public AbortRetryIgnoreLayout()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Executes the command when the Abort button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnAbortClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.Abort);
        }

        /// <summary>
        /// Executes the command when the Retry button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnRetryClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.Retry);
        }

        /// <summary>
        /// Executes the command when the Ignore button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnIgnoreClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.Ignore);
        }
    }
}

