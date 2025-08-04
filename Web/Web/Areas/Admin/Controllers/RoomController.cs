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
    [PemisitonAttribute("Phòng")]
    public class RoomController : BaseController
    {
        // GET: Admin/Room
        public ActionResult Index()
        {
            var list = Db.Phongs.ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new Phong());
        }

        [HttpPost]
        public ActionResult Add(Phong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.Phongs.FirstOrDefault(x => x.TenPhong == model.TenPhong);
                    if (obj == null)
                    {
                        model.TrangThai = "Chưa sử dụng";
                        Db.Phongs.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên phòng đã tồn tại! Vui lòng chọn tên khác!";
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
            var model = Db.Phongs.FirstOrDefault(x => x.MaPhong == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Phong model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objCheck = Db.Phongs.FirstOrDefault(x => x.TenPhong == model.TenPhong && x.MaPhong == model.MaPhong);
                    if (objCheck == null)
                    {
                        var obj = Db.Phongs.FirstOrDefault(x => x.MaLoaiPhong == model.MaLoaiPhong);
                        obj.TenPhong = model.TenPhong;
                        obj.ViTri = model.ViTri;
                        obj.HinhAnh = model.HinhAnh;
                        obj.DienTich = model.DienTich;
                        obj.GioiThieu = model.GioiThieu;

                        Db.Phongs.Attach(obj);
                        Db.Entry(obj).State = EntityState.Modified;
                        Db.SaveChanges();
                        TempData["notice"] = "Sửa thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên phòng đã tồn tại! Vui lòng chọn tên khác!";
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
                var model = Db.Phongs.FirstOrDefault(x => x.MaPhong == id);
                Db.Phongs.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.Phongs.Remove(model);
                Db.SaveChanges();
                TempData["notice"] = "Xóa thành công!";
            }
            catch
            {
                TempData["notice"] = "Xóa không thành công! Nguyên nhân: Có ràng buộc cơ sở dữ liệu!";
            }

            return RedirectToAction("Index");
        }
    }
}