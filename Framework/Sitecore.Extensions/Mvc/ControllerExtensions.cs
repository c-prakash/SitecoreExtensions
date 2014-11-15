using Framework.Sc.Extensions.BaseModel;
using System;
using System.Web;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc
{
    /// <summary>
    /// Mvc controller extension method.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Redirects the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="url">The URL.</param>
        /// <param name="model">The model.</param>
        /// <returns>Return the redirect result with redirect url.</returns>
        /// <exception cref="System.ArgumentException">Value can not be null or empty.;url</exception>
        public static RedirectResult Redirect<T>(this Controller controller, string url, T model) where T : FormModel
        {
            if (string.IsNullOrEmpty(url))
                url = controller.HttpContext.Request.RawUrl;

            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("Value can not be null or empty.", "url");

            if (model != null)
            {
                if (GetRequestType(controller.HttpContext) == HttpVerbs.Post)
                    model.IsPost = true;

                // controller.ViewData.Model = model;
                controller.TempData[model.GetType().Name] = model;
            }

            return new RedirectResult(url);
        }

        /// <summary>
        /// Redirects the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="model">The model.</param>
        /// <returns>Return the redirect result with redirect url.</returns>
        public static RedirectResult Redirect<T>(this Controller controller, T model) where T : FormModel
        {
            var url = controller.HttpContext.Request.RawUrl;
            return controller.Redirect(url, model);
        }

        /// <summary>
        /// Redirects the specified controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns>Return the redirect result with redirect url.</returns>
        public static RedirectResult Redirect(this Controller controller)
        {
            return controller.Redirect(string.Empty, (FormModel)null);
        }

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Return HTTP request type of executing request.</returns>
        private static HttpVerbs GetRequestType(HttpContextBase httpContext)
        {
            HttpVerbs httpVerb;
            if (!Enum.TryParse(httpContext.Request.RequestType, true, out httpVerb))
            {
                return default(HttpVerbs);
            }

            return httpVerb;
        }
    }
}
