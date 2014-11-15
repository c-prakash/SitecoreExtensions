/*
 --------------------------------------------------------------
 ***** Concept from *****
  http://blog.horizontalintegration.com/2014/03/26/sitecore-analytics-disable-page-views-for-mvc-routes/
 --------------------------------------------------------------
 */

using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Configuration;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Request.RequestBegin;
using Sitecore.Sites;
using Sitecore.Web;
using System.Web;
using System.Web.Routing;

namespace Framework.Sc.Extensions.Pipelines.Request.RequestBegin
{
    public class DisableAjaxMvcAnalyticsTracking : RequestBeginProcessor
    {
        /// <summary>
        /// The Site-core disable analytics key.
        /// </summary>
        private const string DisableAjaxMvcAnalyticsKey = "DisableAjaxMvcAnalytics";

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(RequestBeginArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (!AnalyticsSettings.Enabled)
            {
                return;
            }

            var site = Context.Site;
            if (site == null || !site.EnableAnalytics || site.DisplayMode != DisplayMode.Normal)
            {
                return;
            }

            // get the current route data from the http context
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));
            if (routeData != null)
            {
                // get the specific route values based off your current route/url
                var routeValueDictionary =
                    (routeData.Route as Route).ValueOrDefault(r => r.Defaults);
                if (routeValueDictionary != null)
                {
                    bool disableAnalytics;

                    // check if scDisableAnalyticsKey is one of those values in the route
                    if (bool.TryParse(Settings.GetSetting(DisableAjaxMvcAnalyticsKey), out disableAnalytics))
                    {
                        if (disableAnalytics)
                        {
                            // stop Tracker.StartTracking() line below from executing in the mvc.requestBegin pipeline
                            return;
                        }
                    }
                }
            }

            // If everything fine start tracking
            Tracker.IsActive = false;
            Tracker.StartTracking();
            if (!Tracker.IsActive)
            {
                return;
            }

            var currentVisit = Tracker.CurrentVisit;
            if (!string.IsNullOrEmpty(currentVisit.AspNetSessionId))
            {
                return;
            }

            currentVisit.AspNetSessionId = WebUtil.GetSessionID();
        }
    }
}
