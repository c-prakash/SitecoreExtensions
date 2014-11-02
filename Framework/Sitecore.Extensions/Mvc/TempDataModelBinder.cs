using System;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc
{
    /// <summary>
    /// Temp data model binder.
    /// </summary>
    public class TempDataModelBinder : CustomModelBinderAttribute, IModelBinder
    {
        /// <summary>
        /// Retrieves the associated model binder.
        /// </summary>
        /// <returns>
        /// A reference to an object that implements the <see cref="T:System.Web.Mvc.IModelBinder" /> interface.
        /// </returns>
        public override IModelBinder GetBinder()
        {
            return this;
        }

        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>
        /// The bound value.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult val = bindingContext.ValueProvider.GetValue(bindingContext.ModelType.Name);
            if (val == null)
                return null;

            object result = val.RawValue;
            result = result ?? Activator.CreateInstance(bindingContext.ModelType);
            return result;
        }
    }
}
