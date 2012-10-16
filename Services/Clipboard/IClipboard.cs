using System;

namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Provides methods to place data on and retrieve data from the system Clipboard.
    /// </summary>
    public interface IClipboard
    {
        /// <summary>
        /// Removes all data from the Clipboard.
        /// </summary>
        void Clear();

        /// <summary>
        /// Retrieves data from the Clipboard.
        /// </summary>
        /// <returns>An object representing the Clipboard data or null if the Clipboard does not contain any data.</returns>
        object GetData();

        /// <summary>
        /// Adds data to the Clipboard.
        /// </summary>
        /// <param name="data">An object representing the data to add.</param>
        void SetData(object data);
    }
}
