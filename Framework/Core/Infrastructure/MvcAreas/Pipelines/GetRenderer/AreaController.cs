/*
 --------------------------------------------------------------
 ***** Source code taken from *****
 http://webcmd.wordpress.com/2013/01/24/sitecore-mvc-area-controller-rendering-type/
 --------------------------------------------------------------
 */

using Sitecore.Data.Templates;
using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Sitecore.Mvc.Presentation;

namespace Infrastructure.MvcAreas.Pipelines.GetRenderer
{
    public class AreaController : GetRendererProcessor
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
            if (!renderingTemplate.DescendsFromOrEquals(new Sitecore.Data.ID(TemplateId)))
            {
                return;
            }
            args.Result = this.GetRenderer(args.Rendering, args);
        }

        protected virtual Renderer GetRenderer(Rendering rendering, GetRendererArgs args)
        {
            string action = rendering.RenderingItem.InnerItem.Fields["controller action"].GetValue(true);
            string controller = rendering.RenderingItem.InnerItem.Fields["controller"].GetValue(true);
            string area = rendering.RenderingItem.InnerItem.Fields["area"].GetValue(true);
            string namespaceNames = rendering.RenderingItem.InnerItem.Fields["namespaces"].GetValue(true);

            return new AreaControllerRenderer
            {
                Action = action,
                Controller = controller,
                Area = area,
                Namespaces = namespaceNames
            };
        }
    }
}
