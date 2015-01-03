using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc.Filters
{
    /// <summary>
    /// Model state temp data transfer base class.
    /// </summary>
    public abstract class ModelStateTransfer : ActionFilterAttribute
    {
        /// <summary>
        /// The key
        /// </summary>
        protected static readonly string Key = typeof(ModelStateTransfer).FullName;
    }

    public abstract class ResultTransfer : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ResultTransfer).FullName;
        protected static readonly string OriginalRequestTypeKey = Key + "_OriginalRequestType";

        protected virtual string CreateResultKey(ControllerContext ctrlContext, string actionMethodName)
        {
            return Key + "_" + ctrlContext.Controller.GetType().Name + "_" + actionMethodName;
        }
    }
}
