using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc.Filters
{
    public class ImportResult : ResultTransfer
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.RequestType.Equals("GET", System.StringComparison.OrdinalIgnoreCase))
                return;

            var requestItems = filterContext.HttpContext.Items;

            var resultKey = this.CreateResultKey(filterContext, filterContext.ActionDescriptor.ActionName);
            var originalRequestType = requestItems[OriginalRequestTypeKey] as string;

            if ("POST".Equals(originalRequestType, System.StringComparison.OrdinalIgnoreCase) && requestItems[resultKey] != null)
                filterContext.Result = requestItems[resultKey] as ActionResult;

            base.OnActionExecuting(filterContext);
        }
    }
}
