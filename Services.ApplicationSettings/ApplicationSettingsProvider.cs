using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Ijv.Redstone.Services.ApplicationSettings
{
    /// <summary />
    [Export(typeof(IApplicationSettingsService))]
    public class ApplicationSettingsProvider : IApplicationSettingsService
    {
        /// <summary />
        private static readonly Uri LocalApplicationSettingUri = new Uri("default.Config", UriKind.Relative);

        /// <summary />
        private XDocument localSettings;

        /// <summary />
        private XDocument serverSettings;

        /// <summary />
        [ImportingConstructor]
        public ApplicationSettingsProvider()
        {
            StreamResourceInfo info = Application.GetResourceStream(LocalApplicationSettingUri);

            this.localSettings = XDocument.Load(info.Stream);
        }

        /// <summary />
        public string GetValue(string key)
        {
            return null;
        }

        /// <summary />
        public IEnumerable<Tuple<string, string>> GetApplicationSettings()
        {
            List<Tuple<string, string>> settings = new List<Tuple<string, string>>();
            return settings;
        }
    }
}
