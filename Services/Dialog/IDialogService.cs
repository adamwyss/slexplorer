
namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Provides methods to display alert and confirmation dialogs.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Gets or sets the title of the child window.
        /// </summary>
        string Title { get; set;}

        /// <summary>
        /// Displays the specified content in a child window.
        /// </summary>
        /// <param name="content">The content to be displayed.</param>
        /// <param name="buttons">One of the <see cref="DialogButtons" /> values that specifies which buttons to display in the message box.</param>
        /// <param name="callback">The delegate that is called when the dialog box closes.</param>
        void Show(object content, DialogButtons buttons, DialogCompletedDelegate callback);

        /// <summary>
        /// Closes the child window.
        /// </summary>
        void Close();
    }
}
