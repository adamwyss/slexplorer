using System.Windows.Controls;

namespace Ijv.Redstone.Design
{
    /// <summary>
    /// Represents a view model implementation
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Gets the view that is attached to this viewmodel.
        /// </summary>
        Control View { get; }
    }
}
