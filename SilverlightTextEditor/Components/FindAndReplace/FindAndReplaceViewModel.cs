using Ijv.Redstone.Design;
using Ijv.Redstone.TextEditor.Views;
using Ijv.Redstone.Input;

namespace Ijv.Redstone.TextEditor
{
    /// <summary>
    /// The view model for the find view class.
    /// </summary>
    public class FindAndReplaceViewModel : ViewModel
    {
        #region Static Bindable Property Definitions

        /// <summary>
        /// Identifies the SelectedObject bindable property.
        /// </summary>
        public static readonly BindableProperty FindTextProperty = BindableProperty.Register<string>(
            "FindText",
            typeof(FindAndReplaceViewModel),
            WhenBindablePropertyChanged);

        /// <summary>
        /// Identifies the SelectedObject bindable property.
        /// </summary>
        public static readonly BindableProperty ReplaceTextProperty = BindableProperty.Register<string>(
            "ReplaceText",
            typeof(FindAndReplaceViewModel));

        /// <summary>
        /// Routes the bindable properties 
        /// </summary>
        /// <param name="sender">The object that contains the bindable property that changed.</param>
        /// <param name="property">The property that changed.</param>
        /// <param name="oldValue">The previous value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        private static void WhenBindablePropertyChanged(BindableObject sender, BindableProperty property, object oldValue, object newValue)
        {
            FindAndReplaceViewModel vm = sender as FindAndReplaceViewModel;
            if (vm != null)
            {
                if (property == FindTextProperty)
                {
                    vm.FindNextCommand.Refresh();
                    vm.ReplaceCommand.Refresh();
                    vm.ReplaceAllCommand.Refresh();
                }
            }
        }

        #endregion

        /// <summary>
        /// The text editor that we are searching.
        /// </summary>
        private IFindAndReplaceTarget textEditor;

        /// <summary>
        /// Initializes a new instance of the FindAndReplaceViewModel class.
        /// </summary>
        public FindAndReplaceViewModel(IFindAndReplaceTarget target)
            : base(new FindAndReplaceView())
        {
            // preconditions

            Argument.IsNotNull("target", target);

            // implementation

            this.textEditor = target;

            this.InitializeCommands();

            this.FindText = this.textEditor.SelectedText;
        }

        /// <summary>
        /// Gets or sets the text that is being searched for.
        /// </summary>
        public string FindText
        {
            get { return this.GetValue<string>(FindTextProperty); }
            set { this.SetValue<string>(FindTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text taht will replace the current selection.
        /// </summary>
        public string ReplaceText
        {
            get { return this.GetValue<string>(ReplaceTextProperty); }
            set { this.SetValue<string>(ReplaceTextProperty, value); }
        }

        /// <summary>
        /// Gets the command that finds the next word
        /// </summary>
        public DelegateCommand FindNextCommand { get; private set;}

        /// <summary>
        /// Gets the command that replaces the current selection with the specified value.
        /// </summary>
        public DelegateCommand ReplaceCommand { get; private set;}

        /// <summary>
        /// Gets the command that replaces all instances of the current selection with the specified value.
        /// </summary>
        public DelegateCommand ReplaceAllCommand { get; private set; }

        private void InitializeCommands()
        {
            this.FindNextCommand = new DelegateCommand(this.FindNextExecute, this.FindNextCanExecute);
            this.ReplaceCommand = new DelegateCommand(this.ReplaceExecute, this.ReplaceCanExecute);
            this.ReplaceAllCommand = new DelegateCommand(this.ReplaceAllExecute, this.ReplaceAllCanExecute);
        }

        private bool FindNextCanExecute()
        {
            return this.FindText.Length > 0;
        }

        private void FindNextExecute()
        {
            int index = this.textEditor.Text.IndexOf(this.FindText, this.textEditor.SelectionStart);

            if (index >= 0)
            {
                this.textEditor.SelectionStart = index;
                this.textEditor.SelectionLength = this.FindText.Length;
            }
            else
            {
                // TODO: notify that no more data could be found.
            }
        }

        private bool ReplaceCanExecute()
        {
            return this.FindText.Length > 0;
        }

        private void ReplaceExecute()
        {
            if (this.textEditor.SelectionLength > 0)
            {
                this.textEditor.SelectedText = this.ReplaceText ?? string.Empty;
            }
            else
            {
                // replace was executed, but nothing was selected, we will perform the
                // seach instead.
                this.FindNextExecute();
            }
        }

        private bool ReplaceAllCanExecute()
        {
            return this.FindText.Length > 0;
        }

        private void ReplaceAllExecute()
        {
        }
    }
}
