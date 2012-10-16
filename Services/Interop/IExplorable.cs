using System.Collections.Generic;

namespace Ijv.Redstone.Services.Data
{
    public delegate void GetChildrenCallback(IEnumerable<IContentPath> children, int childCount, object userState);

    public interface IExplorable
    {
        void GetChildren(IContentPath parent, GetChildrenCallback callback, object userState);

        bool Copy(IContentPath[] sources, IContentPath destination);
        bool Move(IContentPath[] sources, IContentPath destination);
        bool Delete(params IContentPath[] items);
    }
}
