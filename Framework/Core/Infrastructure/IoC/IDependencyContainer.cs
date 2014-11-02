using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Core.Infrastructure.IoC
{
    /// <summary>
    /// Container lifetime scope enumeration
    /// </summary>
    public enum LifetimeScope
    {
        /// <summary>
        /// The instance
        /// </summary>
        Instance,
        /// <summary>
        /// The singleton
        /// </summary>
        Singleton,
        /// <summary>
        /// The per HTTP request
        /// </summary>
        PerHttpRequest
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDependencyContainer : IDisposable
    {
        /// <summary>
        /// Determines whether the specified type to check is registered.
        /// </summary>
        /// <param name="typeToCheck">The type to check.</param>
        /// <returns></returns>
        bool IsRegistered(Type typeToCheck);

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        T Resolve<T>(Type type);

        /// <summary>
        /// Resolves the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        T Resolve<T>(Type type, string name);

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>(Type type);

        /// <summary>
        /// Resolves the specified arguments.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        T Resolve<T>(IDictionary arguments);

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        T Resolve<T>(string name, IDictionary arguments);

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Injects the specified existing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existing">The existing.</param>
        void Inject<T>(T existing);

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        void Register<T>(T instance);

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="TFrom">The type of from.</typeparam>
        /// <typeparam name="TTo">The type of to.</typeparam>
        void Register<TFrom, TTo>()
            where TTo : TFrom;

        /// <summary>
        /// Registers the specified arguments.
        /// </summary>
        /// <typeparam name="TFrom">The type of from.</typeparam>
        /// <typeparam name="TTo">The type of to.</typeparam>
        /// <param name="arguments">The arguments.</param>
        void Register<TFrom, TTo>(IDictionary arguments = null)
            where TTo : TFrom;

        /// <summary>
        /// Registers the specified lifetime scope.
        /// </summary>
        /// <typeparam name="TFrom">The type of from.</typeparam>
        /// <typeparam name="TTo">The type of to.</typeparam>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="arguments">The arguments.</param>
        void Register<TFrom, TTo>(LifetimeScope lifetimeScope, IDictionary arguments = null)
            where TTo : TFrom;
    }
}
