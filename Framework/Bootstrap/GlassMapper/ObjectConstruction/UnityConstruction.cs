using Glass.Mapper.Pipelines.ObjectConstruction;
using System.Dynamic;

namespace Framework.Bootstrap.GlassMapper.ObjectConstruction
{
    /// <summary>
    /// Unity construction task for Glass Mapper.
    /// </summary>
    public class UnityConstruction : IObjectConstructionTask
    {
        private static readonly object LockObject  = new object();

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Execute(ObjectConstructionArgs args)
        {
            var resolver = args.Context.DependencyResolver as DependencyResolver;

            if (resolver == null)
                return;

            //check that no other task has created an object
            //also check that this is a dynamic object
            if (args.Result != null || args.Configuration.Type.IsAssignableFrom(typeof (IDynamicMetaObjectProvider)))
                return;

            //check to see if the type is registered with the container
            //if it isn't added it
            if (resolver.Container.IsRegistered(args.Configuration.Type))
            {
                lock (LockObject)
                {
                    if (resolver.Container.IsRegistered(args.Configuration.Type))
                    {
                        resolver.Container.Register<object>(args.Configuration.Type);
                    }
                }
            }

            //create instance using SimpleInjector
            var obj = resolver.Container.Resolve<object>(args.Configuration.Type);

            //map properties from item to model
            args.Configuration.MapPropertiesToObject(obj, args.Service, args.AbstractTypeCreationContext);

            //set the new object as the returned result
            args.Result = obj;
        }
    }
}

//namespace Bootstrap.GlassMapper.Task
//{
//    /// <summary>
//    /// UnityConstruction
//    /// </summary>
//    public class UnityConstruction : IObjectConstructionTask
//    {
//        public static volatile object _key = new object();

//        /// <summary>
//        /// Initializes static members of the <see cref="CreateConcreteTask"/> class.
//        /// </summary>
//        static UnityConstruction()
//        {

//        }

//        /// <summary>
//        /// Executes the specified args.
//        /// </summary>
//        /// <param name="args">The args.</param>
//        public void Execute(ObjectConstructionArgs args)
//        {
//            if (args.Result != null)
//            {
//                return;
//            }

//            var resolver = args.Context.DependencyResolver as DependencyResolver;
//            if (resolver == null)
//            {
//                return;
//            }

//            if (args.AbstractTypeCreationContext.ConstructorParameters == null || !args.AbstractTypeCreationContext.ConstructorParameters.Any())
//            {
//                if (args.Configuration != null)
//                {
//                    var configuration = args.Configuration;
//                    var type = configuration.Type;
//                    var container = resolver.Container;

//                    if (type.IsClass)
//                    {

//                        TypeRegistrationCheck(container, type);

//                        Action<object> mappingAction = (target) => configuration.MapPropertiesToObject(target, args.Service,args.AbstractTypeCreationContext);

//                        object result = null;
//                        if (args.AbstractTypeCreationContext.IsLazy)
//                        {
//                            using (new UsingLazyInterceptor())
//                            {
//                                result = container.Resolve(type.FullName + "lazy", type);
//                                var proxy = result as IProxyTargetAccessor;
//                                var interceptor = proxy.GetInterceptors().First(x => x is LazyObjectInterceptor) as LazyObjectInterceptor;
//                                interceptor.MappingAction = mappingAction;
//                                interceptor.Actual = result;
//                            }
//                        }
//                        else
//                        {
//                            result = container.Resolve<object>(type);
//                            if (result != null)
//                            {
//                                mappingAction(result);
//                            }
//                        }

//                        args.Result = result;
//                    }
//                }
//            }
//        }

//        //private void TypeRegistrationCheck(IDependencyContainer container, Type type)
//        //{
//        //    if (!container.IsRegistered(typeof(LazyObjectInterceptor)))
//        //    {
//        //        lock (_key)
//        //        {
//        //            if (!container.IsRegistered(typeof(LazyObjectInterceptor)))
//        //            {
//        //                container.Register(Component.For<LazyObjectInterceptor>().LifestyleCustom<NoTrackLifestyleManager>());
//        //            }
//        //        }
//        //    }
//        //    if (!container.IsRegistered(type))
//        //    {
//        //        lock (_key)
//        //        {
//        //            if (!container.IsRegistered(type))
//        //            {
//        //                container.Register(
//        //                    Component.For(type).Named(type.FullName).LifeStyle.Custom<NoTrackLifestyleManager>()
//        //                    );
//        //                container.Kernel.Register(
//        //                    Component.For(type).Named(type.FullName + "lazy").LifeStyle.Custom<NoTrackLifestyleManager>()
//        //                             .Interceptors<LazyObjectInterceptor>()
//        //                    );
//        //            }
//        //        }
//        //    }
//        //}
//    }
//}
