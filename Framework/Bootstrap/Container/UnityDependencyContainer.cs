using Framework.Core.Infrastructure.IoC;
using Microsoft.Practices.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.Bootstrap.Container
{
    public class UnityDependencyContainer : IDependencyContainer
    {
        private readonly IUnityContainer _container;

        #region Constructor

        public UnityDependencyContainer()
            : this(new UnityContainer())
        {
            //UnityConfigurationSection configuration = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            //configuration.Configure(_container);
        }

        public UnityDependencyContainer(IUnityContainer container)
        {
            _container = container;
        }

        ~UnityDependencyContainer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
            }
        }

        #endregion

        public string Name
        {
            get { return "Unity"; }
        }

        public bool IsRegistered(Type typeToCheck)
        {
            return _container.IsRegistered(typeToCheck);
        }

        public void Register<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        public void Register<TFrom, TTo>()
            where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>();
        }

        public void Register<TFrom, TTo>(IDictionary arguments = null) where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>();
        }

        public void Register<TFrom, TTo>(LifetimeScope lifetimeScope, IDictionary arguments = null) where TTo : TFrom
        {
            _container.RegisterType<TFrom, TTo>();
        }

        public void Inject<T>(T existing)
        {
            _container.BuildUp(existing);
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(string name)
        {
            return _container.Resolve<T>(name);
        }

        public T Resolve<T>(Type type)
        {
            return (T)_container.Resolve(type);
        }

        public T Resolve<T>(Type type, string name)
        {
            return (T)_container.Resolve(type, name);
        }

        public T Resolve<T>(IDictionary arguments)
        {
            ParameterOverrides resolverOverride = GetParametersOverrideFromDictionary<T>(arguments);
            return _container.Resolve<T>(resolverOverride);
        }

        public T Resolve<T>(string name, IDictionary arguments)
        {
            ParameterOverrides resolverOverride = GetParametersOverrideFromDictionary<T>(arguments);
            return _container.Resolve<T>(name, resolverOverride);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            IEnumerable<T> namedInstances = _container.ResolveAll<T>();
            T unnamedInstance = default(T);

            try
            {
                unnamedInstance = _container.Resolve<T>();
            }
            catch (ResolutionFailedException)
            {
                //When default instance is missing
            }

            if (Equals(unnamedInstance, default(T)))
            {
                return namedInstances;
            }

            return new ReadOnlyCollection<T>(new List<T>(namedInstances) { unnamedInstance });
        }

        public IEnumerable<T> ResolveAll<T>(Type type)
        {
            return (IEnumerable<T>)_container.ResolveAll(type);
        }

        private static ParameterOverrides GetParametersOverrideFromDictionary<T>(IDictionary arguments)
        {
            ParameterOverrides resolverOverride = new ParameterOverrides();
            foreach (string key in arguments.Keys)
            {
                resolverOverride.Add(key, arguments[key]);
            }

            return resolverOverride;
        }
    }
}
