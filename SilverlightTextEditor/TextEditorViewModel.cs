using System;
using System.ComponentModel.Composition;
using System.Windows;
using Ijv.Redstone.Design;
using Ijv.Redstone.Input;
using Ijv.Redstone.Services;

namespace Ijv.Redstone.TextEditor
{
    /// <summary />
    public class TextEditorViewModel : ViewModel, IFindAndReplaceTarget
    {
        #region Static Bindable Property Definitions

        /// <summary>
        /// Identifies the Text bindable property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Register<string>(
            "Text",
            typeof(TextEditorViewModel));

        /// <summary>
        /// Identifies the SelectedText bindable property.
        /// </summary>
        public static readonly BindableProperty SelectedTextProperty = BindableProperty.Register<string>(
            "SelectedText",
            typeof(TextEditorViewModel),
            string.Empty);

        /// <summary>
        /// Identifies the SelectionStart bindable property.
        /// </summary>
        public static readonly BindableProperty SelectionStartProperty = BindableProperty.Register<int>(
            "SelectionStart",
            typeof(TextEditorViewModel),
            WhenBindablePropertyChanged);

        /// <summary>
        /// Identifies the SelectionLength bindable property.
        /// </summary>
        public static readonly BindableProperty SelectionLengthProperty = BindableProperty.Register<int>(
            "SelectionLength",
            typeof(TextEditorViewModel),
            WhenBindablePropertyChanged);

        /// <summary>
        /// Identifies the IsStatusBarVisible bindable property.
        /// </summary>
        public static readonly BindableProperty IsStatusBarVisibleProperty = BindableProperty.Register<bool>(
            "IsStatusBarVisible",
            typeof(TextEditorViewModel),
            false);

        /// <summary>
        /// Identifies the IsWordWrapEnabled bindable property.
        /// </summary>
        public static readonly BindableProperty IsWordWrapEnabledProperty = BindableProperty.Register<bool>(
            "IsWordWrapEnabled",
            typeof(TextEditorViewModel),
            true);

        /// <summary>
        /// Routes the bindable properties 
        /// </summary>
        /// <param name="sender">The object that contains the bindable property that changed.</param>
        /// <param name="property">The property that changed.</param>
        /// <param name="oldValue">The previous value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        private static void WhenBindablePropertyChanged(BindableObject sender, BindableProperty property, object oldValue, object newValue)
        {
            TextEditorViewModel vm = sender as TextEditorViewModel;
            if (vm != null)
            {
                if (property == SelectionLengthProperty)
                {
                    vm.CutCommand.Refresh();
                    vm.CopyCommand.Refresh();
                }
                else if (property == TextProperty)
                {
                    vm.FindCommand.Refresh();
                    vm.ReplaceCommand.Refresh();
                    vm.SelectAllCommand.Refresh();
                }
            }
        }

        #endregion

        /// <summary>
        /// The settings service that is injected by MEF and is responsible for persisting the values of the decorated properties.
        /// </summary>
        [Import]
        public IUserSettingsService settingsSvc;

        /// <summary>
        /// The clipboard service that is injected by MEF and provides access to the clipboard.
        /// </summary>
        [Import]
        public IClipboard clipboardSvc;

        /// <summary>
        /// The dialog service that is injected by MEF and allows a dialog to be displayed.
        /// </summary>
        [Import]
        public IDialogService dialogSvc;

        /// <summary>
        /// Creates an instance of the TextEditorViewModel class.
        /// </summary>
        public TextEditorViewModel()
            : base(new Ijv.Redstone.TextEditor.Views.TextEditorView())
        {
            this.InitializeCommands();

            this.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec bibendum, lectus et semper egestas, enim felis eleifend urna, id consectetur ipsum leo ut justo. Maecenas ac nunc nec dolor consectetur consequat ut vulputate lacus. Ut ut neque in justo viverra dapibus. Morbi volutpat laoreet justo eu posuere. Integer et eros vitae leo lacinia fringilla at in odio. Curabitur ac porta dolor. Nullam metus elit, lobortis et pellentesque vitae, iaculis non metus. Sed vel mauris vel odio vehicula condimentum. Fusce lobortis, ante in gravida tempus, elit eros tristique libero, et ullamcorper neque metus sit amet tellus. Nulla consectetur placerat risus, in aliquet velit convallis eget. Duis nec lacus eget ligula placerat interdum. Cras at dolor arcu, vel congue sem. Duis venenatis justo nec nibh malesuada eget consectetur lorem porttitor. Praesent mollis sem vitae erat interdum dignissim."
                         + Environment.NewLine + "Phasellus auctor aliquam turpis, sit amet iaculis nulla dictum accumsan. Suspendisse potenti. Proin odio orci, pharetra eu sodales vitae, tempus nec ante. Integer ultricies, purus ac pharetra porttitor, sapien dui tempus turpis, tristique adipiscing diam tortor vel odio. Nam non ligula ipsum. Nullam eros est, hendrerit eu venenatis nec, imperdiet ac erat. Cras tortor dolor, porta nec consequat vitae, bibendum eu lectus. Pellentesque pellentesque bibendum accumsan. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum in eros eget nisi placerat venenatis. Duis mattis massa ac leo tincidunt sodales. Maecenas sit amet mi vitae libero luctus dapibus. Aenean scelerisque rhoncus malesuada. Curabitur sed purus sem, eu posuere urna. Maecenas consequat pulvinar justo ac elementum."
                         + Environment.NewLine + "Praesent dapibus, ligula et ultrices rhoncus, augue dui feugiat libero, in luctus ipsum tellus luctus turpis. Nulla facilisi. Nulla ornare nisi vitae tellus placerat in ultrices turpis blandit. Aenean sed nisi lectus, sed adipiscing ante. Nulla facilisi. Nam at ligula sit amet mauris sodales cursus id quis velit. Mauris dolor orci, consequat quis vulputate ut, accumsan nec nulla. Nunc rutrum adipiscing urna, quis vehicula tellus blandit sed. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Etiam non felis elit. Nullam vestibulum, nisi vel tempor tincidunt, eros nisl rhoncus lorem, eu bibendum risus nulla a nulla. Nam quam nibh, molestie ac gravida eget, suscipit id nunc."
                         + Environment.NewLine + "Duis feugiat commodo nunc ut aliquam. Morbi in neque sed velit posuere hendrerit. Praesent ultrices risus ut lorem venenatis mattis at quis erat. Aenean augue purus, sagittis et luctus id, fringilla in nisi. Suspendisse et nisl sed orci hendrerit hendrerit. Nam interdum augue sit amet lorem tristique condimentum. Morbi tristique libero a erat dignissim posuere. Integer ante elit, vulputate eu euismod at, commodo vitae eros. Nulla diam nisi, sodales in suscipit et, convallis et est. Proin a odio at est malesuada feugiat id ut nibh. Suspendisse nec arcu non nunc fermentum euismod vel sit amet metus. In diam justo, facilisis sed bibendum quis, elementum non purus. Cras dolor tortor, volutpat eu sodales nec, sollicitudin et mi. Nullam sit amet risus libero, non vestibulum massa. Integer et lacus non orci auctor scelerisque."
                         + Environment.NewLine + "Suspendisse potenti. Fusce felis magna, luctus vitae laoreet vel, lobortis vel dolor. Nam feugiat, tortor ac pulvinar condimentum, nisl nibh cursus orci, in lobortis lacus ante lacinia tellus. Aliquam quam metus, iaculis at suscipit eget, luctus eget justo. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Duis at magna neque, vitae tincidunt elit. Maecenas tincidunt tempus dignissim. Praesent magna risus, convallis eu laoreet sit amet, convallis ac magna. Aliquam convallis commodo erat sit amet semper. Sed ut diam vel leo porta ornare. Donec eu urna lorem, quis dictum tellus. Aliquam nulla odio, interdum sed venenatis nec, sagittis fermentum sem. Mauris eget urna est, non scelerisque risus.";

            CompositionInitializer.SatisfyImports(this);
            this.settingsSvc.Load(this);

            Application.Current.Exit += delegate(object sender, EventArgs e)
                {
                    this.settingsSvc.Save(this);
                };

            IApplicationSettingsService appSettings = ServiceLocator.Current.GetService<IApplicationSettingsService>();
        }

        /// <summary>
        /// Gets or sets the text that is being edited.
        /// </summary>
        public string Text
        {
            get { return this.GetValue<string>(TextProperty); }
            set { this.SetValue<string>(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text that is currently selected.
        /// </summary>
        public string SelectedText
        {
            get { return this.GetValue<string>(SelectedTextProperty); }
            set { this.SetValue<string>(SelectedTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the starting point of the current selection or the current insertion point of the selection is zero.
        /// </summary>
        public int SelectionStart
        {
            get { return this.GetValue<int>(SelectionStartProperty); }
            set { this.SetValue<int>(SelectionStartProperty, value); }
        }

        /// <summary>
        /// Gets or sets the length of the current selection.
        /// </summary>
        public int SelectionLength
        {
            get { return this.GetValue<int>(SelectionLengthProperty); }
            set { this.SetValue<int>(SelectionLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the status bar is visible.
        /// </summary>
        [Persistable]
        public bool IsStatusBarVisible
        {
            get { return this.GetValue<bool>(IsStatusBarVisibleProperty); }
            set { this.SetValue<bool>(IsStatusBarVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether word wrap is enabled for the text
        /// </summary>
        [Persistable]
        public bool IsWordWrapEnabled
        {
            get { return this.GetValue<bool>(IsWordWrapEnabledProperty); }
            set { this.SetValue<bool>(IsWordWrapEnabledProperty, value); }
        }
        

        public DelegateCommand CutCommand { get; internal set; }
        public DelegateCommand CopyCommand { get; internal set; }
        public DelegateCommand PasteCommand { get; internal set; }
        
        public DelegateCommand FindCommand { get; internal set; }
        public DelegateCommand ReplaceCommand { get; internal set; }
        public DelegateCommand SelectAllCommand { get; internal set; }

        private void InitializeCommands()
        {
            this.CutCommand = new DelegateCommand(this.CutExecute, this.CutCanExecute);
            this.CopyCommand = new DelegateCommand(this.CopyExecute, this.CopyCanExecute);
            this.PasteCommand = new DelegateCommand(this.PasteExecute, this.PasteCanExecute);

            this.FindCommand = new DelegateCommand(this.FindExecute, this.FindCanExecute);
            this.ReplaceCommand = new DelegateCommand(this.ReplaceExecute, this.ReplaceCanExecute);
            this.SelectAllCommand = new DelegateCommand(this.SelectAllExecute, this.SelectAllCanExecute);
        }

        private bool CutCanExecute()
        {
            return this.clipboardSvc != null && this.SelectionLength > 0;
        }

        private void CutExecute()
        {
            this.clipboardSvc.SetData(this.SelectedText);
            this.SelectedText = string.Empty;
        }

        private bool CopyCanExecute()
        {
            return this.clipboardSvc != null && this.SelectionLength > 0;
        }

        private void CopyExecute()
        {
            this.clipboardSvc.SetData(this.SelectedText);
        }

        private bool PasteCanExecute()
        {
            object data = this.clipboardSvc.GetData();
            return this.clipboardSvc != null && data != null && data is string;
        }

        private void PasteExecute()
        {
            this.SelectedText = (string)this.clipboardSvc.GetData();
        }

        private bool FindCanExecute()
        {
            return this.Text.Length > 0;
        }

        private void FindExecute()
        {
            FindAndReplaceViewModel vm = new FindAndReplaceViewModel(this);
            this.dialogSvc.Show(vm.View, DialogButtons.None, null);
        }

        private bool ReplaceCanExecute()
        {
            return this.Text.Length > 0;
        }

        private void ReplaceExecute()
        {
            FindAndReplaceViewModel vm = new FindAndReplaceViewModel(this);
            this.dialogSvc.Show(vm.View, DialogButtons.None, null);
        }

        private bool SelectAllCanExecute()
        {
            return this.Text.Length > 0;
        }

        private void SelectAllExecute()
        {
            this.SelectionStart = 0;
            this.SelectionLength = this.Text.Length;
        }
    }
}
