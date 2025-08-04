using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Attributes;
using Web.Models;

namespace Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [PemisitonAttribute("Đơn đặt phòng")]
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            var list = Db.DonDatPhongs.OrderByDescending(x => x.MaDonDatPhong).ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new DonDatPhong());
        }

        [HttpPost]
        public ActionResult Add(DonDatPhong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.NgayDatPhong = DateTime.Now;
                    model.MaTaiKhoan = 1;

                    Db.DonDatPhongs.Add(model);
                    Db.SaveChanges();
                    TempData["notice"] = "Thêm thành công!";

                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["notice"] = "Thêm không thành công!";
                }
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = Db.DonDatPhongs.FirstOrDefault(x => x.MaDonDatPhong == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DonDatPhong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.DonDatPhongs.FirstOrDefault(x => x.MaDonDatPhong == model.MaDonDatPhong);
                    obj.NgayDen = model.NgayDen;
                    obj.NgayDi = model.NgayDi;
                    obj.HoTen = model.HoTen;
                    obj.SoDienThoai = model.SoDienThoai;
                    obj.Email = model.Email;
                    obj.GhiChu = model.GhiChu;

                    Db.DonDatPhongs.Attach(obj);
                    Db.Entry(obj).State = EntityState.Modified;
                    Db.SaveChanges();
                    TempData["notice"] = "Sửa thành công!";

                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["notice"] = "Sửa không thành công!";
                }
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var model = Db.DonDatPhongs.FirstOrDefault(x => x.MaDonDatPhong == id);
                Db.DonDatPhongs.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.DonDatPhongs.Remove(model);
                Db.SaveChanges();
                TempData["notice"] = "Xóa thành công!";
            }
            catch
            {
                TempData["notice"] = "Xóa không thành công! Nguyên nhân: Có ràng buộc cơ sở dữ liệu!";
            }

            return RedirectToAction("Index");
        }
        public ActionResult View(int id)
        {
            var model = Db.DonDatPhongs.FirstOrDefault(x => x.MaDonDatPhong == id);
            return View(model);
        }

        public ActionResult AddDetail(int id)
        {
            var model = new ChiTietDonDatPhong
            {
                MaDonDatPhong = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDetail(ChiTietDonDatPhong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.ChiTietDonDatPhongs.FirstOrDefault(x => x.MaPhong == model.MaPhong && x.MaDonDatPhong == model.MaDonDatPhong && x.MaKhachSan == model.MaKhachSan);
                    if (obj == null)
                    {
                        Db.ChiTietDonDatPhongs.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return Redirect("/Admin/Order/View/" + model.MaDonDatPhong);
                    }
                    else
                    {
                        TempData["notice"] = "Loại phòng đã tồn tại! Vui lòng chọn loại phòng khác!";
                    }
                }
                catch
                {
                    TempData["notice"] = "Thêm không thành công!";
                }
            }

            return View(model);
        }

        public ActionResult EditDetail(int id)
        {
            var model = Db.ChiTietDonDatPhongs.FirstOrDefault(x => x.MaChiTiet == id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditDetail(ChiTietDonDatPhong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objCheck = Db.ChiTietDonDatPhongs.FirstOrDefault(x => x.MaPhong == model.MaPhong && x.MaDonDatPhong == model.MaDonDatPhong && x.MaKhachSan == model.MaKhachSan && x.MaChiTiet != model.MaChiTiet);
                    if (objCheck == null)
                    {
                        var obj = Db.ChiTietDonDatPhongs.FirstOrDefault(x => x.MaChiTiet == model.MaChiTiet);
                        obj.MaKhachSan = model.MaKhachSan;
                        obj.MaPhong = model.MaPhong;
                        obj.SoNguoi = model.SoNguoi;

                        Db.ChiTietDonDatPhongs.Attach(obj);
                        Db.Entry(obj).State = EntityState.Modified;
                        Db.SaveChanges();
                        TempData["notice"] = "Sửa thành công!";

                        return Redirect("/Admin/Order/View/" + model.MaDonDatPhong);
                    }
                    else
                    {
                        TempData["notice"] = "Loại phòng đã tồn tại! Vui lòng chọn loại phòng khác!";
                    }
                }
                catch
                {
                    TempData["notice"] = "Sửa không thành công!";
                }
            }
            return View(model);
        }

        public ActionResult DeleteDetail(int id)
        {
            int? ma = 0;
            try
            {
                var model = Db.ChiTietDonDatPhongs.FirstOrDefault(x => x.MaChiTiet == id);

                ma = model.MaDonDatPhong;

                Db.ChiTietDonDatPhongs.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.ChiTietDonDatPhongs.Remove(model);
                Db.SaveChanges();
                TempData["notice"] = "Xóa thành công!";
            }
            catch
            {
                TempData["notice"] = "Xóa không thành công! Nguyên nhân: Có ràng buộc cơ sở dữ liệu!";
            }

            if (ma == 0)
            {
                return RedirectToAction("Index");
            }
            return Redirect("/Admin/Order/View/" + ma);
        }

        public ActionResult UpdateStatus(int? ma, string trangthai)
        {
            try
            {
                var model = Db.DonDatPhongs.FirstOrDefault(x => x.MaDonDatPhong == ma);
                HttpCookie cookie = Request.Cookies.Get("MaTaiKhoan");

                model.NgaySuLy = DateTime.Now;
                model.TrangThai = trangthai;
                model.MaTaiKhoan = int.Parse(cookie.Value);

                Db.DonDatPhongs.Attach(model);
                Db.Entry(model).State = EntityState.Modified;
                Db.SaveChanges();
                TempData["notice"] = "Cập nhật thành công!";
            }
            catch
            {
                TempData["notice"] = "Cập nhật trạng thái không thành công!";
            }


            return Redirect("/Admin/Order/View/" + ma);
        }
    }
}