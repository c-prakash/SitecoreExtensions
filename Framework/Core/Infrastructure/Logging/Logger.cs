using Framework.Core.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                cancellationToken.Cancel();
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

        private async static void LogMessage(Log log)
        {
            if (!Current.CurrentLogger.EnableAsync)
            {
                Current.CurrentLogger.Write(log);
                return;
            }

            await Current.bufferBlock.SendAsync(log);
        }

        private static void Write(string message, Exception exception, System.Diagnostics.TraceEventType severity, IDictionary<string, object> extendedParameters = null)
        {
            severity = exception != null ? System.Diagnostics.TraceEventType.Error : severity;
            LogMessage(new Log { Message = message, Exception = exception, Severity = severity, ExtendedParameters = extendedParameters });
        }

        public static void Info(string format, params object[] args)
        {
            Info(format.FormatWith(args));
        }

        public static void Info(string message, IDictionary<string, object> extendedParameters = null)
        {
            Write(message, null, System.Diagnostics.TraceEventType.Information, extendedParameters);
        }

        public static void Warning(string format, params object[] args)
        {
            Warning(format.FormatWith(args));
        }

        public static void Warning(string message, IDictionary<string, object> extendedParameters = null)
        {
            Write(message, null, System.Diagnostics.TraceEventType.Warning, extendedParameters);
        }

        public static void Error(string format, params object[] args)
        {
            Error(format.FormatWith(args));
        }

        public static void Error(string message, IDictionary<string, object> extendedParameters = null)
        {
            Write(message, null, System.Diagnostics.TraceEventType.Error, extendedParameters);
        }

        public static void Exception(Exception exception, IDictionary<string, object> extendedParameters = null)
        {
            Write(null, exception, System.Diagnostics.TraceEventType.Error, extendedParameters);
        }
    }

    //public sealed class Logger1 : IDisposable
    //{
    //    //create singleton instance of logger queue
    //    public static Logger1 Current = new Logger1();
    //    private ILogger currentLogger;
    //    private static readonly object logQueueLock = new object();

    //    private Queue<Log> logQueue = new Queue<Log>();
    //    private BackgroundWorker loggerThread = new BackgroundWorker();
    //    private static bool disposed;

    //    private Logger1()
    //        : this(IoC.IoC.Resolve<ILogger>())
    //    {
    //    }

    //    private Logger1(ILogger logger)
    //    {
    //        this.currentLogger = logger;
    //        //configure background worker
    //        loggerThread.WorkerSupportsCancellation = false;
    //        loggerThread.DoWork += new DoWorkEventHandler(LoggerDoWork);

    //    }

    //    #region IDisposable Implementation

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);           
    //    }

    //    // Protected implementation of Dispose pattern. 
    //    protected void Dispose(bool disposing)
    //    {
    //        if (disposed)
    //            return;

    //        if (disposing)
    //        {
    //            // Free any other managed objects here. 
    //            if (loggerThread != null)
    //                loggerThread.Dispose();
    //        }

    //        // Free any unmanaged objects here. 
    //        disposed = true;
    //    }

    //    #endregion

    //    public ILogger CurrentLogger
    //    {
    //        get { return this.currentLogger; }

    //        private set
    //        {
    //            if (value == null)
    //            {
    //                throw new ArgumentNullException("logger", "The current logger was not located correctly.");
    //            }

    //            this.currentLogger = value;
    //        }
    //    }

    //    private static void LogMessage(Log log)
    //    {
    //        if (!Current.CurrentLogger.EnableAsync)
    //        {
    //            Current.CurrentLogger.Write(log);
    //            return;
    //        }

    //        //lock during write
    //        lock (logQueueLock)
    //        {
    //            Current.logQueue.Enqueue(log);

    //            //while locked check to see if the BW is running, if not start it
    //            if (!Current.loggerThread.IsBusy)
    //                Current.loggerThread.RunWorkerAsync();
    //        }
    //    }

    //    private void LoggerDoWork(object sender, DoWorkEventArgs e)
    //    {
    //        while (true)
    //        {
    //            Log log = null;

    //            bool skipEmptyCheck = false;
    //            lock (logQueueLock)
    //            {
    //                if (logQueue.Count <= 0) //if queue is empty than BW is done
    //                    return;
    //                else if (logQueue.Count > 1) //if greater than 1 we can skip checking to see if anything has been enqueued during the logging operation
    //                    skipEmptyCheck = true;

    //                //dequeue the LogEntry that will be written to the log
    //                log = logQueue.Dequeue();
    //            }

    //            //pass LogEntry to Enterprise Library
    //            currentLogger.Write(log);

    //            if (skipEmptyCheck) //if LogEntryQueue.Count was > 1 before we wrote the last LogEntry we know to continue without double checking
    //            {
    //                lock (logQueueLock)
    //                {
    //                    if (logQueue.Count <= 0) //if queue is still empty than BW is done
    //                        return;
    //                }
    //            }
    //        }
    //    }

    //    private static void Write(string message, Exception exception, System.Diagnostics.TraceEventType severity, IDictionary<string, object> extendedParameters = null)
    //    {
    //        severity = exception != null ? System.Diagnostics.TraceEventType.Error : severity;
    //        LogMessage(new Log { Message = message, Exception = exception, Severity = severity, ExtendedParameters = extendedParameters });
    //    }

    //    public static void Info(string format, params object[] args)
    //    {
    //        Info(format.FormatWith(args));
    //    }

    //    public static void Info(string message, IDictionary<string, object> extendedParameters = null)
    //    {
    //        Write(message, null, System.Diagnostics.TraceEventType.Information, extendedParameters);
    //    }

    //    public static void Warning(string format, params object[] args)
    //    {
    //        Warning(format.FormatWith(args));
    //    }

    //    public static void Warning(string message, IDictionary<string, object> extendedParameters = null)
    //    {
    //        Write(message, null, System.Diagnostics.TraceEventType.Warning, extendedParameters);
    //    }

    //    public static void Error(string format, params object[] args)
    //    {
    //        Error(format.FormatWith(args));
    //    }

    //    public static void Error(string message, IDictionary<string, object> extendedParameters = null)
    //    {
    //        Write(message, null, System.Diagnostics.TraceEventType.Error, extendedParameters);
    //    }

    //    public static void Exception(Exception exception, IDictionary<string, object> extendedParameters = null)
    //    {
    //        Write(null, exception, System.Diagnostics.TraceEventType.Error, extendedParameters);
    //    }
    //}
}
