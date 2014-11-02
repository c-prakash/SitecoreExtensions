
namespace Infrastructure.Pipelines.MvcEvents
{
    public class ExceptionProcessor : Sitecore.Mvc.Pipelines.MvcEvents.Exception.ExceptionProcessor
    {
        public override void Process(Sitecore.Mvc.Pipelines.MvcEvents.Exception.ExceptionArgs args)
        {
            var exceptionContext = args.ExceptionContext;
            var httpContext = exceptionContext.HttpContext;

            // Bail if we can't do anything; propogate the error for further processing. 
            if (exceptionContext == null || httpContext == null)
                return;

            if (exceptionContext.ExceptionHandled)
                return;

            exceptionContext.ExceptionHandled = true;
            var innerException = exceptionContext.Exception;

            Infrastructure.Error.ExceptionHandler.HandleException(innerException, httpContext);
        }
    }
}
