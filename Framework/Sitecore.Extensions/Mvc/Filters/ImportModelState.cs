using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc.Filters
{
    /// <summary>
    /// Import Model State to add into view result after GET processing
    /// </summary>
    public class ImportModelState : ModelStateTransfer
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var privateKey = Key + "_" + filterContext.Controller.GetType().Name + "_" + filterContext.ActionDescriptor.ActionName;

            var modelState = filterContext.HttpContext.Session[privateKey] as ModelStateDictionary;
            if (modelState != null)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                
                //Otherwise remove it.
                filterContext.HttpContext.Session.Remove(privateKey);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
