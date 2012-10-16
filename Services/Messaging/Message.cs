using System;
using System.Reflection;

namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Provides utility methods that perform common tasks involving messaging.
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// Sends a message to registered recipients. The message will reach all recipients that registered for this message type using one of the Register methods.
        /// </summary>
        /// <param name="message">The message to send to registered recipients.</param>
        public static void Send(object message)
        {
            // preconditions

            Argument.IsNotNull("message", message);

            // implementation

            IMessagingService messageSvc = ServiceLocator.Current.GetServiceSafe<IMessagingService>();
            messageSvc.Send(message);
        }

        /// <summary />
        public static void Register(object subscriber)
        {
            // preconditions

            Argument.IsNotNull("subscriber", subscriber);

            // implementation

            IMessagingService messageSvc = ServiceLocator.Current.GetServiceSafe<IMessagingService>();

            Type subscriberType = subscriber.GetType();

            foreach (Type @interface in subscriberType.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IHandleMessages<>))
                {
                    Type[] genericArgs = @interface.GetGenericArguments();
                    if (genericArgs.Length != 1)
                    {
                        throw new InvalidOperationException("The IHandleMessages interface contains more than one generic argument.");
                    }

                    Type messageType = genericArgs[0];

                    InterfaceMapping mapping = subscriberType.GetInterfaceMap(@interface);
                    MethodInfo method = mapping.InterfaceMethods[0];

                    Type t1 = typeof(Action<>);
                    Type delegateType = t1.MakeGenericType(new Type[] { messageType });

                    Delegate @delegate = Delegate.CreateDelegate(delegateType, method);


                    




                    // call the registier method on the message service.
                    MethodInfo registerMethod = typeof(IMessagingService).GetMethod("Register", new Type[] { typeof(object), typeof(bool), delegateType });
                    if (registerMethod == null)
                    {
                        throw new InvalidOperationException();
                    }

                    MethodInfo genericMethod = registerMethod.MakeGenericMethod(new Type[] { messageType });
                    if (genericMethod == null)
                    {
                        throw new InvalidOperationException();
                    }

                    genericMethod.Invoke(messageSvc, new object[] { subscriber, true, @delegate });

                }
            }


        }
    }
}
