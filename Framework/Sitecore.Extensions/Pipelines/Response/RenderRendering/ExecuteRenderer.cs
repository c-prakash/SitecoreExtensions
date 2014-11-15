using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;

namespace Framework.Sc.Extensions.Pipelines.Response.RenderRendering
{
    public class ExecuteRenderer : Sitecore.Mvc.Pipelines.Response.RenderRendering.ExecuteRenderer
    {
        public bool ShowExceptionsToAdministrators { get; set; }
        public bool ShowExceptionsInPageEditor { get; set; }
        public bool ShowExceptionsInPreview { get; set; }
        public bool ShowExceptionsInDebugger { get; set; }

        public override void Process(
          Sitecore.Mvc.Pipelines.Response.RenderRendering.RenderRenderingArgs args)
        {
            TextWriter restoreWriter = args.Writer;

            try
            {
                StringBuilder sb = new StringBuilder();
                using (StringWriter sw = new StringWriter(sb))
                {
                    // nested try attempts to workaround defect in HtmlTextWriter
                    // http://stackoverflow.com/questions/6595742/htmltextwriter-doesnt-flush-upon-disposal
                    HtmlTextWriter hw = new HtmlTextWriter(sw);

                    try
                    {
                        args.Writer = hw;
                        base.Process(args);
                    }
                    finally
                    {
                        hw.Close();
                        hw.Dispose();
                    }
                }

                restoreWriter.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Framework.Sc.Extensions.ErrorHandler.ExceptionHandlerFactory.Create().HandleException(ex, new HttpContextWrapper(HttpContext.Current));
                args.AbortPipeline();
               
                return;
                args.Cacheable = false;
                Sitecore.Diagnostics.Log.Error(
                  "Rendering exception processing " + args.Rendering + " for " + Sitecore.Context.RawUrl,
                  ex,
                  this);

                if (this.ShouldRenderErrors())
                {
                    Sitecore.Web.UI.WebControls.ErrorControl errorControl = Sitecore.Configuration.Factory.CreateErrorControl(
                      HttpUtility.HtmlEncode("Rendering exception processing " + args.Rendering + " : " + ex.Message),
                      ex.ToString());
                    //restoreWriter.Write(errorControl.RenderAsText());

                    TextWriter tx= new StringWriter();
                    tx.Write(errorControl.RenderAsText());
                    restoreWriter = tx;
                }
                else
                {
                    // if you don't ensure proper exception handling at a higher level
                    // you may prefer to redirect here.
                    // SC.Web.WebUtil.RedirectToErrorPage(
                    //   SC.Globalization.Translate.Text("An error occurred."));
                    throw;
                }
            }
            finally
            {
                args.Writer = restoreWriter;
            }
        }

        protected bool ShouldRenderErrors()
        {
            return HttpContext.Current.IsCustomErrorEnabled
              || (this.ShowExceptionsToAdministrators && Sitecore.Context.User.IsAdministrator)
              || (this.ShowExceptionsInPageEditor && Sitecore.Context.PageMode.IsPageEditor)
              || (this.ShowExceptionsInPreview && Sitecore.Context.PageMode.IsPreview)
              || (this.ShowExceptionsInDebugger && Sitecore.Context.PageMode.IsDebugging);
        }
    }

    public class CustomPerformRendering : PerformRendering
    {
        public override void Process(RenderPlaceholderArgs args)
        {
            base.Process(args);
        }
    }
}

