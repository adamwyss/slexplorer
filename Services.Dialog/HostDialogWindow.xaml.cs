using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Interaction logic for HostDialogWindow.xaml
    /// </summary>
    public partial class HostDialogWindow : ChildWindow
    {
        /// <summary>
        /// Creates an instance of the HostDialogWindow class.
        /// </summary>
        public HostDialogWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the callback that is called when the dialog completes.
        /// </summary>
        public DialogCompletedDelegate CompletedCallback { get; set; }

        /// <summary>
        /// Gets or sets the dialog result that indicates the user action.
        /// </summary>
        internal DialogResult Result { get; set; }

        /// <summary>
        /// Occurs when the dialog has closed and called the completed callback.
        /// </summary>
        /// <param name="e">The EventArgs that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (this.CompletedCallback != null)
            {
                this.CompletedCallback(this.Result, this.Tag);
            }
        }

        /// <summary>
        /// When the escape key is pressed, closes the dialog with a result of none.
        /// </summary>
        /// <param name="e">The KeyEventArgs that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.Escape:
                    this.Result = Services.DialogResult.None;
                    this.Close();
                    break;
            }
        }
    }
}

