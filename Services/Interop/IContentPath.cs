using System;

namespace Ijv.Redstone.Services.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContentPath
    {
        /// <summary>
        /// Gets the full path to this item.
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Gets the type information for this content.
        /// </summary>
        IContentType Type { get; }

        /// <summary>
        /// Gets the value true if this item contains children; otherwise false.
        /// </summary>
        bool HasChildren { get; }
    }
}
