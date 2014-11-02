using Framework.Core.Infrastructure.IoC;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Framework.Bootstrap.Container
{
    /// <summary>
    /// An implementation of <see cref="IDependencyResolver"/> that wraps a Unity container.
    /// </summary>
    public class CustomDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// Resolves an instance of the default requested type from the container.
        /// </summary>
        /// <param name="serviceType">The <see cref="Type"/> of the object to get from the container.</param>
        /// <returns>The requested object.</returns>
        public object GetService(Type serviceType)
        {
            if (typeof(IController).IsAssignableFrom(serviceType))
            {
                //return this.container.Resolve(serviceType);
                return IoC.Resolve<object>(serviceType);
            }
            try
            {
                //return this.container.Resolve(serviceType);
                return IoC.Resolve<object>(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>The requested services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            //return this.container.ResolveAll(serviceType);
            return IoC.ResolveAll<object>(serviceType);
        }
    }
}
