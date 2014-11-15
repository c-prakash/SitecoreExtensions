/*
 --------------------------------------------------------------
 ***** Source code from *****
 http://webcmd.wordpress.com/2013/01/24/sitecore-mvc-area-controller-rendering-type/
 --------------------------------------------------------------
 */

using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;
using System.Web.Routing;

namespace Framework.Sc.Extensions.MvcAreas
{
    public class AreaControllerRunner : ControllerRunner
    {
        public AreaControllerRunner(string controllerName, string actionName, string area, string namespaceName)
            : base(controllerName, actionName)
        {
            Area = area;
            ControllerName = controllerName;
            ActionName = actionName;
            Namespace = namespaceName;
        }

        public string Area { get; set; }
        public string Namespace { get; set; }

        protected override void ExecuteController(Controller controller)
        {
            RequestContext requestContext = PageContext.Current.RequestContext;
            var value = requestContext.RouteData.Values["controller"];
            var value2 = requestContext.RouteData.Values["action"];
            var value3 = requestContext.RouteData.DataTokens["area"];
            var value4 = requestContext.RouteData.DataTokens["namespace"];

            try
            {
                requestContext.RouteData.Values["controller"] = ActualControllerName;
                requestContext.RouteData.Values["action"] = ActionName;
                requestContext.RouteData.DataTokens["area"] = Area;

                var namespaces = new[] { string.Empty };
                if (!string.IsNullOrWhiteSpace(Namespace))
                    namespaces = Namespace.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                requestContext.RouteData.DataTokens["namespace"] = namespaces;
                ((IController)controller).Execute(PageContext.Current.RequestContext);
            }
            finally
            {
                requestContext.RouteData.Values["controller"] = value;
                requestContext.RouteData.Values["action"] = value2;
                requestContext.RouteData.DataTokens["area"] = value3;
                requestContext.RouteData.DataTokens["namespace"] = value4;
            }
        }
    }
}