using Framework.Core.Infrastructure.IoC;
using Framework.Bootstrap.Container;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web.Mvc;
using WebActivatorEx;
using Framework.Sc.Extensions.Mvc;
using Framework.Sc.Extensions.Security;

[assembly: PreApplicationStartMethod(typeof(Framework.Bootstrap.Start.Bootstrapper), "Initialize")]
[assembly: PostApplicationStartMethod(typeof(Framework.Bootstrap.Start.Bootstrapper), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Framework.Bootstrap.Start.Bootstrapper), "Shutdown")]

namespace Framework.Bootstrap.Start
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            // Register our modules
            DynamicModuleUtility.RegisterModule(typeof(ApplicationErrorModule));
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
    }
}