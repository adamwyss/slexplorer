
namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Specifies a persisted setting scope.
    /// </summary>
    public enum PersistableSettingScope
    {
        /// <summary>
        /// The setting is persisted on the local machine for this user.
        /// </summary>
        Local,

        /// <summary>
        /// The setting is persisted on a server for this user.
        /// </summary>
        Remote,
    }
}
