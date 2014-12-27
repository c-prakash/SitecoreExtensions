using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Framework.Sc.Extensions.Helpers
{
    /// <summary>
    /// Html helper for scripts on partial views.
    /// </summary>
    public static class HtmlHelperScriptExtensions
    {
        /// <summary>
        /// The scriptblock builder constant.
        /// </summary>
        private const string IncludeScriptBlocks = "_IncludeScripts_";

        /// <summary>
        /// Includes the script.
        /// </summary>
        /// <param name="htmlHelper">The html helper.</param>
        /// <param name="template">The webpages helper template.</param>
        /// <returns>Return Html string.</returns>
        public static MvcHtmlString IncludeScriptBlock(this HtmlHelper htmlHelper, Func<object, dynamic> template)
        {
            var context = htmlHelper.ViewContext.HttpContext;
            if (context == null)
                throw new NullReferenceException("HttpContext unavailable.");

            if (!context.Request.IsAjaxRequest())
            {
                var scriptBuilder = context.Items[IncludeScriptBlocks] as List<Func<object, dynamic>> ?? new List<Func<object, dynamic>>();
                scriptBuilder.Add(template);
                context.Items[IncludeScriptBlocks] = scriptBuilder;
                return new MvcHtmlString(string.Empty);
            }

            return new MvcHtmlString(template(null).ToHtmlString());
        }

        public static MvcHtmlString IncludeScript(this HtmlHelper htmlHelper, string scriptFileName)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            
            if (!scriptFileName.StartsWith("~/"))
                scriptFileName = "~/" + scriptFileName;

            var script = new TagBuilder("script");
            script.Attributes["type"] = "text/javascript";
            script.Attributes["src"] = urlHelper.Content(scriptFileName);
            htmlHelper.IncludeScriptBlock(x => new HtmlString(script.ToString(TagRenderMode.Normal)));

            return new MvcHtmlString(string.Empty);
        }


        /// <summary>
        /// Renders the script.
        /// </summary>
        /// <param name="helper">The html helper.</param>
        /// <returns>Return Html string.</returns>
        public static MvcHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.HttpContext.Items[IncludeScriptBlocks] != null)
            {
                List<Func<object, dynamic>> resources = (List<Func<object, dynamic>>)htmlHelper.ViewContext.HttpContext.Items[IncludeScriptBlocks];
                foreach (var resource in resources)
                {
                    if (resource != null) htmlHelper.ViewContext.Writer.Write(resource(null));
                }
            }

            return new MvcHtmlString(string.Empty);
        }
    }
}
