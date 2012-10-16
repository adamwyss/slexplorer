using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Interaction logic for OkCancelLayout.xaml
    /// </summary>
    public partial class OkCancelLayout : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the OkCancelLayout class.
        /// </summary>
        public OkCancelLayout()
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
                case Key.Enter:
                    break;

                case Key.Escape:
                    break;
            }
        }

        /// <summary>
        /// Executes the command when the Ok button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnOkClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.Ok);
        }

        /// <summary>
        /// Executes the command when the Cancel button is clicked.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The RoutedEventArgs that contains the event data.</param>
        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogHelper.CloseDialog(this, DialogResult.Cancel);
        }
    }
}

