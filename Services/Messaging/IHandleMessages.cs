
namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Indicates that any classes that implement this method want to handle a message of 
    /// type T.
    /// </summary>
    /// <typeparam name="T">The message type that is to be handed by this implementation.</typeparam>
    public interface IHandleMessages<T>
    {
        /// <summary>
        /// The method that will perform the message handling.
        /// </summary>
        /// <param name="message">An instance of a message that needs to be handled.</param>
        void Handle(T message);
    }
}