using System;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using System.Windows;

namespace Ijv.Redstone.Services.Messaging
{
    [Export(typeof(IMessagingService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MessageService : IMessagingService
    {
        /// <summary>
        /// Contains the messages that need to be sent.  Once a message is sent, it is removed from the queue.
        /// </summary>
        private Collection<Envelope> queue = new Collection<Envelope>();
        
        /// <summary>
        /// Initializes a new instance of the MessageService class.
        /// </summary>
        public MessageService()
        {

        }

        /// <summary /> 
        public void Register<TMessage>(object recipient, Action<TMessage> action)
        {
            // 1) add recipient and action to list
        }

        /// <summary />
        public void Register<TMessage>(object recipient, bool receiveDerivedMessagesToo, Action<TMessage> action)
        {
            // 1) add recipient and action to list.
        }

        /// <summary />
        public void Send(object message)
        {
            Envelope envelope = new Envelope();
            envelope.Message = message;
            this.queue.Add(envelope);

            Deployment.Current.Dispatcher.BeginInvoke(this.ProcessMessageQueue);
        }

        /// <summary />
        public void Send<TTarget>(object message)
        {
            Envelope envelope = new Envelope();
            envelope.Message = message;
            this.queue.Add(envelope);

            Deployment.Current.Dispatcher.BeginInvoke(this.ProcessMessageQueue);
        }

        /// <summary />
        public void Unregister<TMessage>(object recipient)
        {
            // 1) remove recipient's action for TMessage
        }

        /// <summary />
        public void Unregister(object recipient)
        {
            // 1) remove all reciepient's actions
        }

        private void ProcessMessageQueue()
        {
            // foreach item in queue
            // 1) determine message type
            // 2) determine if message has specific target type.
            // 4) get subscribers to that message type
            // 5) determine if subscribers want derives messages
            // 5) call the 'action' on each qualified subscriber
        }
    }
}
