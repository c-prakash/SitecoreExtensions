using Common.Models;
using Framework.Sc.Extensions.Mvc;
using Framework.Sc.Extensions.Mvc.Filters;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class ContactUsController: Controller
    {
        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Index([TempDataModelBinder]ContactUsModel model)
        {
            if (!model.IsPost)
            {
                // Todo- if something is required during get on form post
            }

            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult ContactUsSave(ContactUsModel model)
        {
            if (ModelState.IsValid)
            {
                model.Result = "You details has been recorded, we will contact you very soon.";
            }

            return this.Redirect(model);
        }
    }
}