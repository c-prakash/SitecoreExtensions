using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Common.Controllers
{
    public class CarouselController : Controller
    {
        // GET: Common/Carousel
        public ActionResult Index()
        {
            return View();
        }
    }
}