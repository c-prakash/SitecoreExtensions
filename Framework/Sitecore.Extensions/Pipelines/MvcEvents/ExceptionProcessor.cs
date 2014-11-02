using Framework.Sc.Extensions.ErrorHandler;
using Sitecore.Mvc.Pipelines.MvcEvents.Exception;

namespace Framework.Sc.Extensions.Pipelines.MvcEvents
{
    public class ExceptionProcessorExtension : ExceptionProcessor
    {
        public override void Process(ExceptionArgs args)
        {
            var exceptionContext = args.ExceptionContext;
            var httpContext = exceptionContext.HttpContext;

            // Bail if we can't do anything; propogate the error for further processing. 
            if (exceptionContext == null || httpContext == null)
                return;

            if (!httpContext.IsCustomErrorEnabled)
                return;

            if (exceptionContext.ExceptionHandled)
                return;

            exceptionContext.ExceptionHandled = true;
            var innerException = exceptionContext.Exception;

            new ExceptionHandler().HandleException(innerException, httpContext);
        }
    }
}
