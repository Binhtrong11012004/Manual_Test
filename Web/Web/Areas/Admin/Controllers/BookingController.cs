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
    [PemisitonAttribute("Book phòng")]
    public class BookingController : BaseController
    {
        // GET: Admin/Booking
        public ActionResult Index(int? KhachSan, int? LoaiPhong, string TrangThai)
        {
            SearchBooking model = new SearchBooking();
            var list = Db.Phongs.ToList();
            if(KhachSan>0)
            {
                list = list.Where(x => x.MaKhachSan == KhachSan).ToList();
            }

            if (LoaiPhong > 0)
            {
                list = list.Where(x => x.MaLoaiPhong == LoaiPhong).ToList();
            }

            if (!string.IsNullOrEmpty(TrangThai))
            {
                list = list.Where(x => x.TrangThai == TrangThai).ToList();
            }

            model.ListPhong = list;
            model.KhachSan = KhachSan;
            model.LoaiPhong = LoaiPhong;
            model.TrangThai = TrangThai;
            return View(model);
        }

        public ActionResult Clean(int? id)
        {
            try
            {
                var objCheck = Db.Phongs.FirstOrDefault(x => x.MaPhong == id && x.TrangThai == "Chưa sử dụng");
                if (objCheck != null)
                {
                    var obj = Db.Phongs.FirstOrDefault(x => x.MaPhong == id);
                    obj.TrangThai = "Đang dọn";

                    Db.Phongs.Attach(obj);
                    Db.Entry(obj).State = EntityState.Modified;
                    Db.SaveChanges();
                    TempData["notice"] = "Thao tác thành công!";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "Thao tác không thành công!";
                }
            }
            catch
            {
                TempData["notice"] = "Thao tác không thành công!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult CleanOk(int? id)
        {
            try
            {
                var objCheck = Db.Phongs.FirstOrDefault(x => x.MaPhong == id && x.TrangThai == "Đang dọn");
                if (objCheck != null)
                {
                    var obj = Db.Phongs.FirstOrDefault(x => x.MaPhong == id);
                    obj.TrangThai = "Chưa sử dụng";

                    Db.Phongs.Attach(obj);
                    Db.Entry(obj).State = EntityState.Modified;
                    Db.SaveChanges();
                    TempData["notice"] = "Thao tác thành công!";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "Thao tác không thành công!";
                }
            }
            catch
            {
                TempData["notice"] = "Thao tác không thành công!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Book(int? id)
        {
            try
            {
                var objCheck = Db.Phongs.FirstOrDefault(x => x.MaPhong == id && x.TrangThai == "Chưa sử dụng");
                if (objCheck != null)
                {
                    var phongKhachHang = new PhongKhachHang
                    {
                        MaPhong = id,
                        NgayDen = DateTime.Now
                    };

                    return View(phongKhachHang);
                }
                else
                {
                    TempData["notice"] = "Thao tác không thành công!";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["notice"] = "Thao tác không thành công!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Book(PhongKhachHang model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.TrangThai = "Chờ thanh toán";
                    model.NgayDen = DateTime.Now;
                    Db.PhongKhachHangs.Add(model);
                    Db.SaveChanges();

                    var obj = Db.Phongs.FirstOrDefault(x => x.MaPhong == model.MaPhong);
                    obj.TrangThai = "Đang sử dụng";

                    Db.Phongs.Attach(obj);
                    Db.Entry(obj).State = EntityState.Modified;
                    Db.SaveChanges();

                    TempData["notice"] = "Đặt phòng thành công!";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["notice"] = "Đặt phòng không thành công!";
                }
            }

            return View(model);
        }

        public ActionResult Pay(int? id)
        {
            var model = Db.PhongKhachHangs.FirstOrDefault(x => x.MaPhongKhachHang == id && x.TrangThai == "Chờ thanh toán");
            return View(model);
        }

        public ActionResult Service(int? id)
        {
            var model = Db.PhongKhachHangs.FirstOrDefault(x => x.MaPhongKhachHang == id && x.TrangThai == "Chờ thanh toán");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddService(SuDungDichVu model)
        {
            try
            {
                model.ThoiGianSuDung = DateTime.Now;
                Db.SuDungDichVus.Add(model);
                Db.SaveChanges();
                TempData["notice"] = "Thêm dịch vụ thành công!";
                return Redirect("/Admin/Booking/Service/" + model.MaPhongKhachHang);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteService(int? id)
        {
            try
            {
                int? ma = 0;
                var model = Db.SuDungDichVus.FirstOrDefault(x => x.MaSuDungDichVu == id);
                ma = model.MaPhongKhachHang;
                Db.SuDungDichVus.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.SuDungDichVus.Remove(model);
                Db.SaveChanges();
                TempData["notice"] = "Xóa thành công!";
                return Redirect("/Admin/Booking/Service/" + ma);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult EditService(int? MaSuDungDichVu, int? SoLuong)
        {
            try
            {
                int? ma = 0;
                var model = Db.SuDungDichVus.FirstOrDefault(x => x.MaSuDungDichVu == MaSuDungDichVu);
                model.SoLuong = SoLuong;
                ma = model.MaPhongKhachHang;

                Db.SuDungDichVus.Attach(model);
                Db.Entry(model).State = EntityState.Modified;
                Db.SaveChanges();
                TempData["notice"] = "Cập nhật thành công!";
                return Redirect("/Admin/Booking/Service/" + ma);
            }
            catch
            {
                TempData["notice"] = "Cập nhật không thành công!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult AddCustomer(int? id)
        {
            var model = Db.PhongKhachHangs.FirstOrDefault(x => x.MaPhongKhachHang == id && x.TrangThai == "Chờ thanh toán");
            return View(model);
        }

        [HttpPost]
        public ActionResult AddCustomer(KhachDiKem model)
        {
            try
            {
                var obj = Db.PhongKhachHangs.FirstOrDefault(x => x.MaPhongKhachHang == model.MaPhongKhachHang);
                Db.KhachDiKems.Add(model);
                Db.SaveChanges();
                TempData["notice"] = "Thêm thành công!";
                return Redirect("/Admin/Booking/AddCustomer/" + model.MaPhongKhachHang);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteCustomer(int? id)
        {
            try
            {
                int? ma = 0;
                var model = Db.KhachDiKems.FirstOrDefault(x => x.MaKhachDiKem == id);
                ma = model.MaPhongKhachHang;
                Db.KhachDiKems.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.KhachDiKems.Remove(model);
                Db.SaveChanges();
                TempData["notice"] = "Xóa thành công!";
                return Redirect("/Admin/Booking/AddCustomer/" + ma);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult PaySub(int? id)
        {
            try
            {
                var model = Db.PhongKhachHangs.FirstOrDefault(x => x.MaPhongKhachHang == id && x.TrangThai == "Chờ thanh toán");
                model.NgayDi = DateTime.Now;
                model.TrangThai = "Đã thanh toán";

                Db.PhongKhachHangs.Attach(model);
                Db.Entry(model).State = EntityState.Modified;
                Db.SaveChanges();

                var songay = ((int)(DateTime.Now - model.NgayDen).Value.TotalDays) == 0 ? 1 : ((int)(DateTime.Now - model.NgayDen).Value.TotalDays);
                var sotienphong = (songay * model.Phong.LoaiPhong.DonGia);
                var tindichvu = model.SuDungDichVus.Sum(x => x.DichVu.Gia * x.SoLuong);
                var tong = sotienphong + tindichvu;

                var chitiet = new ChiTietThanhToan
                {
                    MaPhongKhachHang = model.MaPhongKhachHang,
                    ThoiGianThanhToan = DateTime.Now,
                    GhiChu = "Thanh toán tiền thuê phòng",
                    TienThanhToan = tong
                };
                Db.ChiTietThanhToans.Add(chitiet);
                Db.SaveChanges();

                var obj = Db.Phongs.FirstOrDefault(x => x.MaPhong == model.MaPhong);

                obj.TrangThai = "Đang dọn";
                Db.Phongs.Attach(obj);
                Db.Entry(obj).State = EntityState.Modified;
                Db.SaveChanges();


                TempData["notice"] = "Thanh toán thành công!";
            }
            catch
            {
                TempData["notice"] = "Thanh toán không thành công!";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                var model = Db.PhongKhachHangs.FirstOrDefault(x => x.MaPhongKhachHang == id && x.TrangThai == "Chờ thanh toán");
                var ma = model.MaPhong;
                Db.PhongKhachHangs.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.PhongKhachHangs.Remove(model);
                Db.SaveChanges();

                var obj = Db.Phongs.FirstOrDefault(x => x.MaPhong == ma);

                obj.TrangThai = "Chưa sử dụng";
                Db.Phongs.Attach(obj);
                Db.Entry(obj).State = EntityState.Modified;
                Db.SaveChanges();


                TempData["notice"] = "Hủy thành công!";
            }
            catch
            {
                TempData["notice"] = "Hủy không thành công! do đã sử dụng dịch vụ hoặc vẫn có người đi kèm";
            }

            return RedirectToAction("Index");
        }
    }
}