using System.Windows;
using System.Windows.Controls;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Interaction logic for YesNoLayout.xaml
    /// </summary>
    public partial class YesNoLayout : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the YesNoLayout class.
        /// </summary>
        public YesNoLayout()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Executes the command when the Yes button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnYesClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.Yes);
        }

        /// <summary>
        /// Executes the command when the No button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnNoClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.No);
        }
    }
}

