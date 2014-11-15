using Sitecore.Configuration;
using Sitecore.Xml;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Xml;

namespace Framework.Sc.Extensions.ErrorHandler
{
    /// <summary>
    /// Error object for status code and url mapping.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
    }

    /// <summary>
    /// Site and errors mapping.
    /// </summary>
    public class SiteErrors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiteErrors"/> class.
        /// </summary>
        public SiteErrors()
        {
            Errors = new List<Error>();
        }

        /// <summary>
        /// Gets or sets the default URL.
        /// </summary>
        /// <value>
        /// The default URL.
        /// </value>
        public string DefaultUrl { get; set; }

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public List<Error> Errors { get; set; }

        /// <summary>
        /// Exists the specified status code.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns></returns>
        public bool Exist(string statusCode)
        {
            return Errors.Exists(e => e.StatusCode == statusCode);
        }

        /// <summary>
        /// Firsts the status URL.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns></returns>
        public string FirstStatusUrl(string statusCode)
        {
            return Errors.Find(e => e.StatusCode == statusCode).Url;
        }

        /// <summary>
        /// Firsts the or default status URL.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <returns></returns>
        public string FirstOrDefaultStatusUrl(string statusCode)
        {
            return Exist(statusCode) ? FirstStatusUrl(statusCode) : DefaultUrl;
        }

        /// <summary>
        /// Converts from.
        /// </summary>
        /// <param name="customErrorsSection">The custom errors section.</param>
        /// <returns></returns>
        public static SiteErrors ConvertFrom(CustomErrorsSection customErrorsSection)
        {
            var customErrors = new SiteErrors { DefaultUrl = customErrorsSection.DefaultRedirect };
            foreach (var key in customErrorsSection.Errors.AllKeys)
            {
                customErrors.Errors.Add(new Error { StatusCode = key, Url = customErrorsSection.Errors.Get(key).Redirect });
            }

            return customErrors;
        }
    }

    /// <summary>
    /// Custom error class to retrieve site error configuration.
    /// </summary>
    public sealed class CustomError
    {
        /// <summary>
        /// The instance.
        /// </summary>
        public static CustomError Instance = new CustomError();

        /// <summary>
        /// The site errors
        /// </summary>
        private readonly IDictionary<string, SiteErrors> siteErrors;

        /// <summary>
        /// Initializes the <see cref="CustomError" /> class.
        /// </summary>
        private CustomError()
        {
            siteErrors = new Dictionary<string, SiteErrors>();
            //Read the configuration nodes
            foreach (XmlNode node in Factory.GetConfigNodes("sitesErrors/site"))
            {
                var siteName = XmlUtil.GetAttribute("name", node);
                var siteErrorsNode = XmlUtil.GetChildElement("errors", node);
                var defaultUrl = XmlUtil.GetAttribute("defaultUrl", siteErrorsNode);

                var customError = new SiteErrors { DefaultUrl = defaultUrl };
                foreach (XmlNode cItem in siteErrorsNode.ChildNodes)
                {
                    var statusCode = XmlUtil.GetAttribute("statusCode", cItem);
                    var url = XmlUtil.GetAttribute("url", cItem);
                    customError.Errors.Add(new Error { StatusCode = statusCode, Url = url });
                }

                siteErrors.Add(siteName, customError);
            }
        }

        /// <summary>
        /// Gets the site errors.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <returns>Return site errors.</returns>
        public SiteErrors GetSiteErrors(string siteName)
        {
            if (siteErrors.ContainsKey(siteName))
                return siteErrors[siteName];

            return SiteErrors.ConvertFrom(ConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection);
        }
    }
}
