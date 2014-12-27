using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc.Filters
{
    /// <summary>
    /// Export model state to store for processing in GET request.
    /// </summary>
    public class ExportModelState : ModelStateTransfer
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
            //Only export when ModelState is not valid
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                //Export if we are redirecting
                if ((filterContext.Result is RedirectResult) || (filterContext.Result is RedirectToRouteResult))
                {
                    ImportMethodName = string.IsNullOrWhiteSpace(ImportMethodName) ? filterContext.ActionDescriptor.ActionName : ImportMethodName;
                    var privateKey = Key + "_" + filterContext.Controller.GetType().Name + "_" + ImportMethodName;
                    filterContext.HttpContext.Session[privateKey] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
