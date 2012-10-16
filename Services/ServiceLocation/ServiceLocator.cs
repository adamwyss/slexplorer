using System;
using System.ComponentModel.Composition.Hosting;

namespace Ijv.Redstone.Services
{
    /// <summary />
    public static class ServiceLocator
    {
        /// <summary>
        /// Gets the the current service provider.
        /// </summary>
        public static IServiceProvider Current { get; private set; }

        /// <summary />
        public static void SetLocatorProvider(CompositionContainer container)
        {
            // preconditions

            Argument.IsNotNull("container", container);

            // implementation

            if (Current != null)
            {
                throw new InvalidOperationException("The ServiceLocator has already been initialized either by another call to SetLocatorProvider or by someone causing the default container to be constructed. Ensure that SetLocatorProvider() is one of the first things that happens in the application to ensure that it is ready.");
            }

            Current = new CompositionCatalogServiceProvider(container);
        }
    }
}
