using Framework.Bootstrap.Container;
using Framework.Core.Infrastructure.IoC;
using Framework.Sc.Extensions.ErrorHandler;
using Framework.Sc.Extensions.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Framework.Bootstrap.Start.Bootstrapper), "Initialize")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Framework.Bootstrap.Start.Bootstrapper), "Shutdown")]

namespace Framework.Bootstrap.Start
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            // Register our modules
            DynamicModuleUtility.RegisterModule(typeof(ErrorLoggerModule));
            DynamicModuleUtility.RegisterModule(typeof(ErrorHandlerModule));
            //DynamicModuleUtility.RegisterModule(typeof(ClaimsTransformationHttpModule));
        }

        public static void Start()
        {
            AreaRegistration.RegisterAllAreas();
            IoC.InitializeWith(new DependencyContainerFactory());
            TypeRegistration.RegisterTypes();
            DependencyResolver.SetResolver(new CustomDependencyResolver());
            ValueProviderFactories.Factories.Add(new TempDataModelProviderFactory());
        }

        public static void Shutdown()
        {
        }

        public virtual void Process(object args)
        {
            Start();
        }
    }
}