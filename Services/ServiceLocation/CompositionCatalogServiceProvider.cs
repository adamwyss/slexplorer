using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace Ijv.Redstone.Services
{
    /// <summary />
    internal class CompositionCatalogServiceProvider : IServiceProvider
    {
        /// <summary />
        CompositionContainer serviceContainer;

        /// <summary />
        public CompositionCatalogServiceProvider(CompositionContainer container)
        {
            // preconditions

            Argument.IsNotNull("container", container);

            // implementation

            this.serviceContainer = container;
        }

        /// <summary />
        public object GetService(Type serviceType)
        {
            // preconditions

            Argument.IsNotNull("serviceType", serviceType);

            // implementation

            MethodInfo method = typeof(CompositionContainer).GetMethod("GetExportedValue", new Type[] {} );
            if (method != null)
            {
                MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { serviceType });
                if (genericMethod != null)
                {
                    try
                    {
                        return genericMethod.Invoke(this.serviceContainer, null);
                    }
                    catch (TargetInvocationException ex)
                    {
                        // the call threw an exception, we will unwrap it so the exception is
                        // valid to the caller.
                        throw ex.InnerException;
                    }
                }
            }

            // we were unable to find a way to retrieve the service, so it 
            // doesn't exist.
            return null;
        }
    }
}
