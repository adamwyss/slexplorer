using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;

namespace Ijv.Redstone.TextEditor.Views
{
    /// <summary>
    /// The interaction logic for TextEditorView.xaml
    /// </summary>
    public partial class TextEditorView : UserControl
    {
        /// <summary>
        /// When true, the selection information is currently being updated and we are mostly likely getting 
        /// an handler of the updated event, so whe should ignore it.
        /// </summary>
        private bool selectionUpdateBusy = false;

        /// <summary>
        /// Initializes a new instance of the TextEditorView class.
        /// </summary>
        public TextEditorView()
        {
            this.InitializeComponent();

            this.Loaded += this.TextEditorView_Loaded;
            this.Unloaded += this.TextEditorView_Unloaded;
        }

        /// <summary />
        private void TextEditorView_Unloaded(object sender, RoutedEventArgs e)
        {
            this.TextArea.SelectionChanged -= this.WhenTextBoxSelectionChanged;
            ((TextEditorViewModel)this.DataContext).PropertyChanged -= this.WhenViewModelPropertyChanged;
        }

        /// <summary />
        private void TextEditorView_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextArea.SelectionChanged += this.WhenTextBoxSelectionChanged;
            ((TextEditorViewModel)this.DataContext).PropertyChanged += this.WhenViewModelPropertyChanged;
        }

        /// <summary />
        private void WhenViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.selectionUpdateBusy)
            {
                return;
            }

            this.selectionUpdateBusy = true;
            try
            {
                TextEditorViewModel vm = (TextEditorViewModel)this.DataContext;

                switch (e.PropertyName)
                {
                    case "SelectedText":
                        this.TextArea.SelectedText = vm.SelectedText;
                        break;

                    case "SelectionStart":
                        this.TextArea.SelectionStart = vm.SelectionStart;
                        break;

                    case "SelectionLength":
                        this.TextArea.SelectionLength = vm.SelectionLength;
                        break;
                }
            }
            finally
            {
                this.selectionUpdateBusy = false;
            }
        }

        /// <summary />
        private void WhenTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.selectionUpdateBusy)
            {
                return;
            }

            this.selectionUpdateBusy = true;
            try
            {
                TextBox textbox = (TextBox)sender;
                TextEditorViewModel vm = (TextEditorViewModel)this.DataContext;

                vm.SelectedText = textbox.SelectedText;
                vm.SelectionStart = textbox.SelectionStart;
                vm.SelectionLength = textbox.SelectionLength;
            }
            finally
            {
                this.selectionUpdateBusy = false;
            }
        }
    }
}
