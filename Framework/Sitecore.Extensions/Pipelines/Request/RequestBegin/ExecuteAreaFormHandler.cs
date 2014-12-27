using Framework.Sc.Extensions.MvcAreas;
using Sitecore.Mvc.Configuration;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Request.RequestBegin;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Routing;

namespace Framework.Sc.Extensions.Pipelines.Request.RequestBegin
{
    /// <summary>
    /// ExecuteAreaFormHandler pipelien for Form Post.
    /// </summary>
    public class ExecuteAreaFormHandler : ExecuteFormHandler
    {
        /// <summary>
        /// Executes the handler.
        /// </summary>
        /// <param name="formValues">The form values.</param>
        /// <param name="args">The arguments.</param>
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

        /// <summary>
        /// Executes the handler.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="args">The arguments.</param>
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
