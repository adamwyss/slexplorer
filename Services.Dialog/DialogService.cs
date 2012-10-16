using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Provides ui blocking alert and confirmation dialogs.
    /// </summary>
    [Export(typeof(IDialogService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DialogService : IDialogService
    {
        /// <summary>
        /// Gets or sets the title of the child window.
        /// </summary>
        public string Title
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Displays the specified content in a child window.
        /// </summary>
        /// <param name="content">The content to be displayed.</param>
        /// <param name="buttons">One of the <see cref="DialogButtons" /> values that specifies which buttons to display in the message box.</param>
        /// <param name="callback">The delegate that is called when the dialog box closes.</param>
        public void Show(object content, DialogButtons buttons, DialogCompletedDelegate callback)
        {
            Argument.IsNotNull("content", content);
            this.VerifyAccess();

            HostDialogWindow dialog = new HostDialogWindow();
            
            // store a reference to the content in the tag, so we can
            // return it to the caller in the callback method.
            dialog.Tag = content;

            UserControl uc = this.GetDialogLayout(buttons);
            if (uc != null)
            {
                uc.DataContext = content;
                dialog.Content = uc;
            }
            else
            {
                dialog.Content = content;
            }

            dialog.CompletedCallback = callback;
            dialog.Show();
        }

        /// <summary>
        /// Closes the current instance of the child window.
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// Gets the correct dialog layout for the provided button enum.
        /// </summary>
        /// <param name="buttons">An enumeration containing the possible button arrangements.</param>
        /// <returns>An usercontrol that provides the dialog layout.</returns>
        private UserControl GetDialogLayout(DialogButtons buttons)
        {
            switch (buttons)
            {
                case DialogButtons.Ok:
                    return new OkLayout();

                case DialogButtons.OkCancel:
                    return new OkCancelLayout();

                case DialogButtons.AbortRetryIgnore:
                    return new AbortRetryIgnoreLayout();

                case DialogButtons.YesNoCancel:
                    return new YesNoCancelLayout();

                case DialogButtons.YesNo:
                    return new YesNoLayout();

                case DialogButtons.RetryCancel:
                    return new RetryCancelLayout();

                default:
                    return null;
            }
        }

        /// <summary>
        /// Throws an exception if a cross thread call is being made.
        /// </summary>
        private void VerifyAccess()
        {
            if (!Deployment.Current.Dispatcher.CheckAccess())
            {
                throw new UnauthorizedAccessException("Invalid cross-thread access.");
            }
        }
    }
}
