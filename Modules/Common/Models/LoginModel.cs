using Framework.Sc.Extensions.BaseModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class LoginModel :FormModel
    {
        [Required(ErrorMessage="Please supply a valid user name.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please supply a valid password.")]
        public string Password { get; set; }

    }
}