
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(Framework.Bootstrap.GlassMapper.GlassMapperSc), "Start")]

namespace Framework.Bootstrap.GlassMapper
{
    /// <summary>
    /// Glass Mapper statup for Sitecore.
    /// </summary>
    public static class GlassMapperSc
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public static void Start()
        {
            //create the resolver
            var resolver = DependencyResolver.CreateStandardResolver();

            //install the custom services
            GlassMapperScCustom.ContainerConfig(resolver.Container);

            //create a context
            var context = Glass.Mapper.Context.Create(resolver);
            context.Load(GlassMapperScCustom.GlassLoaders());

            GlassMapperScCustom.PostLoad();
        }

    }
}