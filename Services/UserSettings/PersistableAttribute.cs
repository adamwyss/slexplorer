using System;

namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Associates a property with a key in a user store.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class PersistableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets thee key for the setting.  If empty, the property name will be used.
        /// </summary>
        public string SettingsKey { get; set; }

        /// <summary>
        /// Gets or sets the scope of this persisted setting.
        /// </summary>
        public PersistableSettingScope Scope { get; set; }
    }
}
