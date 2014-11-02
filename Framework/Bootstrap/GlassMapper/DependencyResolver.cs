using Glass.Mapper;
using Framework.Core.Infrastructure.IoC;
using System.Collections.Generic;

namespace Framework.Bootstrap.GlassMapper
{
    /// <summary>
    /// IDependencyResolver implementation.
    /// </summary>
    public class DependencyResolver : IDependencyResolver
    {
        /// <summary> 
        /// Creates the standard resolver. 
        /// </summary> 
        /// <returns>IDependencyResolver.</returns> 
        public static DependencyResolver CreateStandardResolver()
        {
            IDependencyContainer container = new DependencyContainerFactory().CurrentContainer;
            return new DependencyResolver(container);
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="DependencyResolver"/> class. 
        /// </summary> 
        /// <param name="container">The container.</param> 
        public DependencyResolver(IDependencyContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public IDependencyContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Resolves the specified arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public T Resolve<T>(IDictionary<string, object> args = null)
        {
            return this.Container.Resolve<T>();
        }

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return this.Container.ResolveAll<T>();
        }
    }
}
