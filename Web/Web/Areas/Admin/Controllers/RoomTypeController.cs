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
    [PemisitonAttribute("Loại phòng")]
    public class RoomTypeController : BaseController
    {
        // GET: Admin/RoomType
        public ActionResult Index()
        {
            var list = Db.LoaiPhongs.ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new LoaiPhong());
        }

        [HttpPost]
        public ActionResult Add(LoaiPhong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.LoaiPhongs.FirstOrDefault(x => x.MaLoaiPhong == model.MaLoaiPhong);
                    if (obj == null)
                    {
                        Db.LoaiPhongs.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên loại phòng đã tồn tại! Vui lòng chọn tên khác!";
                    }
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
            var model = Db.LoaiPhongs.FirstOrDefault(x => x.MaLoaiPhong == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(LoaiPhong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objCheck = Db.LoaiPhongs.FirstOrDefault(x => x.TenLoaiPhong == model.TenLoaiPhong && x.MaLoaiPhong != model.MaLoaiPhong);
                    if (objCheck == null)
                    {
                        var obj = Db.LoaiPhongs.FirstOrDefault(x => x.MaLoaiPhong == model.MaLoaiPhong);
                        obj.TenLoaiPhong = model.TenLoaiPhong;
                        obj.HinhAnh = model.HinhAnh;
                        obj.SoGiuong = model.SoGiuong;
                        obj.SoNguoi = model.SoNguoi;
                        obj.DonGia = model.DonGia;
                        obj.MoTa = model.MoTa;

                        Db.LoaiPhongs.Attach(obj);
                        Db.Entry(obj).State = EntityState.Modified;
                        Db.SaveChanges();
                        TempData["notice"] = "Sửa thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên loại phòng đã tồn tại! Vui lòng chọn tên khác!";
                    }
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
                var model = Db.LoaiPhongs.FirstOrDefault(x => x.MaLoaiPhong == id);
                Db.LoaiPhongs.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.LoaiPhongs.Remove(model);
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
            var model = Db.LoaiPhongs.FirstOrDefault(x => x.MaLoaiPhong == id);
            return View(model);
        }

        public ActionResult AddConvenient(int id)
        {
            var model = new TienNghi
            {
                MaLoaiPhong = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddConvenient(TienNghi model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.TienNghis.FirstOrDefault(x => x.MaLoaiPhong == model.MaLoaiPhong && x.TenTienNghi == model.TenTienNghi);
                    if (obj == null)
                    {
                        Db.TienNghis.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return Redirect("/Admin/RoomType/View/" + model.MaLoaiPhong);
                    }
                    else
                    {
                        TempData["notice"] = "Tên tiện nghi đã tồn tại! Vui lòng chọn tên câu hỏi khác!";
                    }
                }
                catch
                {
                    TempData["notice"] = "Thêm không thành công!";
                }
            }

            return View(model);
        }

        public ActionResult EditConvenient(int id)
        {
            var model = Db.TienNghis.FirstOrDefault(x => x.MaTienNghi == id);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditConvenient(TienNghi model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objCheck = Db.TienNghis.FirstOrDefault(x => x.MaLoaiPhong == model.MaLoaiPhong && x.TenTienNghi == model.TenTienNghi && x.MaTienNghi != model.MaTienNghi);
                    if (objCheck == null)
                    {
                        var obj = Db.TienNghis.FirstOrDefault(x => x.MaTienNghi == model.MaTienNghi);
                        obj.TenTienNghi = model.TenTienNghi;

                        Db.TienNghis.Attach(obj);
                        Db.Entry(obj).State = EntityState.Modified;
                        Db.SaveChanges();
                        TempData["notice"] = "Sửa thành công!";

                        return Redirect("/Admin/RoomType/View/" + model.MaLoaiPhong);
                    }
                    else
                    {
                        TempData["notice"] = "Tên tiện nghi đã tồn tại! Vui lòng chọn tên câu hỏi khác!";
                    }
                }
                catch
                {
                    TempData["notice"] = "Sửa không thành công!";
                }
            }
            return View(model);
        }

        public ActionResult DeleteConvenient(int id)
        {
            int? ma = 0;
            try
            {
                var model = Db.TienNghis.FirstOrDefault(x => x.MaTienNghi == id);

                ma = model.MaLoaiPhong;

                Db.TienNghis.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.TienNghis.Remove(model);
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
            return Redirect("/Admin/RoomType/View/" + ma);
        }
    }
}