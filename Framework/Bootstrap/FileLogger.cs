using System.IO;
using Framework.Core.Infrastructure.Logging;

namespace Framework.Bootstrap
{
    /// <summary>
    /// Logger implementation for File logging.
    /// </summary>
    public class FileLogger : ILogger
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogger"/> class.
        /// </summary>
        public FileLogger()
        {
            writer = new StreamWriter("c:\\temp\\logtester.log", true) {AutoFlush = true};
            EnableAsync = true;
        }

        #endregion

        /// <summary>
        /// The writer
        /// </summary>
        private StreamWriter writer;
        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Gets or sets a value indicating whether [enable asynchronous] logging or not..
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable asynchronous]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableAsync { get; set; }

        /// <summary>
        /// Writes the specified log object.
        /// </summary>
        /// <param name="logObject">The log object.</param>
        public void Write(Log logObject)
        {
            //return writer.WriteLineAsync(logObject.ToString());
            writer.WriteLine(logObject.ToString());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }

                writer = null;
            }

            // Free any unmanaged objects here. 
            disposed = true;
        }
    }
}
