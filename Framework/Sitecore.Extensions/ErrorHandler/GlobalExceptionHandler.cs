using System.Configuration;
using System.Globalization;
using System.Web.Configuration;

namespace Framework.Sc.Extensions.ErrorHandler
{
    /// <summary>
    /// Global Exception Handler class.
    /// </summary>
    public class GlobalExceptionHandler : BaseExceptionHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
        /// </summary>
        public GlobalExceptionHandler()
            : this(ConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
        /// </summary>
        /// <param name="errorSection">The error section.</param>
        public GlobalExceptionHandler(CustomErrorsSection errorSection)
            : this(SiteErrors.ConvertFrom(errorSection))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
        /// </summary>
        /// <param name="statusErrors">The custom errors.</param>
        public GlobalExceptionHandler(SiteErrors statusErrors)
        {
            this.statusErrors = statusErrors;
        }

        /// <summary>
        /// The error section
        /// </summary>
        private readonly SiteErrors statusErrors;

        /// <summary>
        /// Gets the redirect page.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns>
        /// Return the status page based on status code.
        /// </returns>
        public override string GetErrorUrl(int statusCode)
        {
            return statusErrors.FirstOrDefaultStatusUrl(statusCode.ToString(CultureInfo.InvariantCulture));
        }
    }
}
