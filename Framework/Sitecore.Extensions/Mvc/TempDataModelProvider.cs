using System;
using System.Globalization;
using System.Web.Mvc;

namespace Framework.Sc.Extensions.Mvc
{
    /// <summary>
    /// Temp data model provider class to retrieve model from tempdata.
    /// </summary>
    public sealed class TempDataModelProvider : IValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TempDataModelProvider"/> class.
        /// </summary>
        /// <param name="tempData">The temporary data.</param>
        public TempDataModelProvider(TempDataDictionary tempData)
        {
            this.tempData = tempData;
        }

        /// <summary>
        /// The temporary data
        /// </summary>
        private readonly TempDataDictionary tempData;
        /// <summary>
        /// Determines whether the collection contains the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix to search for.</param>
        /// <returns>
        /// true if the collection contains the specified prefix; otherwise, false.
        /// </returns>
        public bool ContainsPrefix(string prefix)
        {
            return tempData.ContainsKey(prefix);
        }

        /// <summary>
        /// Retrieves a value object using the specified key.
        /// </summary>
        /// <param name="key">The key of the value object to retrieve.</param>
        /// <returns>
        /// The value object for the specified key.
        /// </returns>
        public ValueProviderResult GetValue(string key)
        {
            return new ValueProviderResult(tempData[key], string.Empty, CultureInfo.CurrentCulture);
        }
    }

    /// <summary>
    /// Represents a factory for creating route-data value provider objects.
    /// </summary>
    public sealed class TempDataModelProviderFactory : ValueProviderFactory
    {
        /// <summary>
        /// Returns a value-provider object for the specified controller context.
        /// </summary>
        /// <param name="controllerContext">An object that encapsulates information about the current HTTP request.</param>
        /// <returns>
        /// A value-provider object.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">controllerContext</exception>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="controllerContext" /> parameter is null.</exception>
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            return new TempDataModelProvider(controllerContext.Controller.TempData);
        }
    }
}
