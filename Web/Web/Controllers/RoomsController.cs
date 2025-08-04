using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class RoomsController : BaseController
    {
        // GET: Rooms
        public ActionResult Index(DateTime? ngaybatdau, DateTime? ngayketthuc, int loaiphong = 0)
        {
            ViewBag.ngaybatdau = ngaybatdau?.ToString("yyyy-MM-dd");
            ViewBag.ngayketthuc = ngayketthuc?.ToString("yyyy-MM-dd");
            ViewBag.loaiphong = loaiphong;
            var phongs = db.Phongs.Where(x => x.TrangThai == "Chưa sử dụng" && (loaiphong == 0 || x.MaLoaiPhong == loaiphong) && x.ChiTietDonDatPhongs.Count(y => (y.DonDatPhong.NgayDen >= ngaybatdau && ngaybatdau <= y.DonDatPhong.NgayDi) || (y.DonDatPhong.NgayDen >= ngayketthuc && ngayketthuc <= y.DonDatPhong.NgayDi)) == 0).ToList();
            return View(phongs);
        }

        public ActionResult Detail(int id)
        {
            var phong = db.Phongs.FirstOrDefault(x => x.MaPhong == id);
            if (phong == null)
                return Redirect("/rooms");
            return View(phong);
        }
    }
}