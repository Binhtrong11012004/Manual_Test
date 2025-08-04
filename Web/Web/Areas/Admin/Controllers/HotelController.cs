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
    [PemisitonAttribute("Khách sạn")]
    public class HotelController : BaseController
    {
        // GET: Admin/Hotel
        public ActionResult Index()
        {
            var list = Db.KhachSans.ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new KhachSan());
        }

        [HttpPost]
        public ActionResult Add(KhachSan model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.KhachSans.FirstOrDefault(x => x.TenKhachSan == model.TenKhachSan);
                    if (obj == null)
                    {
                        Db.KhachSans.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên khách sạn đã tồn tại! Vui lòng chọn tên khác!";
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
            var model = Db.KhachSans.FirstOrDefault(x => x.MaKhachSan == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(KhachSan model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objCheck = Db.KhachSans.FirstOrDefault(x => x.TenKhachSan == model.TenKhachSan && x.MaKhachSan != model.MaKhachSan);
                    if (objCheck == null)
                    {
                        var obj = Db.KhachSans.FirstOrDefault(x => x.MaKhachSan == model.MaKhachSan);
                        obj.TenKhachSan = model.TenKhachSan;
                        obj.HinhAnh = model.HinhAnh;
                        obj.DiaChi = model.DiaChi;
                        obj.Email = model.Email;
                        obj.SDT = model.SDT;

                        Db.KhachSans.Attach(obj);
                        Db.Entry(obj).State = EntityState.Modified;
                        Db.SaveChanges();
                        TempData["notice"] = "Sửa thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên khách sạn đã tồn tại! Vui lòng chọn tên khác!";
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
                var model = Db.KhachSans.FirstOrDefault(x => x.MaKhachSan == id);
                Db.KhachSans.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.KhachSans.Remove(model);
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