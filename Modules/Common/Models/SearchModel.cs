using Framework.Sc.Extensions.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    /// <summary>
    /// SearchModel
    /// </summary>
    public class SearchModel : FormModel
    {
        /// <summary>
        /// Gets or sets the search criteria.
        /// </summary>
        /// <value>
        /// The search criteria.
        /// </value>
        [Required(ErrorMessage="Parameter required.")]
        public string SearchCriteria { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public List<string> Result { get; set; }
    }
}