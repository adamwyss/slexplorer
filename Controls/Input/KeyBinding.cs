using System.Windows;
using System.Windows.Input;

namespace Ijv.Redstone.Input
{
    /// <summary>
    /// Binds a key gesture to a command.
    /// </summary>
    public class KeyBinding : InputBinding
    {
        /// <summary>
        /// Identifies the Key dependency property.
        /// </summary>
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(Key),
            typeof(KeyBinding),
            null);

        /// <summary>
        /// Identifies the Modifier dependency property.
        /// </summary>
        public static readonly DependencyProperty ModifierProperty = DependencyProperty.Register(
            "Modifier",
            typeof(ModifierKeys),
            typeof(KeyBinding),
            null);

        /// <summary>
        /// Get or sets the Key fo the gesture associated with this key binding.
        /// </summary>
        public Key Key
        {
            get { return (Key)this.GetValue(KeyProperty); }
            set { this.SetValue(KeyProperty, value); }
        }

        /// <summary>
        /// Get or sets the modifier key of the key gesture associated with this key binding.
        /// </summary>
        public ModifierKeys Modifier
        {
            get { return (ModifierKeys)this.GetValue(ModifierProperty); }
            set { this.SetValue(ModifierProperty, value); }
        }
    }
}
