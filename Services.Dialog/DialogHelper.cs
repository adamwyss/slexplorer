using System.Windows;
using System.Windows.Media;

namespace Ijv.Redstone.Services.Dialog
{
    /// <summary>
    /// Provides utility methods that perform common tasks involving the dialog service.
    /// </summary>
    public static class DialogHelper
    {
        /// <summary>
        /// Closes the current dialog.
        /// </summary>
        /// <param name="child">An ui element that is hosted by the dialog service.</param>
        /// <param name="result">The desired dialog result</param>
        public static void CloseDialog(DependencyObject element, DialogResult result)
        {
            // walk visual tree looking for the richtextbox.
            for (DependencyObject obj = element; obj != null; obj = VisualTreeHelper.GetParent(obj))
            {
                HostDialogWindow host = obj as HostDialogWindow;
                if (host != null)
                {
                    host.Result = result;
                    host.Close();
                    break;
                }
            }
        }
    }
}
