using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Staff(string search = "")
        {
            ViewBag.Search = search;

            var data = db.TaiKhoans.Where(x => x.HoTen.Contains(search)).ToList();

            return View(data);
        }
    }
}