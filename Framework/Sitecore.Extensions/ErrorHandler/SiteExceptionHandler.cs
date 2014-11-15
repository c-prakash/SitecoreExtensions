using System;
using System.Globalization;

namespace Framework.Sc.Extensions.ErrorHandler
{
    /// <summary>
    /// Exception handler for controllers/sitecore.
    /// </summary>
    public class SiteExceptionHandler : BaseExceptionHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteExceptionHandler"/> class.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <exception cref="System.ArgumentNullException">Site name;Site name is null.</exception>
        public SiteExceptionHandler(string siteName)
        {
            if (string.IsNullOrWhiteSpace(siteName))
                throw new ArgumentNullException("siteName", "Site name is null.");

            this.siteName = siteName;
        }

        private readonly string siteName = string.Empty;

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>
        /// Return the Url display the content.
        /// </returns>
        public override string GetErrorUrl(int statusCode)
        {
            // Get Url for specific site.
            return GetSiteErrorUrl(statusCode);
        }

        /// <summary>
        /// Gets the URL by site
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>
        /// Return url based on sitename and status code.
        /// </returns>
        private string GetSiteErrorUrl(int statusCode)
        {
            var statusError = CustomError.Instance.GetSiteErrors(siteName);
            return statusError.FirstOrDefaultStatusUrl(statusCode.ToString(CultureInfo.InvariantCulture));
        }
    }
}
