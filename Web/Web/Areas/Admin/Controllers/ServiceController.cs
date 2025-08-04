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
    [PemisitonAttribute("Dịch vụ")]
    public class ServiceController : BaseController
    {
        // GET: Admin/Service
        public ActionResult Index()
        {
            var list = Db.DichVus.ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new DichVu());
        }

        [HttpPost]
        public ActionResult Add(DichVu model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.DichVus.FirstOrDefault(x => x.TenDichVu == model.TenDichVu);
                    if (obj == null)
                    {
                        Db.DichVus.Add(model);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên dịch vụ đã tồn tại! Vui lòng chọn tên khác!";
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
            var model = Db.DichVus.FirstOrDefault(x => x.MaDichVu == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(DichVu model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var objCheck = Db.DichVus.FirstOrDefault(x => x.TenDichVu == model.TenDichVu && x.MaDichVu == model.MaDichVu);
                    if (objCheck == null)
                    {
                        var obj = Db.DichVus.FirstOrDefault(x => x.MaDichVu == model.MaDichVu);
                        obj.TenDichVu = model.TenDichVu;
                        obj.Gia = model.Gia;

                        Db.DichVus.Attach(obj);
                        Db.Entry(obj).State = EntityState.Modified;
                        Db.SaveChanges();
                        TempData["notice"] = "Sửa thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên dịch vụ đã tồn tại! Vui lòng chọn tên khác!";
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
                var model = Db.DichVus.FirstOrDefault(x => x.MaDichVu == id);
                Db.DichVus.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.DichVus.Remove(model);
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