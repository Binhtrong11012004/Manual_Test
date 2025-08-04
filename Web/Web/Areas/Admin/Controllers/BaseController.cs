using Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Attributes;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected static WebDatPhongEntities Db;

        public BaseController()
        {
            Db = new WebDatPhongEntities();
        }
    }
}