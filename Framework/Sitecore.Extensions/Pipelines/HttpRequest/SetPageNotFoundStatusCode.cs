using Sitecore.Pipelines.HttpRequest;
using System.Net;

namespace Framework.Sc.Extensions.Pipelines.HttpRequest
{
    public class SetNotFoundStatusCode : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (args == null)
                return;

            var context = args.Context;
            
            if (!(context.Items["404ErrorPage"] != null && (bool)context.Items["404ErrorPage"]))
                return;

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.StatusDescription = "Page not found";
            context.Response.TrySkipIisCustomErrors = true;
        }
    }
}
