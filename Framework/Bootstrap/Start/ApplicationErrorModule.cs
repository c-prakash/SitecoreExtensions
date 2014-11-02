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
            application.Error += new EventHandler(OnApplicationError);
        }

        /// <summary>
        /// Called when [application error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void OnApplicationError(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            var error = application.Server.GetLastError();
            new ExceptionHandler().HandleException(error, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
