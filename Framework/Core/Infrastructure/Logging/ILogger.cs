using System;

namespace Framework.Core.Infrastructure.Logging
{
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
