using System.Windows;
using Ijv.Redstone.TextEditor.Views;
using Ijv.Redstone.TextEditor;

namespace Ijv.Redstone
{
    /// <summary>
    /// The interaction logic for TextEditorApplication.xaml
    /// </summary>
    public partial class TextEditorApplication : Application
    {
        /// <summary>
        /// Initializes a new instance of the TextEditorApplication class.
        /// </summary>
        public TextEditorApplication()
        {
            new Bootstrapper().Run<TextEditorViewModel>();
            InitializeComponent();
        }
    }
}
