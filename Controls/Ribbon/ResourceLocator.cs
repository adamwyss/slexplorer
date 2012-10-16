using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

namespace Ijv.Redstone.Controls
{
    /// <summary>
    /// Locates the control style for the ribbon control.
    /// </summary>
    internal static class ResourceLocator
    {
        internal const string Button = "ribbon/system.windows.controls.button.xaml";
        internal const string CheckBox = "ribbon/system.windows.controls.checkbox.xaml";
        internal const string MenuButton = "ribbon/ijv.redstone.controls.menubutton.xaml";

        /// <summary>
        /// The cached version of the resource dictionary.
        /// </summary>
        private static Dictionary<string, ResourceDictionary> cache = new Dictionary<string,ResourceDictionary>();

        /// <summary>
        /// Gets the requested style.  If the key is not found, an exception if thrown.
        /// </summary>
        /// <param name="resource">The resource path that contains the key being searched for.</param>
        /// <param name="key">The resource key that is being searched for.</param>
        /// <returns>The object that was specified.</returns>
        public static T Get<T>(string resource, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            return (T)GetDictionary(resource)[key];
        }

        /// <summary>
        /// Gets the style dictionary from the assembly and caches it.
        /// </summary>
        /// <returns>The style dictionary that is contained in the resource.</returns>
        private static ResourceDictionary GetDictionary(string path)
        {
            ResourceDictionary returnValue;

            bool success = cache.TryGetValue(path, out returnValue);
            if (!success)
            {
                Assembly assembly = typeof(ResourceLocator).Assembly;

                string n1 = assembly.FullName.Split(',')[0];
                string n2 = n1 + ".g";
                string n3 = n2 + ".resources";

                foreach (string name in assembly.GetManifestResourceNames())
                {
                    if (name == n3)
                    {
                        ResourceManager resourceManager = new ResourceManager(n2, assembly);
                        using (Stream stream = resourceManager.GetStream(path))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string text = reader.ReadToEnd();
                                returnValue = (ResourceDictionary)XamlReader.Load(text);
                            }
                        }

                        break;
                    }
                }

                cache.Add(path, returnValue);
            }

            return returnValue;
        }
    }
}
