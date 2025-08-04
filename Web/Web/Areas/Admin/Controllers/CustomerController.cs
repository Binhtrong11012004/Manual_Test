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
    [PemisitonAttribute("Khách hàng")]
    public class CustomerController : BaseController
    {
        // GET: Admin/Customer
        public ActionResult Index()
        {
            var list = Db.KhachHangs.ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new KhachHang());
        }

        [HttpPost]
        public ActionResult Add(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.KhachHangs.FirstOrDefault(x => x.SDT == model.SDT);
                    if (obj == null)
                    {
                        Db.KhachHangs.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Số điện thoại đã tồn tại! Vui lòng chọn số điện thoại khác!";
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
            var model = Db.KhachHangs.FirstOrDefault(x => x.MaKhachHang == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(KhachHang model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objCheck = Db.KhachHangs.FirstOrDefault(x => x.SDT == model.SDT && x.MaKhachHang != model.MaKhachHang);
                    if (objCheck == null)
                    {
                        var obj = Db.KhachHangs.FirstOrDefault(x => x.MaKhachHang == model.MaKhachHang);
                        obj.HoTen = model.HoTen;
                        obj.GioiTinh = model.GioiTinh;
                        obj.NgaySinh = model.NgaySinh;
                        obj.Email = model.Email;
                        obj.SDT = model.SDT;
                        obj.CMTND = model.CMTND;
                        obj.LoaiKhachHang = model.LoaiKhachHang;

                        Db.KhachHangs.Attach(obj);
                        Db.Entry(obj).State = EntityState.Modified;
                        Db.SaveChanges();
                        TempData["notice"] = "Sửa thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Số điện thoại đã tồn tại! Vui lòng chọn số điện thoại khác!";
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
                var model = Db.KhachHangs.FirstOrDefault(x => x.MaKhachHang == id);
                Db.KhachHangs.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.KhachHangs.Remove(model);
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