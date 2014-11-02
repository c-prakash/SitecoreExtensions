using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class HeaderController : Controller
    {
        // GET: Common/Header
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Navigation()
        {
            return View();
        }
    }
}