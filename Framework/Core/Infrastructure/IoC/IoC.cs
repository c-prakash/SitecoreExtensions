using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework.Core.Infrastructure.IoC
{
    public static class IoC
    {
        private static IDependencyContainer resolver;

        public static void InitializeWith(IDependencyContainerFactory factory)
        {
            resolver = factory.CurrentContainer;
        }

        public static bool IsRegistered(Type typeToCheck)
        {
            return resolver.IsRegistered(typeToCheck);
        }

        public static void Register<T>(T instance)
        {
            resolver.Register<T>(instance);
        }

        public static void Register<TFrom, TTo>()
            where TTo : TFrom
        {
            resolver.Register<TFrom, TTo>();
        }

        public static void Register<TFrom, TTo>(IDictionary arguments)
            where TTo : TFrom
        {
            resolver.Register<TFrom, TTo>(arguments);
        }

        public static void Register<TFrom, TTo>(LifetimeScope lifetimeScope, IDictionary arguments = null)
            where TTo : TFrom
        {
            resolver.Register<TFrom, TTo>(lifetimeScope, arguments);
        }

        public static void Inject<T>(T existing)
        {
            resolver.Inject<T>(existing);
        }

        public static T Resolve<T>()
        {
            return resolver.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return resolver.Resolve<T>(name);
        }

        public static T Resolve<T>(Type type)
        {
            return (T)resolver.Resolve<T>(type);
        }

        public static T Resolve<T>(Type type, string name)
        {
            return (T)resolver.Resolve<T>(type, name);
        }

        public static T Resolve<T>(IDictionary arguments)
        {
            return (T)resolver.Resolve<T>(arguments);
        }

        public static T Resolve<T>(string name, IDictionary arguments)
        {
            return (T)resolver.Resolve<T>(name, arguments);
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return resolver.ResolveAll<T>();
        }

        public static IEnumerable<T> ResolveAll<T>(Type type)
        {
            return (IEnumerable<T>)resolver.ResolveAll<T>(type);
        }

        public static void Reset()
        {
            if (resolver != null)
            {
                resolver.Dispose();
            }
        }
    }
}
