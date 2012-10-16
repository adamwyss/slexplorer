
namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Represents a method that executes by a dialog result.
    /// </summary>
    /// <param name="result">The dialog button option selected by the user.</param>
    /// <param name="data">Data associated with the dialog.</param>
    public delegate void DialogCompletedDelegate(DialogResult result, object data);
}
