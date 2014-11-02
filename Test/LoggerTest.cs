using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Bootstrap;
using Framework.Core.Infrastructure.Logging;
using Framework.Core.Infrastructure.IoC;

namespace Test
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void WriteLog()
        {
            IoC.InitializeWith(new DependencyContainerFactory());
            IoC.Register<ILogger, FileLogger>();

            for (var i = 0; i < 100; i++)
                Logger.Info(string.Format("Hi! this is {0} test message.", i));

            for (var i = 0; i < 10000; i++)
                Logger.Info(string.Format("Hi! this is {0} test message.", i));
        }
    }
}
