using System;
using System.Web;

namespace Framework.Sc.Extensions.ErrorHandler
{
    /// <summary>
    /// IModule implementation to handle application level errors.
    /// </summary>
    public class ErrorHandlerModule : IHttpModule
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
            // Custom error flag check from context 
            var context = new HttpContextWrapper(HttpContext.Current);
            if (!context.IsCustomErrorEnabled)
                return;

            var application = (HttpApplication)sender;
            var error = application.Server.GetLastError();
            var exceptionHandler = ExceptionHandlerFactory.Create();
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
