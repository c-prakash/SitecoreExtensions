using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Web;
using Framework.Core.Infrastructure.Logging;

namespace Framework.Bootstrap
{
    /// <summary>
    /// Log extension class to collect more exception info.
    /// </summary>
    public static class LogExtension
    {
        /// <summary>
        /// Gets the exception information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Return log object with additional information.</returns>
        /// <exception cref="System.ArgumentNullException">Log;Log object not initialized.</exception>
        public static Log GetExceptionInfo(this Log log, HttpContextBase httpContext = null)
        {
            if (log == null)
                throw new ArgumentNullException("log", "Log object not initialized.");

            if (log.Exception == null)
                return log;

            var exception = log.Exception;
            var properties = new Dictionary<string, object>();


            log.HostName = TryGetValue(() => Environment.MachineName, string.Empty);
            log.ExceptionType = TryGetValue(() => exception.GetBaseException().GetType().FullName, string.Empty);
            log.Message = TryGetValue(() => exception.GetBaseException().Message, string.Empty);
            log.User = Thread.CurrentPrincipal.Identity.Name ?? string.Empty;
            log.Timestamp = DateTime.Now;

            var eProperties = new ExceptionProperties(httpContext);
            log.User = eProperties.CurrentUserName;
            properties.Add("IPAddress", eProperties.CurrentUserIpAddress);
            properties.Add("User Agent", eProperties.CurrentUserAgent);
            properties.Add("Current Url", eProperties.CurrentUrl);
            properties.Add("Referrer Url", eProperties.CurrentUrlReferrer);
            properties.Add("Server Variable", eProperties.ServerVariable);
            properties.Add("Query String", eProperties.QueryString);
            properties.Add("Form Values", eProperties.Form);
            //properties.Add("Cookies", eProperties.Cookies);
            log.ExtendedParameters = properties;

            return log;
        }

        /// <summary>
        /// Tries to get value else return default value.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="evaluator">The evaluator.</param>
        /// <param name="failureValue">The failure value.</param>
        /// <returns>Return value after evaluation or default value.</returns>
        public static TResult TryGetValue<TResult>(Func<TResult> evaluator, TResult failureValue)
        {
            try
            {
                return evaluator();
            }
            catch
            {
                return failureValue;
            }
        }

        /// <summary>
        /// Exception properties class to discover additional info.
        /// </summary>
        private class ExceptionProperties
        {
            /// <summary>
            /// The unavailable
            /// </summary>
            private const string Unavailable = "n/a";
            /// <summary>
            /// The context
            /// </summary>
            private readonly HttpContextBase context;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExceptionProperties"/> class.
            /// </summary>
            /// <param name="context">The context.</param>
            public ExceptionProperties(HttpContextBase context = null)
            {
                this.context = context ?? ((HttpContext.Current != null) ? new HttpContextWrapper(HttpContext.Current) : null);
            }

            /// <summary>
            /// Gets the name of the current user.
            /// </summary>
            /// <value>
            /// The name of the current user.
            /// </value>
            public string CurrentUserName
            {
                get
                {
                    if (IsHosted)
                    {
                        if ((context.User != null) && (context.User.Identity != null))
                        {
                            return context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "Unauthenticated User";
                        }
                    }

                    return Unavailable;
                }
            }

            /// <summary>
            /// Gets the current user ip address.
            /// </summary>
            /// <value>
            /// The current user ip address.
            /// </value>
            public string CurrentUserIpAddress
            {
                get
                {
                    if (IsRequestAvailable)
                    {
                        string ip = context.Request.UserHostAddress;

                        if (!string.IsNullOrEmpty(context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                        {
                            ip += "->" + context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        }

                        return ip;
                    }

                    return Unavailable;
                }
            }

            /// <summary>
            /// Gets the current user agent.
            /// </summary>
            /// <value>
            /// The current user agent.
            /// </value>
            public string CurrentUserAgent
            {
                get { return IsRequestAvailable ? context.Request.UserAgent : Unavailable; }
            }

            /// <summary>
            /// Gets the current URL.
            /// </summary>
            /// <value>
            /// The current URL.
            /// </value>
            public string CurrentUrl
            {
                get { return IsRequestAvailable ? context.Request.RawUrl : Unavailable; }
            }

            /// <summary>
            /// Gets the current URL referrer.
            /// </summary>
            /// <value>
            /// The current URL referrer.
            /// </value>
            public string CurrentUrlReferrer
            {
                get
                {
                    if (IsRequestAvailable)
                    {
                        if (context.Request.UrlReferrer != null)
                            return context.Request.UrlReferrer.ToString();
                    }

                    return Unavailable;
                }
            }

            /// <summary>
            /// Gets the server variable.
            /// </summary>
            /// <value>
            /// The server variable.
            /// </value>
            public string ServerVariable
            {
                get
                {
                    if (IsRequestAvailable) return Join(context.Request.ServerVariables, "|");
                    return Unavailable;
                }
            }

            /// <summary>
            /// Gets the query string.
            /// </summary>
            /// <value>
            /// The query string.
            /// </value>
            public string QueryString
            {
                get
                {
                    if (IsRequestAvailable) return Join(context.Request.QueryString, "|");
                    return Unavailable;
                }
            }

            /// <summary>
            /// Gets the form.
            /// </summary>
            /// <value>
            /// The form.
            /// </value>
            public string Form
            {
                get
                {
                    if (IsRequestAvailable) return Join(context.Request.Form, "|");
                    return Unavailable;
                }
            }

            /// <summary>
            /// Gets the cookies.
            /// </summary>
            /// <value>
            /// The cookies.
            /// </value>
            public string Cookies
            {
                get
                {
                    if (IsRequestAvailable)
                    {
                        string result = context.Request.Cookies.Cast<HttpCookie>().
                                        Aggregate("|", (current, cookie) =>
                                        current +
                                        string.Format("Name={0}; Value={1}; Expires={2}; Domain={3}; Path={4}; Secure={5}",
                                        cookie.Name, cookie.Value, cookie.Expires.ToString("MM/dd/yyyy HH:mm:ss.fffff"), cookie.Domain, cookie.Path, cookie.Secure));
                    }

                    return Unavailable;
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is request available.
            /// </summary>
            /// <value>
            /// <c>true</c> if this instance is request available; otherwise, <c>false</c>.
            /// </value>
            private bool IsRequestAvailable
            {
                get
                {
                    Func<bool> isAvailable = () =>
                    {
                        try
                        {
                            return context.Request != null;
                        }
                        catch (HttpException)
                        {
                        }

                        return false;
                    };

                    return IsHosted && isAvailable();
                }
            }

            /// <summary>
            /// Gets a value indicating whether this instance is hosted.
            /// </summary>
            /// <value>
            ///   <c>true</c> if this instance is hosted; otherwise, <c>false</c>.
            /// </value>
            private bool IsHosted { get { return context != null; } }

            /// <summary>
            /// Joins the specified collection.
            /// </summary>
            /// <param name="collection">The collection.</param>
            /// <param name="separator">The separator.</param>
            /// <returns>Join the values available in <see cref="NameValueCollection"/> and return as <see cref="String"/>.</returns>
            private string Join(NameValueCollection collection, string separator)
            {
                var dict = ToDictionary(collection);
                return string.Join(separator, dict.Select(x => x.Key + "=" + x.Value));
            }

            /// <summary>
            /// To the dictionary.
            /// </summary>
            /// <param name="collection">The collection.</param>
            /// <returns>Convert <see cref="NameValueCollection"/> to dictionary.</returns>
            private IEnumerable<KeyValuePair<string, string>> ToDictionary(NameValueCollection collection)
            {
                return collection.AllKeys.ToDictionary(k => k, v => collection[v]);
            }
        }
    }
}
