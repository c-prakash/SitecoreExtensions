using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;
using Sitecore.Web;
using System.IO;
using System.Web;

namespace Framework.Sc.Extensions.Pipelines.HttpRequest
{
    public class PageNotFoundHandler : HttpRequestProcessor
    {
        public const string PageNotFound = "404ErrorPage";

        public override void Process(HttpRequestArgs args)
        {
            if (Context.Item != null)
            {
                if (!(Context.Item.Visualization.Layout == null
                    && string.IsNullOrEmpty(WebUtil.GetQueryString("sc_layout"))))
                    return;
            }

            if(args.LocalPath.StartsWith("/sitecore")|| IsLocalFile(args.Context, args.Url.FilePath))
                return;

            if (string.IsNullOrEmpty(Context.Site.Properties[PageNotFound]))
                return;

            Context.Item = GetPageNotFoundItem(Context.Site);

            if (Context.Item == null)
                return;

            HttpContext.Current.Items[PageNotFound] = true;
        }

        protected virtual bool IsLocalFile(HttpContext context, string filePath)
        {
            return File.Exists(context.Server.MapPath(filePath));
        }

        protected Item GetItemByShortPath(SiteContext siteContext, string shortPath)
        {
            shortPath = shortPath.StartsWith("/") ? shortPath.Substring(1) : shortPath;
            
            var fullPath = string.Concat(StringUtil.EnsurePostfix('/', siteContext.StartPath), shortPath);
            return siteContext.Database.GetItem(fullPath);
        }

        public Item GetPageNotFoundItem(SiteContext siteContext)
        {
            var propertyValue = siteContext.Properties[PageNotFound];
            
            if (string.IsNullOrEmpty(propertyValue))
                return null;
            
            if (ID.IsID(propertyValue) || propertyValue.StartsWith(Constants.ContentPath))
                return siteContext.Database.GetItem(propertyValue);
            
            return GetItemByShortPath(siteContext, propertyValue);
        }
    }
}
