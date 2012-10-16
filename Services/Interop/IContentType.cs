
namespace Ijv.Redstone.Services.Data
{
    /// <summary>
    /// Represents a type defination for a piece of content.
    /// </summary>
    public interface IContentType
    {
        /// <summary>
        /// Gets the name of this type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value true if content of this type can have children; otherwise false.
        /// </summary>
        bool CanContainChildren { get; }
    }
}
