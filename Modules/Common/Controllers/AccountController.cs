using Common.Models;
using Framework.Sc.Extensions.Helpers;
using Framework.Sc.Extensions.Mvc;
using Framework.Sc.Extensions.Mvc.Filters;
using Sitecore.Security.Authentication;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [ImportResult]
        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                var claimsPrincipal = (User as Sitecore.Security.Accounts.User).ToClaimsPrincipal();
            }

            return View(new LoginModel());
        }

        [HttpPost]
        [ExportResult]
        public  ActionResult Index(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                if (login.UserName == "test" && login.Password == "test")
                {
                    Task<string> authTask = new SimpleAuthenticationService().AuthenticateUserAsync(login.UserName, login.Password); 
                    System.Diagnostics.Debug.WriteLine("Authentication started"); 
                    string userId = authTask.Result;

                    AuthenticationManager.Login(userId, false);
                    return Redirect(ControllerContext.HttpContext.Request.RawUrl);
                }
            }

            return View(login);
        }
    }


    public sealed class SimpleAuthenticationService
    {
        public async Task<string> AuthenticateUserAsync(string user, string password)
        {
            //await Task.Delay(5000);

            return user;
        }
    }

}