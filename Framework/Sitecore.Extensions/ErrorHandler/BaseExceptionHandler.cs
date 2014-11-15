using Sitecore.Mvc.Presentation;
using System;
using System.Web;

namespace Framework.Sc.Extensions.ErrorHandler
{
    /// <summary>
    /// Base exception handler for exception handling.
    /// </summary>
    public abstract class BaseExceptionHandler
    {
        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>Return the Url display the content.</returns>
        public abstract string GetErrorUrl(int statusCode);

        /// <summary>
        /// Renders the content of the error.
        /// </summary>
        /// <param name="urlToRender">The error URL.</param>
        /// <returns>Return the Url content to write on respons stream.</returns>
        protected virtual string RenderUrlContent(string urlToRender)
        {
            using (var textWriter = new System.IO.StringWriter())
            {
                var urlRenderer = new UrlRenderer { Url = urlToRender };
                urlRenderer.Render(textWriter);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Writes the response.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="responseString">The response string.</param>
        protected virtual void WriteResponse(HttpContextBase httpContext, int statusCode, string responseString)
        {
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.TrySkipIisCustomErrors = true;
            httpContext.Server.ClearError();
            httpContext.Response.Write(responseString);
            //httpContext.Response.Flush();
            //httpContext.Response.End();
            //httpContext.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="httpContext">The HTTP context.</param>
        public virtual void HandleException(Exception exception, HttpContextBase httpContext)
        {
            if (httpContext == null)
                return;

            if (!httpContext.IsCustomErrorEnabled)
                return;

            if (httpContext.Response.IsRequestBeingRedirected)
                return;

            var httpException = exception as HttpException;
            var statusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            
            if (httpException != null)
                statusCode = httpException.GetHttpCode();

            var redirectUrl = GetErrorUrl(statusCode);

            if (string.IsNullOrWhiteSpace(redirectUrl))
                throw new NullReferenceException("Error Url for status code is null.");

            // Call out the page and pass it in response stream
            var errorPageContent = RenderUrlContent(redirectUrl);
            WriteResponse(httpContext, statusCode, errorPageContent);
        }
    }
}
