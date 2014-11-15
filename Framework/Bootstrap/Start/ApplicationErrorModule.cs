using Framework.Core.Infrastructure.Logging;
using Framework.Sc.Extensions.ErrorHandler;
using System;
using System.Web;

namespace Framework.Bootstrap.Start
{
    /// <summary>
    /// IModule implementation to handle application level errors.
    /// </summary>
    public class ApplicationErrorModule : IHttpModule
    {
        /// <summary>
        /// Initializes the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        public void Init(HttpApplication application)
        {
            application.Error += new EventHandler(OnError);
        }

        /// <summary>
        /// Called when [application error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void OnError(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var error = application.Server.GetLastError();
            var exceptionHandler = ExceptionHandlerFactory.Create();

            var context = new HttpContextWrapper(HttpContext.Current);
            if (context.IsCustomErrorEnabled)
                Logger.Write(new Log(error).GetExceptionInfo(context));

            exceptionHandler.HandleException(error, context);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
