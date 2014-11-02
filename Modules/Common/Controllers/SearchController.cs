using Common.Models;
using Framework.Core.Infrastructure.Logging;
using Framework.Sc.Extensions.Mvc;
using Framework.Sc.Extensions.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Index([TempDataModelBinder]SearchModel model)
        {
            if (!model.IsPost)
            {
                model = new SearchModel() { RedirectUrl = "/home/secured" };
            }

            return View(model);
        }

        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult Search(SearchModel search)
        {
            if (ModelState.IsValid)
            {
                var criteria = search.SearchCriteria;
                if (!string.IsNullOrWhiteSpace(criteria))
                {
                    search.Result = new List<string> { "Hello", "Hi!!!" };
                }
            }

            return this.Redirect(search.RedirectUrl, search);
        }

        private void LoggerAsyncPerformance()
        {
            Stopwatch watch = new Stopwatch();
            Debug.WriteLine("Start time -{0}", DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss.fffff"));
            watch.Start();
            for (var i = 0; i < 100000; i++)
                Logger.Info(string.Format("I am writing from Action method - {0}", i.ToString()));

            watch.Stop();
            Debug.WriteLine("End time -{0}", DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss.fffff"));
            Debug.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}