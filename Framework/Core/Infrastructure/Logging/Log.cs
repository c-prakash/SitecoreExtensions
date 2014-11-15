using Framework.Core.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Framework.Core.Infrastructure.Logging
{
    /// <summary>
    /// Log class to record Info/Warn/Error messages.
    /// </summary>
    public class Log
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public Log(Exception exception)
        {
            this.Exception = exception;
            this.Severity = TraceEventType.Error;
        }

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>
        /// The type of the exception.
        /// </value>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        /// <value>
        /// The severity.
        /// </value>
        public TraceEventType Severity { get; set; }

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the extended parameters.
        /// </summary>
        /// <value>
        /// The extended parameters.
        /// </value>
        public IDictionary<string, object> ExtendedParameters { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Timestamp -- {0}".FormatWith(DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss.fffff")));

            stringBuilder.AppendLine("Log Level -- {0}".FormatWith(this.Severity.ToString()));

            if (!string.IsNullOrWhiteSpace(this.Message))
                stringBuilder.AppendLine(this.Message);

            if (this.Exception != null)
            {
                stringBuilder.AppendLine(this.Exception.ToString());
                if (this.ExtendedParameters != null)
                {
                    foreach (var prop in this.ExtendedParameters)
                    {
                        stringBuilder.AppendLine("{0}--{1}".FormatWith(prop.Key, prop.Value));
                    }
                }
            }
            stringBuilder.AppendLine("----------------------------------------------------------------------------------------------------------------");

            return stringBuilder.ToString();
        }


    }
}
