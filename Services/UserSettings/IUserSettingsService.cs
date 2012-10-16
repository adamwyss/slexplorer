using System;
using System.ComponentModel;

namespace Ijv.Redstone.Services
{
    /// <summary />
    public interface IUserSettingsService
    {
        /// <summary />
        event EventHandler SettingsCleared;

        /// <summary />
        void Save(object target);

        /// <summary />
        void Load(object target);

        /// <summary />
        void ClearAll();
    }
}
