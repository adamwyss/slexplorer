
namespace Ijv.Redstone.Services
{
    /// <summary>
    /// The possible message routings for a message.
    /// </summary>
    public enum MessageRouting
    {
        /// <summary>
        /// [NOT SUPPORTED] Messages are routed to all subscribers in silverlight hosts on the local machine.
        /// </summary>
        LocalMachine,

        /// <summary>
        /// Message are routed to all subscribers in the current silverlight process.
        /// </summary>
        LocalProcess,
    }
}
