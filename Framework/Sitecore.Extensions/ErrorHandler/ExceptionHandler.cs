using Framework.Core.Infrastructure.Logging;
using Sitecore.Mvc.Presentation;
using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace Framework.Sc.Extensions.ErrorHandler
{
    /// <summary>
    /// Global Exception Handler class.
    /// </summary>
    public class ExceptionHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        public ExceptionHandler()
            : this(ConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        /// <param name="errorSection">The error section.</param>
        public ExceptionHandler(CustomErrorsSection errorSection)
        {
            this.errorSection = errorSection;
        }

        /// <summary>
        /// The error section
        /// </summary>
        private readonly CustomErrorsSection errorSection;

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="httpContext">The HTTP context.</param>
        public void HandleException(System.Exception exception, HttpContextBase httpContext)
        {
            if (httpContext == null)
                return;

            HttpException httpException = exception as HttpException;
            int statusCode = (int)HttpStatusCode.InternalServerError;

            if (!httpContext.IsCustomErrorEnabled)
                return;

            if (httpContext.Response.IsRequestBeingRedirected)
                return;

            var logError = true;

            // Skip Page Not Found and Service not unavailable from logging
            if (httpException != null)
            {
                statusCode = httpException.GetHttpCode();
                if (statusCode == (int)HttpStatusCode.NotFound || statusCode == (int)HttpStatusCode.ServiceUnavailable)
                {
                    logError = false;
                }
            }

            try
            {
                if (logError)
                    Logger.Exception(exception, exception.GetExceptionInfo());

                var redirectUrl = GetStatusPage(this.errorSection, statusCode);
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = statusCode;
                httpContext.Response.TrySkipIisCustomErrors = true;
                httpContext.ClearError();

                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    // Call out the page and pass it in response stream
                    var errorPageContent = RenderUrlContent(redirectUrl);
                    httpContext.Response.Write(errorPageContent);
                }
            }
            catch(Exception ex)
            {
                Logger.Exception(ex, exception.GetExceptionInfo());
                httpContext.Response.Redirect(GetStatusPage(this.errorSection, 500), true);
            }
        }

        /// <summary>
        /// Gets the redirect page.
        /// </summary>
        /// <param name="errorSection">The error section.</param>
        /// <param name="statusCode">The status code.</param>
        /// <returns>Return the status page based on status code.</returns>
        private string GetStatusPage(CustomErrorsSection errorSection, int statusCode)
        {
            var errorPage = errorSection.DefaultRedirect;
            try
            {
                if (errorSection.Errors.Count > 0)
                {
                    var errorItem = errorSection.Errors.Get(statusCode.ToString(CultureInfo.CurrentCulture));
                    if (errorItem != null)
                        errorPage = errorItem.Redirect;
                }
            }
            catch
            { }

            return errorPage;
        }

        /// <summary>
        /// Renders the content of the error.
        /// </summary>
        /// <param name="urlToRender">The error URL.</param>
        /// <returns></returns>
        public static string RenderUrlContent(string urlToRender)
        {
            using (var textWriter = new System.IO.StringWriter())
            {
                var urlRenderer = new UrlRenderer() { Url = urlToRender };
                urlRenderer.Render(textWriter);
                return textWriter.ToString();
            }
        }
    }
}
