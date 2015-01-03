using Common.Models;
using Framework.Sc.Extensions.Mvc;
using Framework.Sc.Extensions.Mvc.Filters;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class ContactUsController : Controller
    {
        //[HttpGet]
        //[ImportModelState]
        //public ActionResult Index(string success)
        //{
        //    var model = new ContactUsModel();
        //    if (!string.IsNullOrWhiteSpace(success) && success == "true")
        //    {
        //        model.Result = "You details has been recorded, we will contact you very soon.";
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //[ExportModelState]
        //public ActionResult Index(ContactUsModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // -To Do- Save the model data into data store
        //        return Redirect(ControllerContext.HttpContext.Request.RawUrl + "?success=true");
        //    }

        //    return Redirect(ControllerContext.HttpContext.Request.RawUrl);
        //}

        [HttpGet]
        [ImportResult]
        public ActionResult Index()
        {
            return View(new ContactUsModel());
        }

        [HttpPost]
        [ExportResult(ImportMethodName="Index")]
        public ActionResult ContactUsSave(ContactUsModel model)
        {
            if (ModelState.IsValid)
            {
                model.Result = "You details has been recorded, we will contact you very soon.";
            }

            return View("Index", model);
        }
    }
}