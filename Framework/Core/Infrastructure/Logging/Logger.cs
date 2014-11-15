using Framework.Core.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Framework.Core.Infrastructure.Logging
{
    public sealed class Logger : IDisposable
    {
        //create singleton instance of logger queue
        public static Logger Current = new Logger();
        private ILogger currentLogger;

        private readonly Task loggerTask;
        private readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();
        private readonly BufferBlock<Log> bufferBlock = new BufferBlock<Log>();
        private bool disposed;

        #region Constructor

        private Logger()
            : this(IoC.IoC.Resolve<ILogger>())
        {
        }

        private Logger(ILogger logger)
        {
            this.currentLogger = logger;
            //configure background worker
            loggerTask = Task.Factory.StartNew(() =>
            {
                var cToken = cancellationToken.Token;
                for (; ; )
                {
                    this.currentLogger.Write(bufferBlock.Receive());
                    if (cToken.IsCancellationRequested && bufferBlock.Count == 0)
                        return;
                }
            });
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        private void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                cancellationToken.Cancel();
                cancellationToken.Dispose();
                Task.WaitAll(loggerTask);
            }

            // Free any unmanaged objects here. 
            disposed = true;
        }

        #endregion

        public ILogger CurrentLogger
        {
            get { return this.currentLogger; }

            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("logger", "The current logger was not located correctly.");
                }

                this.currentLogger = value;
            }
        }

        private async static void WriteLog(Log log)
        {
            if (!Current.CurrentLogger.EnableAsync)
            {
                Current.CurrentLogger.Write(log);
                return;
            }

            await Current.bufferBlock.SendAsync(log);
        }

        public static void Write(Log log)
        {
            WriteLog(log);
        }

        public static void Write(string message, System.Diagnostics.TraceEventType severity, IDictionary<string, object> extendedParameters = null)
        {
            Write(new Log { Message = message, Severity = severity, ExtendedParameters = extendedParameters });
        }

        public static void Info(string format, params object[] args)
        {
            Info(format.FormatWith(args));
        }

        public static void Info(string message, IDictionary<string, object> extendedParameters = null)
        {
            Write(message, System.Diagnostics.TraceEventType.Information, extendedParameters);
        }

        public static void Warning(string format, params object[] args)
        {
            Warning(format.FormatWith(args));
        }

        public static void Warning(string message, IDictionary<string, object> extendedParameters = null)
        {
            Write(message, System.Diagnostics.TraceEventType.Warning, extendedParameters);
        }

        public static void Error(string format, params object[] args)
        {
            Error(format.FormatWith(args));
        }

        public static void Error(string message, IDictionary<string, object> extendedParameters = null)
        {
            Write(message, System.Diagnostics.TraceEventType.Error, extendedParameters);
        }

        public static void Exception(Exception exception, IDictionary<string, object> extendedParameters = null)
        {
            Write(new Log { Exception = exception, Severity = System.Diagnostics.TraceEventType.Error, ExtendedParameters = extendedParameters });
        }
    }
}
