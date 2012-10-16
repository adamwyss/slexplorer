using System;

namespace Ijv.Redstone.Services
{
        /// <summary>
        /// Represents a MessagingService that allows objects to exchange messages.
        /// </summary>
    public interface IMessagingService
    {
        /// <summary>
        /// Registers a recipient for a type of message TMessage. The action parameter will be executed when a corresponding message is sent.
        /// Registering a recipient does not create a hard reference to it, so if this recipient is deleted, no memory leak is caused.
        /// </summary>
        /// <typeparam name="TMessage">The type of message that the recipient registers for.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="action">The action that will be executed when a message of type TMessage is sent.</param>
        void Register<TMessage>(object recipient, Action<TMessage> action);

        /// <summary>
        /// Registers a recipient for a type of message TMessage.  The action parameter will be executed when a corresponding message is
        /// sent. Registering a recipient does not create a hard reference to it, so if this recipient is deleted, no memory leak is caused.
        /// </summary>
        /// <typeparam name="TMessage">The type of message that the recipient registers for.</typeparam>
        /// <param name="recipient">The recipient that will receive the messages.</param>
        /// <param name="receiveDerivedMessagesToo">If true, message types deriving from TMessage will also be transmitted to the recipient.</param>
        /// <param name="action">The action that will be executed when a message of type TMessage is sent.</param>
        void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action);

        /// <summary>
        /// Sends a message to registered recipients. The message will reach all recipients that registered for this message type using one of the Register methods.
        /// </summary>
        /// <param name="message">The message to send to registered recipients.</param>
        void Send(object message);

        /// <summary>
        /// Sends a message to registered recipients. The message will reach only recipients that registered for this message type
        /// using one of the Register methods, and that are of the targetType.
        /// </summary>
        /// <typeparam name="TTarget">The type of recipients that will receive the message. The message won't be sent to recipients of another type.</typeparam>
        /// <param name="message"></param>
        void Send<TTarget>(object message);

        /// <summary>
        /// Unregisters a message recipient for a given type of messages only. After this method is executed, the recipient will not receive messages of type
        /// TMessage anymore, but will still receive other message types (if it registered for them previously).
        /// </summary>
        /// <typeparam name="TMessage">The type of messages that the recipient wants to unregister from.</typeparam>
        /// <param name="recipient">The recipient that must be unregistered.</param>
        void Unregister<TMessage>(object recipient);

        /// <summary>
        /// Unregisters a messager recipient completely. After this method is executed, the recipient will not receive any messages anymore.
        /// </summary>
        /// <param name="recipient">The recipient that must be unregistered.</param>
        void Unregister(object recipient);
    }
}
