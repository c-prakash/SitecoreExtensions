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
        [ImportModelState]
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ExportModelState]
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

        //// GET: Account
        //[ImportModelStateFromTempData]
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ExportModelStateToTempData]
        //public ActionResult Login(LoginModel login)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        if(login.UserName=="test" && login.Password=="test")
        //        {
        //            AuthenticationManager.Login("test", false);
        //            return this.Redirect();
        //        }
        //    }

        //    return this.Redirect(login);
        //}
    }
}