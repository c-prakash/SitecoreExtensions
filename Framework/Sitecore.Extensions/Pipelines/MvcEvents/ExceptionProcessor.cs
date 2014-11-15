using System.Web;
using Framework.Sc.Extensions.ErrorHandler;
using Sitecore.Mvc.Pipelines.MvcEvents.Exception;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Pipelines.MvcEvents
{
    /// <summary>
    /// Exception processor pipeline for Sitecore MVC.
    /// </summary>
    public class ExceptionProcessorExtension : ExceptionProcessor
    {
        /// <summary>
        /// Gets or sets a value indicating whether [show exceptions to administrators].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show exceptions to administrators]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowExceptionsToAdministrators { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show exceptions in page editor].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show exceptions in page editor]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowExceptionsInPageEditor { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show exceptions in preview].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show exceptions in preview]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowExceptionsInPreview { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show exceptions in debugger].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show exceptions in debugger]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowExceptionsInDebugger { get; set; }
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

            if (!ShouldRenderErrors(httpContext))
                return;

            if (!CustomRenderingBehaviourRequired)
                return;

            if (exceptionContext.ExceptionHandled)
                return;

            if (httpContext.IsCustomErrorEnabled)
                exceptionContext.ExceptionHandled = true;

            var innerException = exceptionContext.Exception;
            ExceptionHandlerFactory.Create().HandleException(innerException, httpContext);
        }

        /// <summary>
        /// Shoulds the render errors.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Return true or false based on the flags configured.</returns>
        protected bool ShouldRenderErrors(HttpContextBase httpContext)
        {
            return httpContext.IsCustomErrorEnabled
              || (ShowExceptionsToAdministrators && Sitecore.Context.User.IsAdministrator)
              || (ShowExceptionsInPageEditor && Sitecore.Context.PageMode.IsPageEditor)
              || (ShowExceptionsInPreview && Sitecore.Context.PageMode.IsPreview)
              || (ShowExceptionsInDebugger && Sitecore.Context.PageMode.IsDebugging);
        }
    }
}
