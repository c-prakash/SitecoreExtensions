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
            this.Area = area;
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.Namespace = namespaceName;
        }

        public string Area { get; set; }
        public string Namespace { get; set; }

        protected override void ExecuteController(System.Web.Mvc.Controller controller)
        {
            RequestContext requestContext = PageContext.Current.RequestContext;
            object value = requestContext.RouteData.Values["controller"];
            object value2 = requestContext.RouteData.Values["action"];
            object value3 = requestContext.RouteData.DataTokens["area"];
            object value4 = requestContext.RouteData.DataTokens["namespace"];

            try
            {
                requestContext.RouteData.Values["controller"] = this.ActualControllerName;
                requestContext.RouteData.Values["action"] = this.ActionName;
                requestContext.RouteData.DataTokens["area"] = this.Area;

                var namespaces = new string[] { string.Empty };
                if (!string.IsNullOrWhiteSpace(this.Namespace))
                    namespaces = this.Namespace.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

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