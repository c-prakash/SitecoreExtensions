using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Framework.Sc.Extensions.ErrorHandler
{
    public static class ExceptionExtension
    {
        public static IDictionary<string, object> GetExceptionInfo(this Exception exception)
        {
            var properties = new Dictionary<string, object>();
            var eProperties = new ExceptionProperties(exception);

            //properties.Add("Namespace Name", eProperties.NamespaceName);
            //properties.Add("Class Name", eProperties.ClassName);
            //properties.Add("Method Name", eProperties.MethodSignature);
            properties.Add("Current UserName", eProperties.CurrentUserName);
            properties.Add("IPAddress", eProperties.CurrentUserIPAddress);
            properties.Add("User Agent", eProperties.CurrentUserAgent);
            properties.Add("Current Url", eProperties.CurrentUrl);
            properties.Add("Referrer Url", eProperties.CurrentUrlReferrer);

            return properties;
        }

        private class ExceptionProperties
        {
            internal const string Unavailable = "n/a";
            private readonly HttpContextBase _context;

            public ExceptionProperties(Exception exception, HttpContextBase context)
            {
                _context = context ?? ((HttpContext.Current != null) ? new HttpContextWrapper(HttpContext.Current) : null);
                GetMethodDetails(exception);
            }

            public ExceptionProperties(Exception exception)
                : this(exception, null)
            {
            }

            public string NamespaceName { get; set; }

            public string ClassName { get; set; }

            public string MethodSignature { get; set; }

            public string CurrentUserName
            {
                get
                {
                    if (IsHosted)
                    {
                        if ((_context.User != null) && (_context.User.Identity != null))
                        {
                            return _context.User.Identity.IsAuthenticated ? _context.User.Identity.Name : "Unauthenticated User";
                        }
                    }

                    return Unavailable;
                }
            }

            public string CurrentUserIPAddress
            {
                get
                {
                    if (IsRequestAvailable)
                    {
                        string ip = _context.Request.UserHostAddress;

                        if (!string.IsNullOrEmpty(_context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                        {
                            ip += "->" + _context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        }

                        return ip;
                    }

                    return Unavailable;
                }
            }

            public string CurrentUserAgent
            {
                get
                {
                    return IsRequestAvailable ? _context.Request.UserAgent : Unavailable;
                }
            }

            public string CurrentUrl
            {
                get
                {
                    return IsRequestAvailable ? _context.Request.RawUrl : Unavailable;
                }
            }

            public string CurrentUrlReferrer
            {
                get
                {
                    if (IsRequestAvailable)
                    {
                        if (_context.Request.UrlReferrer != null)
                        {
                            return _context.Request.UrlReferrer.ToString();
                        }
                    }

                    return Unavailable;
                }
            }

            internal bool IsRequestAvailable
            {
                get
                {
                    Func<bool> isAvailable = () =>
                    {
                        try
                        {
                            return _context.Request != null;
                        }
                        catch (HttpException)
                        {
                        }

                        return false;
                    };

                    return IsHosted && isAvailable();
                }
            }

            private bool IsHosted
            {
                get
                {
                    return _context != null;
                }
            }

            internal void GetMethodDetails(Exception exception)
            {
                var st = new StackTrace(exception, true); // create the stack trace
                var frames = st.GetFrames()       // get the frames
                  .Select(frame => new
                   {                   // get the info
                       FileName = frame.GetFileName(),
                       LineNumber = frame.GetFileLineNumber(),
                       ColumnNumber = frame.GetFileColumnNumber(),
                       Method = frame.GetMethod(),
                       Class = frame.GetMethod().DeclaringType,
                   });

                StringBuilder output = new StringBuilder();

                foreach (var frame in frames)
                {
                    NamespaceName = frame.Method.DeclaringType.Namespace;
                    ClassName = frame.Method.DeclaringType.Name;

                    output.Append(frame.Method.Name);
                    output.Append("(");

                    ParameterInfo[] paramInfos = frame.Method.GetParameters();

                    if (paramInfos.Length > 0)
                    {
                        output.Append(string.Format("{0} {1}", paramInfos[0].ParameterType.Name, paramInfos[0].Name));

                        if (paramInfos.Length > 1)
                        {
                            for (int j = 1; j < paramInfos.Length; j++)
                            {
                                output.Append(string.Format(", {0} {1}", paramInfos[j].ParameterType.Name, paramInfos[j].Name));
                            }
                        }
                    }

                    output.Append(")");
                    output.AppendLine();
                }

                MethodSignature = output.ToString();
            }
        }
    }
}
