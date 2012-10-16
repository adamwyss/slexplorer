using System;

namespace Ijv.Redstone.Services
{
    /// <summary />
    public static class ApplicationSettingsExtensions
    {
        /// <summary />
        public static Uri GetUri(IApplicationSettingsService settingSvc, string key)
        {
            Argument.IsNotNull("settingSvc", settingSvc);
            Argument.IsNotNullOrEmpty("key", key);

            string value = settingSvc.GetValue(key);

            bool absoluteUri = value.IndexOf("://", StringComparison.OrdinalIgnoreCase) >= 0;

            try
            {
                return new Uri(value, absoluteUri ? UriKind.Absolute : UriKind.Relative);
            }
            catch (Exception ex)
            {
                throw new ApplicationSettingException("Unable to convert value to an URI", ex);
            }
        }

        /// <summary />
        public static bool GetBoolean(IApplicationSettingsService settingSvc, string key)
        {
            Argument.IsNotNull("settingSvc", settingSvc);
            Argument.IsNotNullOrEmpty("key", key);

            string value = settingSvc.GetValue(key);

            if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(value, "t", StringComparison.OrdinalIgnoreCase)  ||
                string.Equals(value, "0", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
