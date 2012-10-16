
namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Specifies constants defining which buttons to display on a dialog box.
    /// </summary>
    public enum DialogButtons
    {
        /// <summary>
        /// The message box contains no buttons
        /// </summary>
        None,

        /// <summary>
        /// The message box contains an OK button.
        /// </summary>
        Ok,

        /// <summary>
        /// The message box contains OK and Cancel buttons.
        /// </summary>
        OkCancel,

        /// <summary>
        /// The message box contains Abort, Retry, and Ignore buttons.
        /// </summary>
        AbortRetryIgnore,

        /// <summary>
        /// The message box contains Yes, No, and Cancel buttons.
        /// </summary>
        YesNoCancel,

        /// <summary>
        /// The message box contains Yes and No buttons.
        /// </summary>
        YesNo,

        /// <summary>
        /// The message box contains Retry and Cancel buttons.
        /// </summary>
        RetryCancel
    }
}
