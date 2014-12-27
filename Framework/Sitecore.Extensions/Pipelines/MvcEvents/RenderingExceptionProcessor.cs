using System.Web;
using Framework.Sc.Extensions.ErrorHandler;
using Sitecore.Mvc.Pipelines.MvcEvents.Exception;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Pipelines.MvcEvents
{
    /// <summary>
    /// Exception processor pipeline for Sitecore MVC.
    /// </summary>
    public class RenderingExceptionProcessor : ExceptionProcessor
    {
        /// <summary>
        /// Gets or sets a value indicating whether [custom behaviour configured].
        /// </summary>
        /// <value>
        /// <c>true</c> if [custom behaviour configured]; otherwise, <c>false</c>.
        /// </value>
        public bool CustomRenderingBehaviourRequired { get; set; }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(ExceptionArgs args)
        {
            HandleError(args.ExceptionContext);
        }

        /// <summary>
        /// Handles the error.
        /// </summary>
        /// <param name="exceptionContext">The exception context.</param>
        protected virtual void HandleError(ExceptionContext exceptionContext)
        {
            var httpContext = exceptionContext.HttpContext;

            // Bail if we can't do anything; propogate the error for further processing. 
            if (httpContext == null)
                return;

            if (!CustomRenderingBehaviourRequired)
                return;

            if (exceptionContext.ExceptionHandled)
                return;

            if (!httpContext.IsCustomErrorEnabled)
                return;
    
            exceptionContext.ExceptionHandled = true;
            var innerException = exceptionContext.Exception;
            ExceptionHandlerFactory.Create().HandleException(innerException, httpContext);
        }
    }
}
