using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc.Filters
{
    /// <summary>
    /// Export model state to store for processing in GET request.
    /// </summary>
    public class ExportResult : ResultTransfer
    {
        /// <summary>
        /// Gets or sets the name of the import method.
        /// </summary>
        /// <value>
        /// The name of the import method.
        /// </value>
        public string ImportMethodName { get; set; }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Don't Export if we are redirecting
            if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                return;

            if (!filterContext.HttpContext.Request.RequestType.Equals("POST", System.StringComparison.OrdinalIgnoreCase))
                return;

            // Keep the result from post method
            var resultKey = this.CreateResultKey(filterContext, string.IsNullOrWhiteSpace(ImportMethodName) ? filterContext.ActionDescriptor.ActionName : ImportMethodName);
            filterContext.HttpContext.Items[resultKey] = filterContext.Result;

            // Start the GET request for page
            filterContext.HttpContext.Items[OriginalRequestTypeKey] = filterContext.HttpContext.Request.RequestType;
            this.SetHttpMethod("GET");
            
            IView pageView = PageContext.Current.PageView;
            filterContext.Result = new ViewResult { View = pageView };
            
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Restore the original request type
            var originalRequestType = filterContext.HttpContext.Items[OriginalRequestTypeKey] as string;
            if (!string.IsNullOrWhiteSpace(originalRequestType))
                this.SetHttpMethod(originalRequestType);

            base.OnResultExecuted(filterContext);
        }

        private void SetHttpMethod(string httpMethod)
        {
            var a = System.Web.HttpContext.Current.Request.GetType().GetField("_httpMethod", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            a.SetValue(System.Web.HttpContext.Current.Request, httpMethod);
            System.Web.HttpContext.Current.Request.RequestType = httpMethod;
        }
    }
}
