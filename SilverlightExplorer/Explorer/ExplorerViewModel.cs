using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Ijv.Redstone.Design;
using Ijv.Redstone.Explorer.Views;
using Ijv.Redstone.Input;
using Ijv.Redstone.Services;
using Ijv.Redstone.Services.Data;

namespace Ijv.Redstone.Explorer
{
    /// <summary>
    /// Represents a view model of the content explorer.
    /// </summary>
    public class ExplorerViewModel : ViewModel
    {
        #region Static Bindable Property Definitions

        /// <summary>
        /// Identifies the SelectedObject bindable property.
        /// </summary>
        public static readonly BindableProperty SelectedObjectProperty = BindableProperty.Register<object>(
            "SelectedObject",
            typeof(ExplorerViewModel),
            true);

        /// <summary>
        /// Identifies the IsNavigationVisibleProperty bindable property.
        /// </summary>
        public static readonly BindableProperty IsNavigationVisibleProperty = BindableProperty.Register<bool>(
            "IsNavigationVisible",
            typeof(ExplorerViewModel),
            true);

        /// <summary>
        /// Identifies the NavigationColumnWidth bindable property.
        /// </summary>
        public static readonly BindableProperty NavigationColumnWidthProperty = BindableProperty.Register<double>(
            "NavigationColumnWidth",
            typeof(ExplorerViewModel),
            double.NaN);

        /// <summary>
        /// Identifies the IsDetailsVisibleProperty bindable property.
        /// </summary>
        public static readonly BindableProperty IsDetailsVisibleProperty = BindableProperty.Register<bool>(
            "IsDetailsVisible",
            typeof(ExplorerViewModel),
            true);

        /// <summary>
        /// Identifies the DetailRowHeight bindable property.
        /// </summary>
        public static readonly BindableProperty DetailsRowHeightProperty = BindableProperty.Register<double>(
            "DetailsRowHeight",
            typeof(ExplorerViewModel),
            double.NaN);

        /// <summary>
        /// Identifies the IsPreviewPaneVisible bindable property.
        /// </summary>
        public static readonly BindableProperty IsPreviewVisibleProperty = BindableProperty.Register<bool>(
            "IsPreviewVisible",
            typeof(ExplorerViewModel),
            false);

        /// <summary>
        /// Identifies the PreviewPaneWidth bindable property.
        /// </summary>
        public static readonly BindableProperty PreviewColumnWidthProperty = BindableProperty.Register<double>(
            "PreviewColumnWidth",
            typeof(ExplorerViewModel),
            175.0);

        /// <summary>
        /// Identifies the ViewType bindable property.
        /// </summary>
        public static readonly BindableProperty ViewTypeProperty = BindableProperty.Register<ExplorerViews>(
            "ViewType",
            typeof(ExplorerViewModel),
            ExplorerViews.Details);

        /// <summary>
        /// Identifies the ViewScale bindable property.
        /// </summary>
        public static readonly BindableProperty ViewScaleProperty = BindableProperty.Register<double>(
            "ViewScale",
            typeof(ExplorerViewModel));

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
        /// Creates an instance of the ExplorerViewModel class.
        /// </summary>
        public ExplorerViewModel()
            : base(new ExplorerView())
        {
            this.NavigationItems = new ObservableCollection<object>();
            
            this.ViewType = ExplorerViews.LargeIcons;
            this.ViewScale = 1.5;

            this.RegisterOrganizeCommands();

            CompositionInitializer.SatisfyImports(this);
            this.settingsSvc.Load(this);

            Application.Current.Exit += delegate(object sender, EventArgs e)
            {
                this.settingsSvc.Save(this);
            };
        }

        /// <summary>
        /// Gets 
        /// </summary>
        public ObservableCollection<object> NavigationItems { get; private set; }

        #region Public Properties

        /// <summary>
        /// Gets the object that the explorer currently had selected.
        /// </summary>
        public object SelectedObject
        {
            get { return this.GetValue<object>(SelectedObjectProperty); }
            private set { this.SetValue<object>(SelectedObjectProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether the navigation pane is visible.
        /// </summary>
        [Persistable]
        public bool IsNavigationVisible
        {
            get { return this.GetValue<bool>(IsNavigationVisibleProperty); }
            set { this.SetValue<bool>(IsNavigationVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the treeview column width.
        /// </summary>
        [Persistable]
        public double NavigationColumnWidth
        {
            get { return this.GetValue<double>(NavigationColumnWidthProperty); }
            set { this.SetValue<double>(NavigationColumnWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether the details pane is visible.
        /// </summary>
        [Persistable]
        public bool IsDetailsVisible
        {
            get { return this.GetValue<bool>(IsDetailsVisibleProperty); }
            set { this.SetValue<bool>(IsDetailsVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the detail row height.
        /// </summary>
        [Persistable]
        public double DetailsRowHeight
        {
            get { return this.GetValue<double>(DetailsRowHeightProperty); }
            set { this.SetValue<double>(DetailsRowHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets whether the preview pane is visible.
        /// </summary>
        [Persistable]
        public bool IsPreviewVisible
        {
            get { return this.GetValue<bool>(IsPreviewVisibleProperty); }
            set { this.SetValue<bool>(IsPreviewVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the preview pane column width.
        /// </summary>
        [Persistable]
        public double PreviewColumnWidth
        {
            get { return this.GetValue<double>(PreviewColumnWidthProperty); }
            set { this.SetValue<double>(PreviewColumnWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected view type.
        /// </summary>
        [Persistable]
        public ExplorerViews ViewType
        {
            get { return this.GetValue<ExplorerViews>(ViewTypeProperty); }
            set { this.SetValue<ExplorerViews>(ViewTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected view scale.
        /// </summary>
        [Persistable]
        public double ViewScale
        {
            get { return this.GetValue<double>(ViewScaleProperty); }
            set { this.SetValue<double>(ViewScaleProperty, value); }
        }

        #endregion

        #region Menu Commands

        public DelegateCommand CutCommand { get; private set; }
        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }
        public DelegateCommand UndoCommand { get; private set; }
        public DelegateCommand RedoCommand { get; private set; }
        public DelegateCommand SelectAllCommand { get; private set; }
        public DelegateCommand OptionsCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand RenameCommand { get; private set; }
        public DelegateCommand PropertiesCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        public DelegateCommand NewFolderCommand { get; private set; }

        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand OpenWithCommand { get; private set; }
        public DelegateCommand ShareWithCommand { get; private set; }

        public DelegateCommand FileVersionCommand { get; private set; }

        public void RegisterOrganizeCommands()
        {
            this.CutCommand = new DelegateCommand(this.CutExecute, this.CutCanExecute);
            this.CopyCommand = new DelegateCommand(this.CopyExecute, this.CopyCanExecute);
            this.PasteCommand = new DelegateCommand(this.PasteExecute, this.PasteCanExecute);
            this.UndoCommand = new DelegateCommand(this.UndoExecute, this.UndoCanExecute);
            this.RedoCommand = new DelegateCommand(this.RedoExecute, this.RedoCanExecute);
            this.SelectAllCommand = new DelegateCommand(this.SelectAllExecute, this.SelectAllCanExecute);
            this.OptionsCommand = new DelegateCommand(this.OptionsExecute, this.OptionsCanExecute);
            this.DeleteCommand = new DelegateCommand(this.DeleteExecute, this.DeleteCanExecute);
            this.RenameCommand = new DelegateCommand(this.RenameExecute, this.RenameCanExecute);
            this.PropertiesCommand = new DelegateCommand(this.PropertiesExecute, this.PropertiesCanExecute);
            this.CloseCommand = new DelegateCommand(this.CloseExecute, this.CloseCanExecute);

            this.NewFolderCommand = new DelegateCommand(this.NewFolderExecute, this.NewFolderCanExecute);

            this.OpenCommand = new DelegateCommand(this.OpenExecute, this.OpenCanExecute);
            this.OpenWithCommand = new DelegateCommand(this.OpenWithExecute, this.OpenWithCanExecute);
            this.ShareWithCommand = new DelegateCommand(this.ShareWithExecute, this.ShareWithCanExecute);

            this.FileVersionCommand = new DelegateCommand(this.FileVersionExecute, this.FileVersionCanExecute);
        }

        public bool CutCanExecute(object parameter)
        {
            return this.clipboardSvc != null && this.SelectedObject != null;
        }

        public void CutExecute(object parameter)
        {
            this.clipboardSvc.SetData(this.SelectedObject);

            // TODO: flag the operation as a cut operation.
        }

        public bool CopyCanExecute(object parameter)
        {
            return this.clipboardSvc != null && this.SelectedObject != null;
        }

        public void CopyExecute(object parameter)
        {
            this.clipboardSvc.SetData(this.SelectedObject);
        }

        public bool PasteCanExecute(object parameter)
        {
            return this.clipboardSvc != null &&
                   this.clipboardSvc.GetData() is IContentPath;
        }

        public void PasteExecute(object parameter)
        {
            object data = this.clipboardSvc.GetData();
        }

        public bool UndoCanExecute(object parameter)
        {
            return false;
        }

        public void UndoExecute(object parameter)
        {
            // not implemented - this may need to be removed.
        }

        public bool RedoCanExecute(object parameter)
        {
            return false;
        }

        public void RedoExecute(object parameter)
        {
            // not implemented - this may need to be removed.
        }

        public bool SelectAllCanExecute(object parameter)
        {
            return false;
        }

        public void SelectAllExecute(object parameter)
        {
            // TODO: select all items in main pane
        }

        public bool OptionsCanExecute(object parameter)
        {
            return false;
        }

        public void OptionsExecute(object parameter)
        {
//            IDialogService dialogSvc = this.SiteContainer.GetServiceSafe<IDialogService>();
//            dialogSvc.Show("TODO: Settings and options Dialog", DialogButtons.OkCancel, null);
        }

        public bool DeleteCanExecute(object parameter)
        {
            return false; //this.SelectedObject is IContentInfo;
        }

        public void DeleteExecute(object parameter)
        {
//            DialogCompletedDelegate deleteCallback = delegate(DialogResult result, object data)
//                {
//                    if (result == DialogResult.Ok)
//                    {
//                        IContentInfo info = data as IContentInfo;
//                        if (info != null)
//                        {
//                            IExplorable svc = info.Origin.QueryForService<IExplorable>();
//                            if (svc != null)
//                            {
//                                svc.Delete(info);
//                            }
//                        }
//                    }
//                };

//            IDialogService dialogSvc = this.SiteContainer.GetServiceSafe<IDialogService>();
//            dialogSvc.Show((WarningMessage)"Are you sure you want to delete this item?", DialogButtons.YesNo, deleteCallback);

            // TODO: fix layout of dialog:
            // [RecycleBin Icon] Are you sure you want to move this file to the Recycle Bin?
            // [Large File Icon] [Document Name]
            //                   Type: [Document Type] 
            //                   Size: [##] bytes
            //                   Date modified: [Date/Time]
        }

        public bool RenameCanExecute(object parameter)
        {
            return false;
        }

        public void RenameExecute(object parameter)
        {
            // TODO: figure out how to get the selected item to rename.
        }

        public bool PropertiesCanExecute(object parameter)
        {
            return true;
        }

        public void PropertiesExecute(object parameter)
        {
//            PropertiesViewModel vm = new PropertiesViewModel();

//            IDialogService dialogSvc = this.SiteContainer.GetServiceSafe<IDialogService>();
//            dialogSvc.Show(vm);
        }

        public bool CloseCanExecute(object parameter)
        {
            return true;
        }

        public void CloseExecute(object parameter)
        {
//            IHostService hostSvc = this.SiteContainer.GetServiceSafe<IHostService>();
//            hostSvc.Close();
        }

        public bool NewFolderCanExecute(object parameter)
        {
            return true;
        }

        public void NewFolderExecute(object parameter)
        {
            bool shiftPressed = ModifierKeys.Shift == (Keyboard.Modifiers & ModifierKeys.Shift);
            bool altPressed = ModifierKeys.Alt == (Keyboard.Modifiers & ModifierKeys.Alt);
            bool controlPressed = ModifierKeys.Control == (Keyboard.Modifiers & ModifierKeys.Control);
            bool osPressed = ModifierKeys.Windows == (Keyboard.Modifiers & ModifierKeys.Windows) ||
                             ModifierKeys.Apple == (Keyboard.Modifiers & ModifierKeys.Apple);

            // TODO: create a new folder
        }

        public bool OpenCanExecute(object parameter)
        {
            return true;
        }

        public void OpenExecute(object parameter)
        {
        }

        public bool OpenWithCanExecute(object parameter)
        {
            return true;
        }

        public void OpenWithExecute(object parameter)
        {
//            OpenWithViewModel vm = new OpenWithViewModel();

//            IDialogService dialogSvc = this.SiteContainer.GetServiceSafe<IDialogService>();
//            dialogSvc.Show(vm);
        }

        public bool ShareWithCanExecute(object parameter)
        {
            return true;
        }

        public void ShareWithExecute(object parameter)
        {
//            ShareWithViewModel vm = new ShareWithViewModel();

//            IDialogService dialogSvc = this.SiteContainer.GetServiceSafe<IDialogService>();
//            dialogSvc.Show(vm);
        }

        public bool FileVersionCanExecute(object parameter)
        {
            return true;
        }

        public void FileVersionExecute(object parameter)
        {
//            PropertiesViewModel vm = new PropertiesViewModel();

//            IDialogService dialogSvc = this.SiteContainer.GetServiceSafe<IDialogService>();
//            dialogSvc.Show(vm);
        }

        #endregion
    }
}
