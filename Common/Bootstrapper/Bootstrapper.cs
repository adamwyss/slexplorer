using System.Windows;
using Ijv.Redstone.Design;

namespace Ijv.Redstone
{
    /// <summary />
    public class Bootstrapper
    {
        /// <summary />
        public void Run<T>() where T : IViewModel
        {
            Application app = Application.Current;

            MainPageViewModel vm = new MainPageViewModel(app, typeof(T));
        }
    }
}
