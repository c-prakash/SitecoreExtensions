using Common.Models;
using Framework.Sc.Extensions.Mvc;
using Framework.Sc.Extensions.Mvc.Filters;
using Sitecore.Security.Authentication;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [ImportResult]
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ExportResult]
        public ActionResult Index(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                if (login.UserName == "test" && login.Password == "test")
                {
                    AuthenticationManager.Login("test", false);
                    return Redirect(ControllerContext.HttpContext.Request.RawUrl);
                }
            }

            return View(login);
        }
    }
}