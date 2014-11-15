using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Helpers;
using Sitecore.Mvc.Presentation;
using System.Web;

namespace Framework.Sc.Extensions.Helpers
{
    /// <summary>
    /// Sitecore Html Helper extensions.
    /// </summary>
    public static class SitecoreHelperExtension 
    {
        /// <summary>
        /// Areas the form handler.
        /// </summary>
        /// <param name="sitecoreHelper">The sitecore helper.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Return html tag for area form handler.</returns>
        public static HtmlString AreaFormHandler(this SitecoreHelper sitecoreHelper, SafeDictionary<string> parameters = null)
        {
            return sitecoreHelper.AreaFormHandler(string.Empty, parameters);
        }

        /// <summary>
        /// Areas the form handler.
        /// </summary>
        /// <param name="sitecoreHelper">The sitecore helper.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Return html tag for area form handler.</returns>
        public static HtmlString AreaFormHandler(this SitecoreHelper sitecoreHelper, string controller, SafeDictionary<string> parameters = null)
        {
            return sitecoreHelper.AreaFormHandler(controller, string.Empty, string.Empty, string.Empty, parameters);
        }

        /// <summary>
        /// Areas the form handler.
        /// </summary>
        /// <param name="sitecoreHelper">The sitecore helper.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="action">The action.</param>
        /// <param name="areaName">Name of the area.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Return html tag for area form handler.</returns>
        public static HtmlString AreaFormHandler(this SitecoreHelper sitecoreHelper, string controller, string action, string areaName, string namespaceName, SafeDictionary<string> parameters = null)
        {
            if (controller.IsEmptyOrNull())
            {
                controller = sitecoreHelper.GetValueFromCurrentRendering("Form Controller Name");
            }
            if (action.IsEmptyOrNull())
            {
                action = sitecoreHelper.GetValueFromCurrentRendering("Form Controller Action");
            }
            if (areaName.IsEmptyOrNull())
            {
                areaName = sitecoreHelper.GetValueFromCurrentRendering("Form Controller Area");
            }
            if (namespaceName.IsEmptyOrNull())
            {
                namespaceName = sitecoreHelper.GetValueFromCurrentRendering("Form Controller Namespace");
            }
            if (controller.IsEmptyOrNull())
            {
                return new HtmlString(string.Empty);
            }
            string str = HiddenField("scController", controller);
            if (!action.IsEmptyOrNull())
            {
                str = string.Concat(str, HiddenField("scAction", action));
            }
            if (!areaName.IsEmptyOrNull())
            {
                str = string.Concat(str, HiddenField("scArea", areaName));
            }
            if (!namespaceName.IsEmptyOrNull())
            {
                str = string.Concat(str, HiddenField("scNamespace", namespaceName));
            }
            return new HtmlString(str);
        }

        /// <summary>
        /// Gets the value from current rendering.
        /// </summary>
        /// <param name="sitecorehelper">The sitecorehelper.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Return html tag for area form handler.</returns>
        public static string GetValueFromCurrentRendering(this SitecoreHelper sitecorehelper, string fieldName)
        {
            Rendering rendering = RenderingContext.CurrentOrNull.ValueOrDefault(context => context.Rendering);
            if (rendering == null)
            {
                return null;
            }
            string str = rendering[fieldName];
            if (!str.IsEmptyOrNull())
            {
                return str;
            }
            RenderingItem renderingItem = rendering.RenderingItem;
            if (renderingItem == null)
            {
                return null;
            }
            return renderingItem.InnerItem.Fields[fieldName].Value;
        }

        /// <summary>
        /// Hiddens the field.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>Return html tag for area form handler.</returns>
        private static string HiddenField(string name, string value)
        {
            return string.Format("<input type='hidden' name='{0}' value='{1}' >", name, value);
        }
    }
}
