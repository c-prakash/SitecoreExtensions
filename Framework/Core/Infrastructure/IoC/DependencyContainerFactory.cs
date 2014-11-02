using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Infrastructure.IoC
{
    /// <summary>
    /// Dependency Container Factory to create container instance.
    /// </summary>
    public class DependencyContainerFactory : IDependencyContainerFactory
    {
        /// <summary>
        /// The containers
        /// </summary>
        private static readonly Dictionary<string, IDependencyContainer> containers = new Dictionary<string, IDependencyContainer>();

        /// <summary>
        /// Flag to notify assembly has been scanned
        /// </summary>
        private static bool hasScanned;

        /// <summary>
        /// The current container
        /// </summary>
        private IDependencyContainer currentContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainerFactory"/> class.
        /// </summary>
        public DependencyContainerFactory() : this(String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainerFactory"/> class.
        /// </summary>
        /// <param name="dependecyContainer">The dependecy container.</param>
        public DependencyContainerFactory(IDependencyContainer dependecyContainer)
        {
            this.currentContainer = dependecyContainer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainerFactory"/> class.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        public DependencyContainerFactory(string containerName)
        {
            this.currentContainer = GetContainer(containerName) ?? containers.FirstOrDefault().Value;
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <value>
        /// The containers.
        /// </value>
        protected Dictionary<string, IDependencyContainer> Containers
        {
            get
            {
                this.PopulateContainer();
                return containers;
            }
        }

        /// <summary>
        /// Gets or sets the current container.
        /// </summary>
        /// <value>
        /// The current container.
        /// </value>
        /// <exception cref="System.ArgumentNullException">container;The current container was not located correctly.</exception>
        public IDependencyContainer CurrentContainer
        {
            get { return this.currentContainer; }
            protected set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("container", "The current container was not located correctly.");
                }

                this.currentContainer = value;
            }
        }

        /// <summary>
        /// Populates the container.
        /// </summary>
        protected void PopulateContainer()
        {
            if (hasScanned)
            {
                return;
            }

            hasScanned = true;

            // go fishing ;)
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
               PopulateContainerInAssembly(assembly);
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <returns></returns>
        protected IDependencyContainer GetContainer(string containerName) 
        {
            return !Containers.ContainsKey(containerName) ? null : Containers[containerName]; 
        }

        /// <summary>
        /// Populates the container in assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        private static void PopulateContainerInAssembly(Assembly assembly)
        {
            IEnumerable<Type> potentialContainers;
            try
            {
                potentialContainers = assembly.GetTypes().Where(IsPotentialContainer);
            }
            catch
            {
                return;
            }

            foreach (Type potentialContainer in potentialContainers)
            {
                if (potentialContainer.IsAbstract)
                {
                    continue;
                }

                var container = Activator.CreateInstance(potentialContainer) as IDependencyContainer;
                if (container == null)
                {
                    continue;
                }

                containers.Add(container.Name, container);
            }
        }

        /// <summary>
        /// Determines whether [is potential container] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static bool IsPotentialContainer(Type type)
        {
            try
            {
                return type.GetInterfaces().Contains(typeof(IDependencyContainer));
                //return type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDependencyContainer));
            }
            catch
            {
                return false;
            }
        }
    }
}
