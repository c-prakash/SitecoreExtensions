
namespace Framework.Sc.Extensions.ErrorHandler
{
    /// <summary>
    /// Factory for exception handler.
    /// </summary>
    public class ExceptionHandlerFactory
    {
        /// <summary>
        /// Creates the specified site name.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <returns>Return exception handler instance.</returns>
        public static BaseExceptionHandler Create(string siteName)
        {
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                return new SiteExceptionHandler(siteName);
            }

            return new GlobalExceptionHandler();
        }

        /// <summary>
        /// Creates the specified site name.
        /// </summary>
        /// <returns>Return exception handler instance.</returns>
        public static BaseExceptionHandler Create()
        {
            string siteName;
            try
            {
                var site = Sitecore.Context.Site;
                siteName = site.Name;
            }
            catch { siteName = string.Empty; }

            return Create(siteName);
        }
    }
}
