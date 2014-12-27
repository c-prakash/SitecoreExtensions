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

            httpContext.Server.TransferRequest(redirectUrl, true);
        }
    }
}
