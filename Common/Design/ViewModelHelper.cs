using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ijv.Redstone.Design
{
    public static class ViewModelHelper
    {
        public static void InitializeViewModel(UserControl view)
        {
            ViewModel viewModel = view.DataContext as ViewModel;
            if (viewModel != null)
            {
                viewModel.View = view;

                // perform view model initialization code here.
            }
        }
    }
}
