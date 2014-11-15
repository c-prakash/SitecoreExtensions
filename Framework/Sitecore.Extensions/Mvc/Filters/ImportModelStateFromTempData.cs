using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc.Filters
{
    /// <summary>
    /// Import ModelState From TempData
    /// </summary>
    public class ImportModelStateFromTempData : ModelStateTempDataTransfer
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            LoadParameterValuesFromTempData(filterContext);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;
            if (modelState != null)
            {
                //Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    //Otherwise remove it.
                    filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Loads the parameter values from temporary data.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private void LoadParameterValuesFromTempData(ActionExecutingContext filterContext)
        {
            foreach (var parameterValue in GetStoredParameterValues(filterContext))
            {
                filterContext.ActionParameters[GetParameterName(parameterValue.Key)] = parameterValue.Value;
            }
        }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Return parameter name based search key.</returns>
        private string GetParameterName(string key)
        {
            if (key.StartsWith(Key))
            {
                return key.Substring(Key.Length);
            }

            return key;
        }

        /// <summary>
        /// Gets the stored parameter values.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>Return parameter keyvalye pair from tempdata.</returns>
        private IEnumerable<KeyValuePair<string, object>> GetStoredParameterValues(ActionExecutingContext filterContext)
        {
            return filterContext.Controller.TempData.Where(td => td.Key.StartsWith(Key)).ToList();
        }
    }
}
