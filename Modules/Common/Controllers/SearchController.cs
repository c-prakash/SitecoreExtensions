using Common.Models;
using Framework.Core.Infrastructure.Logging;
using Framework.Sc.Extensions.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class SearchController : Controller
    {
        [ImportModelState]
        [HttpGet]
        public ActionResult Index(string term)
        {
            var model = new SearchModel();
            if (!string.IsNullOrWhiteSpace(term))
            {
                model.SearchCriteria = term;
                model.Result = new List<string> { "Hello!", "Hi!!!" };
            }

            return View(model);
        }

        [HttpPost]
        [ExportModelState(ImportMethodName="Index")]
        public ActionResult Search(SearchModel search)
        {
            if (ModelState.IsValid)
            {
                return Redirect(ControllerContext.HttpContext.Request.RawUrl + "?term=" +search.SearchCriteria);
            }

            return Redirect(ControllerContext.HttpContext.Request.RawUrl);
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