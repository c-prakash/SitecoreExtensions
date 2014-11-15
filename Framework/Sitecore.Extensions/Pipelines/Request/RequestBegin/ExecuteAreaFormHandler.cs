using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Request.RequestBegin;
using System;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Presentation;
using System.Web.Routing;
using Framework.Sc.Extensions.MvcAreas;

namespace Framework.Sc.Extensions.Pipelines.Request.RequestBegin
{
    public class ExecuteAreaFormHandler: ExecuteFormHandler
    {
        protected override void ExecuteHandler(System.Collections.Specialized.NameValueCollection formValues, RequestBeginArgs args)
        {
            string str = formValues["scController"].OrIfEmpty(MvcSettings.DefaultFormControllerName);
            string item = formValues["scAction"];
            string areaName = formValues["scArea"];
            string namespaceName = formValues["scNamespace"];
            Tuple<string, string> controllerAndAction = MvcSettings.ControllerLocator.GetControllerAndAction(str, item);
            if (controllerAndAction == null)
            {
                return;
            }

            ExecuteHandler(controllerAndAction.Item1, controllerAndAction.Item2, areaName, namespaceName, args);
        }

        protected virtual void ExecuteHandler(string controllerName, string actionName, string areaName, string namespaceName, RequestBeginArgs args)
        {
            string str = (new AreaControllerRunner(controllerName, actionName, areaName, namespaceName)).Execute();
            if (str.IsEmptyOrNull())
            {
                return;
            }

            RequestContext requestContext = PageContext.Current.RequestContext;
            requestContext.HttpContext.Response.Output.Write(str);
            requestContext.RouteData.Values["scOutputGenerated"] = "1";
        }
    }
}
