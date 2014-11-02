using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Sc;
using Framework.Core.Infrastructure.IoC;
using Framework.Bootstrap.GlassMapper.ObjectConstruction;

namespace Framework.Bootstrap.GlassMapper
{
    public static class GlassMapperScCustom
    {
        public static void ContainerConfig(IDependencyContainer container)
        {
            var config = new Config();

            container.Register<IObjectConstructionTask, UnityConstruction>();
            //  config.EnableCaching = false;

            container.Register(new SitecoreInstaller(config));
        }

        public static IConfigurationLoader[] GlassLoaders()
        {
            var attributes = new AttributeConfigurationLoader("Sitecore.Models");
            return new IConfigurationLoader[] { attributes };
            //return new IConfigurationLoader[] { attributes, Models.Config.ContentConfig.Load() };
        }

        public static void PostLoad()
        {
            //Comment this code in to activate CodeFist
            /* CODE FIRST START
            var dbs = Sitecore.Configuration.Factory.GetDatabases();
            foreach (var db in dbs)
            {
                var provider = db.GetDataProviders().FirstOrDefault(x => x is GlassDataProvider) as GlassDataProvider;
                if (provider != null)
                {
                    using (new SecurityDisabler())
                    {
                        provider.Initialise(db);
                    }
                }
            }
             * CODE FIRST END
             */
        }
    }
}