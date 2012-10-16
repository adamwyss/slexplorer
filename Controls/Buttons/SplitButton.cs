using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Represents a button control.
    /// </summary>
    [TemplatePart(Name = "PART_OptionsButton", Type = typeof(MenuButton))]
    public class SplitButton : Button
    {
        /// <summary>
        /// Identifies the Content dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsMenuProperty = DependencyProperty.Register(
            "OptionsMenu",
            typeof(Menu),
            typeof(SplitButton),
            null);

        /// <summary>
        /// Creates an instance of the SplitButton class
        /// </summary>
        public SplitButton()
        {
            this.DefaultStyleKey = typeof(SplitButton);
        }

        /// <summary>
        /// Gets the menu property that is displayed when the button is pressed.
        /// </summary>
        public Menu OptionsMenu
        {
            get { return (Menu)this.GetValue(OptionsMenuProperty); }
            set { this.SetValue(OptionsMenuProperty, value); }
        }

        /// <summary>
        /// When a template is applied, wireup all necessary template parts.
        /// </summary>
        public override void OnApplyTemplate()
        {
            MenuButton button = this.GetTemplateChild("PART_OptionsButton") as MenuButton;
            if (button != null)
            {
                button.MenuPlacementTarget = this;
                button.MenuPlacement = PlacementMode.Bottom;
                button.GotFocus += new RoutedEventHandler(delegate
                    {
                        // take the focus back.
                        this.Focus();
                    });
            }

            base.OnApplyTemplate();
        }
    }
}
