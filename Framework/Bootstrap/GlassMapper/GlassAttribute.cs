using Glass.Mapper.Sc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Setup.GlassMapper
{
    public enum GlassDataSourceBehavior
    {
        Failover,
        Override
    }

    public class GlassAttribute : CustomModelBinderAttribute, IModelBinder
    {
        public string DataSource { get; set; }

        public GlassDataSourceBehavior Behavior { get; set; }

        public override IModelBinder GetBinder()
        {
            return this;
        }

        public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // get the model type
            var modelType = bindingContext.ModelType;

            var resolver = DependencyResolver.CreateStandardResolver();

            // get the sitecore service
            ISitecoreService sitecoreService = resolver.Resolve<ISitecoreService>();

            if (sitecoreService == null)
                throw new Exception("Unable to resolve dependency for Glass.Sitecore.Mapper.ISitecoreService. Register the interface once in the global application, or in the AreaRegistration.");

            Guid dataSourceId = Guid.Empty;
            string dataSourcePath = string.Empty;

            // check if its an ID or path
            if (!Guid.TryParse(DataSource, out dataSourceId))
                dataSourcePath = DataSource;

            // do some workflow
            if (Behavior == GlassDataSourceBehavior.Failover)
            {
                // get the current rendering context id
                // uses monads http://nuget.org/packages/Monads
                var currentId = RenderingContext.CurrentOrNull
                    .With(x => x.Rendering)
                    .With(x => x.Item)
                    .With(x => x.ID)
                    .Return(x => x.Guid, Guid.Empty);

                // if the current id is not null, use it for the datasource
                if (currentId != Guid.Empty)
                    dataSourceId = currentId;
            }

            // do we have an id?
            if (dataSourceId != Guid.Empty)
                return sitecoreService.GetItem(modelType, dataSourceId);

            // do we have a path?
            if (!string.IsNullOrWhiteSpace(dataSourcePath))
                return sitecoreService.GetItem(modelType, dataSourcePath);

            // no item found to bind
            return null;
        }
    }
}
