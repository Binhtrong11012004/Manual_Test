using Web.Attributes;
using Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [PemisitonAttribute("Tài khoản")]
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            var list = Db.TaiKhoans.ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new TaiKhoan());
        }

        [HttpPost]
        public ActionResult Add(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == model.TenDangNhap);
                    if (obj == null)
                    {
                        Db.TaiKhoans.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tài khoản đã tồn tại! Vui lòng chọn tài khoản khác!";
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
            var model = Db.TaiKhoans.FirstOrDefault(x => x.MaTaiKhoan == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.TaiKhoans.FirstOrDefault(x => x.MaTaiKhoan == model.MaTaiKhoan);
                    obj.HoTen = model.HoTen;
                    obj.GioiTinh = model.GioiTinh;
                    obj.NgaySinh = model.NgaySinh;
                    obj.Email = model.Email;
                    obj.SDT = model.SDT;
                    obj.MaQuyen = model.MaQuyen;

                    Db.TaiKhoans.Attach(obj);
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
                var model = Db.TaiKhoans.FirstOrDefault(x => x.MaTaiKhoan == id);
                Db.TaiKhoans.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.TaiKhoans.Remove(model);
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