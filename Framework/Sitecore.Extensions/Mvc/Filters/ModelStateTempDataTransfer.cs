using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc.Filters
{
    /// <summary>
    /// Model state temp data transfer base class.
    /// </summary>
    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
    {
        /// <summary>
        /// The key
        /// </summary>
        protected static readonly string Key = typeof(ModelStateTempDataTransfer).FullName;
    }
}
