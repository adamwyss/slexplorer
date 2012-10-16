using System;
using System.Collections.Generic;

namespace Ijv.Redstone.Services
{
    /// <summary />
    public interface IApplicationSettingsService
    {
        /// <summary />
        string GetValue(string key);

        /// <summary />
        IEnumerable<Tuple<string, string>> GetApplicationSettings();
    }
}
