using Framework.Core.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Infrastructure.Logging
{
    /// <summary>
    /// Log class to record Info/Warn/Error messages.
    /// </summary>
    public class Log
    {
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
        /// Gets or sets the severity.
        /// </summary>
        /// <value>
        /// The severity.
        /// </value>
        public TraceEventType Severity { get; set; }

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
                    foreach(var prop in this.ExtendedParameters)
                    {
                        stringBuilder.AppendLine("{0}--{1}".FormatWith(prop.Key, prop.Value));
                    }
                }
            }
            stringBuilder.AppendLine("----------------------------------------------------------------------------------------------------------------");

            return stringBuilder.ToString();
        }
    }

    /// <summary>
    /// Interface for log consumer.
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating whether [enable asynchronous].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable asynchronous]; otherwise, <c>false</c>.
        /// </value>
        bool EnableAsync { get; set; }
        /// <summary>
        /// Writes the specified log object.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        void Write(Log logObject);
    }
}
