using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Interaction logic for YesNoCancelLayout.xaml
    /// </summary>
    public partial class YesNoCancelLayout : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the YesNoCancelLayout class.
        /// </summary>
        public YesNoCancelLayout()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles specific keystrokes when they are pressed and relays them to the view model.
        /// </summary>
        /// <param name="e">The KeyEventArgs that contain the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.Escape:
                    break;
            }
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

        /// <summary>
        /// Executes the command when the cancel button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.Cancel);
        }
    }
}

