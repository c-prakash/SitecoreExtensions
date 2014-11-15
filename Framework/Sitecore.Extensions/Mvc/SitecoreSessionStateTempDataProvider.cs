using Sitecore.Mvc.Configuration;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc
{
    /// <summary>
    /// Temp data provider for sitecore, this ignores the temp data access in Sitecore controller.
    /// </summary>
    public class SitecoreSessionStateTempDataProvider : SessionStateTempDataProvider
    {
        /// <summary>
        /// The temporary data session state key
        /// </summary>
        internal const string TempDataSessionStateKey = "__ControllerTempData";

        /// <summary>
        /// Saves the specified values in the temporary data dictionary by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="values">The values.</param>
        public override void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            var controller = controllerContext.RouteData.Values["controller"] as string;
            if (!string.IsNullOrWhiteSpace(controller) &&
                controller.Equals(MvcSettings.SitecoreControllerName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            base.SaveTempData(controllerContext, values);
        }

        /// <summary>
        /// Loads the temporary data by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <returns>
        /// The temporary data.
        /// </returns>
        public override IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            var emptyDictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            var controller = controllerContext.RouteData.Values["controller"] as string;
            if (!string.IsNullOrWhiteSpace(controller) &&
                controller.Equals(MvcSettings.SitecoreControllerName, StringComparison.OrdinalIgnoreCase))
            {
                return emptyDictionary;
            }

            var context = controllerContext.HttpContext;
            var requestItems = context.Items[TempDataSessionStateKey] as IDictionary<string, object>;
            if (requestItems == null || requestItems.Count <= 0)
                context.Items[TempDataSessionStateKey] = requestItems = base.LoadTempData(controllerContext);

            return requestItems ?? emptyDictionary;
        }
    }
}
