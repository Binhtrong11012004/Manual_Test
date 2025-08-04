using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [PemisitonAttribute("Hóa đơn")]
    public class BillController : BaseController
    {
        // GET: Admin/Bill
        public ActionResult Index()
        {
            var list = Db.PhongKhachHangs.OrderByDescending(x=>x.MaPhongKhachHang).ToList();
            return View(list);
        }

        public ActionResult Detail(int? id)
        {
            var model = Db.PhongKhachHangs.FirstOrDefault(x => x.MaPhongKhachHang == id);
            return View(model);
        }
    }
}