using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a group of items contained in a tab of the ribbon.
    /// </summary>
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates"),
    TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"),
    ContentProperty("Items")]
    public class RibbonGroup : Control
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the Items dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
                "Items",
                typeof(ControlsCollection),
                typeof(RibbonGroup),
                null);

        /// <summary>
        /// Identifies the IsMouseOver dependency property.
        /// </summary>
        public static readonly DependencyProperty IsMouseOverProperty = DependencyProperty.Register(
                "IsMouseOver",
                typeof(bool),
                typeof(RibbonGroup),
                new PropertyMetadata(OnIsMouseOverPropertyChanged));

        /// <summary>
        /// Identifies the Label dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(RibbonGroup),
                null);

        /// <summary>
        /// Identifies the ItemsPanel dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsPanelProperty = DependencyProperty.Register(
                "ItemsPanel",
                typeof(ItemsPanelTemplate),
                typeof(RibbonGroup),
                null);

        /// <summary>
        /// Identifies the Command dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(RibbonGroup),
                null);

        /// <summary>
        /// Identifies the CommandProperty dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(RibbonGroup),
                null);

        #endregion

        /// <summary>
        /// Initializes a new instance of the RibbonGroup class.
        /// </summary>
        public RibbonGroup()
            : base()
        {
            this.DefaultStyleKey = typeof(RibbonGroup);

            this.Items = new ControlsCollection();
        }

        #region Properties

        /// <summary>
        /// Gets the items of this group.
        /// </summary>
        public ControlsCollection Items
        {
            get { return (ControlsCollection)this.GetValue(ItemsProperty); }
            private set { this.SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is located over this element (including child elements in the visual tree).
        /// </summary>
        public bool IsMouseOver
        {
            get { return (bool)this.GetValue(IsMouseOverProperty); }
            private set { this.SetValue(IsMouseOverProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text to display on the group.
        /// </summary>
        public string Label
        {
            get { return (string)this.GetValue(LabelProperty); }
            set { this.SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template that defines the panel that controls the layout of items.
        /// </summary>
        public ItemsPanelTemplate ItemsPanel
        {
            get { return (ItemsPanelTemplate)this.GetValue(ItemsPanelProperty); }
            set { this.SetValue(ItemsPanelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to invoke when the dialog button is pressed.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the Command property.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the visual state of the control. 
        /// </summary>
        internal void ChangeVisualState()
        {
            if (this.IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        /// <summary>
        /// Occurs when the mouse enters the control surface.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            this.IsMouseOver = true;
        }

        /// <summary>
        /// Occurs when the mouse leaves the control surface.
        /// </summary>
        /// <param name="e">The MouseEventArgs that contain the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.IsMouseOver = false;
        }

        /// <summary>
        /// Occurs when the IsMouseOver property changes.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs that contains the event data.</param>
        private static void OnIsMouseOverPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RibbonGroup group = sender as RibbonGroup;
            if (group != null)
            {
                group.ChangeVisualState();
            }
        }

        #endregion
    }
}