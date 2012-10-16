using System;
using System.ComponentModel.Composition;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Windows;
using System.ComponentModel;

namespace Ijv.Redstone.Services.UserSettings
{
    /// <summary />
    [Export(typeof(IUserSettingsService))]
    public class UserSettingsProvider : IUserSettingsService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingsProvider"/> class.
        /// </summary>
        public UserSettingsProvider()
        {
        }

        /// <summary />
        public event EventHandler SettingsCleared;

        /// <summary />
        public void Save(object target)
        {
            // preconditions

            Argument.IsNotNull("target", target);

            // implementation

            Type myType = target.GetType();
            Type attribType = typeof(PersistableAttribute);

            IsolatedLocalSettings local = new IsolatedLocalSettings(myType.Name + ".settings");
            IsolatedRemoteSettings remote = new IsolatedRemoteSettings(myType);

            foreach (PropertyInfo p in myType.GetProperties())
            {
                // should automatically cast to a persistable attribute.
                foreach (PersistableAttribute attribute in p.GetCustomAttributes(attribType, true))
                {
                    string key = attribute.SettingsKey;
                    if (string.IsNullOrEmpty(key))
                    {
                        key = p.Name;
                    }

                    // save name / value pair in settings store.
                    object value = p.GetValue(target, null);

                    if (local.Contains(key))
                    {
                        local[key] = value;
                    }
                    else
                    {
                        local.Add(key, value);
                    }

                    break;
                }
            }

            local.Save();
            remote.Save();
        }

        /// <summary />
        public void Load(object target)
        {
            // preconditions

            Argument.IsNotNull("target", target);

            // implementation

            Type myType = target.GetType();
            Type attribType = typeof(PersistableAttribute);

            IsolatedLocalSettings settings = new IsolatedLocalSettings(myType.Name + ".settings");
            IsolatedRemoteSettings remote = new IsolatedRemoteSettings(myType);

            if (settings.Count > 0)
            {
                foreach (PropertyInfo p in myType.GetProperties())
                {
                    // should automatically cast to a persistable attribute.
                    foreach (PersistableAttribute attribute in p.GetCustomAttributes(attribType, true))
                    {
                        string key = attribute.SettingsKey;
                        if (string.IsNullOrEmpty(key))
                        {
                            key = p.Name;
                        }

                        if (settings.Contains(key))
                        {
                            p.SetValue(target, settings[key], null);
                        }

                        break;
                    }
                }
            }
        }

        /// <summary />
        public void ClearAll()
        {
            // remove all locally stored files.
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                storage.Remove();
            }
        }

        /// <summary />
        private void RaiseSettingsClearedEvent()
        {
            EventHandler handler = this.SettingsCleared;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
