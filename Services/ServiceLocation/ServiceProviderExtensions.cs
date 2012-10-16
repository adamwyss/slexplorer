using System;

namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Extends the functionality for the IServiceProvider interface.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">An type that specifies the type of service object to get.</typeparam>
        /// <param name="serviceProvider">Object that can provide services that implements <see cref="IServiceProvider" /></param>
        /// <returns>A service object of the specified type or a null reference if there is no service object of the specified type.</returns>
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            // preconditions

            Argument.IsNotNull("serviceProvider", serviceProvider);

            // implementation

            return (T)serviceProvider.GetService(typeof(T));
        }

        /// <summary>
        /// Gets the service object of the specified type.  If that service does not exist, an NullReferenceException is thrown.
        /// </summary>
        /// <typeparam name="T">An type that specifies the type of service object to get.</typeparam>
        /// <param name="serviceProvider">Object that can provide services that implements <see cref="IServiceProvider" /></param>
        /// <returns>A service object of the specified type.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static T GetServiceSafe<T>(this IServiceProvider serviceProvider)
        {
            // preconditions

            Argument.IsNotNull("serviceProvider", serviceProvider);

            // implementation

            T service = serviceProvider.GetService<T>();
            if (service == null)
            {
                throw new NullReferenceException("Unable to find the specified service.");
            }

            return service;
        }
    }
}
