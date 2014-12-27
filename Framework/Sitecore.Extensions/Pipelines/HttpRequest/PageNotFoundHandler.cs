using System.Globalization;
using Framework.Sc.Extensions.ErrorHandler;
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
    /// <summary>
    /// 404 process for Sitecore.
    /// </summary>
    public class PageNotFoundHandler : HttpRequestProcessor
    {
        /// <summary>
        /// The page not found
        /// </summary>
        public const string PageNotFound = "404ErrorPage";

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(HttpRequestArgs args)
        {
            if (Context.Item != null)
            {
                if (!(Context.Item.Visualization.Layout == null
                    && string.IsNullOrEmpty(WebUtil.GetQueryString("sc_layout"))))
                    return;
            }

            if (args.LocalPath.StartsWith("/sitecore", System.StringComparison.OrdinalIgnoreCase) || args.Url.FilePath.StartsWith("/sitecore", System.StringComparison.OrdinalIgnoreCase) || IsLocalFile(args.Context, args.Url.FilePath))
                return;

            var pageNotFoundUrl = GetPageNotFoundUrl(Context.Site);
            Context.Item = GetPageNotFoundItem(Context.Site, pageNotFoundUrl);

            if (Context.Item == null)
                return;

            HttpContext.Current.Items[PageNotFound] = true;
        }

        /// <summary>
        /// Determines whether [is local file] [the specified context].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>Return true or false after file exist check.</returns>
        protected virtual bool IsLocalFile(HttpContext context, string filePath)
        {
            return File.Exists(context.Server.MapPath(filePath));
        }

        /// <summary>
        /// Gets the item by short path.
        /// </summary>
        /// <param name="siteContext">The site context.</param>
        /// <param name="shortPath">The short path.</param>
        /// <returns>Return Sitecore Item based on path supplied.</returns>
        protected Item GetItemByShortPath(SiteContext siteContext, string shortPath)
        {
            shortPath = shortPath.StartsWith("/") ? shortPath.Substring(1) : shortPath;

            var fullPath = string.Concat(StringUtil.EnsurePostfix('/', siteContext.StartPath), shortPath);
            return siteContext.Database.GetItem(fullPath);
        }

        /// <summary>
        /// Gets the page not found URL.
        /// </summary>
        /// <param name="siteContext">The site context.</param>
        /// <returns>Return page not found url from configuration.</returns>
        public string GetPageNotFoundUrl(SiteContext siteContext)
        {
            var siteError = CustomError.Instance.GetSiteErrors(siteContext.Name);
            return siteError.FirstOrDefaultStatusUrl(((int)System.Net.HttpStatusCode.NotFound).ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Gets the page not found item.
        /// </summary>
        /// <param name="siteContext">The site context.</param>
        /// <param name="pageNotFoundUrl">The page not found URL.</param>
        /// <returns>Return page not found item based on url.</returns>
        public Item GetPageNotFoundItem(SiteContext siteContext, string pageNotFoundUrl)
        {
            if (ID.IsID(pageNotFoundUrl) || pageNotFoundUrl.StartsWith(Constants.ContentPath))
                return siteContext.Database.GetItem(pageNotFoundUrl);

            return GetItemByShortPath(siteContext, pageNotFoundUrl);
        }
    }
}
