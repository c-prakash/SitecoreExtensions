/*
 --------------------------------------------------------------
 ***** Source code from *****
 http://webcmd.wordpress.com/2013/01/24/sitecore-mvc-area-controller-rendering-type/
 --------------------------------------------------------------
 */

using Sitecore.Data;
using Sitecore.Data.Templates;
using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Sitecore.Mvc.Presentation;

namespace Framework.Sc.Extensions.MvcAreas.Pipelines.GetRenderer
{
    public class AreaControllerRendererProcessor : GetRendererProcessor
    {
        public virtual string TemplateId { get; set; }

        public override void Process(GetRendererArgs args)
        {
            if (args.Result != null)
            {
                return;
            }
            
            Template renderingTemplate = args.RenderingTemplate;
            if (renderingTemplate == null)
            {
                return;
            }
            if (!renderingTemplate.DescendsFromOrEquals(new ID(TemplateId)))
            {
                return;
            }
            args.Result = GetRenderer(args.Rendering, args);
        }

        protected virtual Renderer GetRenderer(Rendering rendering, GetRendererArgs args)
        {
            string action = rendering.RenderingItem.InnerItem.Fields["controller action"].GetValue(true);
            string controller = rendering.RenderingItem.InnerItem.Fields["controller"].GetValue(true);
            var area = rendering.RenderingItem.InnerItem.Fields["area"];
            string areaName = area != null ? area.GetValue(true) : string.Empty;
            var namespaceName = rendering.RenderingItem.InnerItem.Fields["namespace"];
            string namespaceNames = namespaceName != null ? namespaceName.GetValue(true) : string.Empty;

            return new AreaControllerRenderer
            {
                Action = action,
                Controller = controller,
                Area = areaName,
                Namespaces = namespaceNames
            };
        }
    }
}
