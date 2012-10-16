using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Interaction logic for OkDialogLayout.xaml
    /// </summary>
    public partial class OkLayout : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the OkLayout class.
        /// </summary>
        public OkLayout()
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
    }
}

