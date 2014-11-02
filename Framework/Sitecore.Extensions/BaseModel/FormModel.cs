
namespace Framework.Sc.Extensions.BaseModel
{
    /// <summary>
    /// FormModel
    /// </summary>
    public class FormModel
    {
        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>
        /// The redirect URL.
        /// </value>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is post.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is post; otherwise, <c>false</c>.
        /// </value>
        public bool IsPost { get; set; }
    }
}
