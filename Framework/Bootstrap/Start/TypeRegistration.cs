using Framework.Core.Infrastructure.IoC;
using Framework.Core.Infrastructure.Logging;
using Framework.Sc.Extensions.Mvc;
using System.Web.Mvc;

namespace Framework.Bootstrap.Start
{
    /// <summary>
    /// Type Registration class for dependency mapping.
    /// </summary>
    public class TypeRegistration
    {
        /// <summary>
        /// Registers the types.
        /// </summary>
        public static void RegisterTypes()
        {
            IoC.Register<ITempDataProvider, SitecoreSessionStateTempDataProvider>();
            IoC.Register<ILogger, FileLogger>();
        }
    }
}
