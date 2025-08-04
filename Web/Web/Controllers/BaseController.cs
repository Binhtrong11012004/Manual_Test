using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected static WebDatPhongEntities db;

        public BaseController()
        {
            db = new WebDatPhongEntities();
        }
    }
}