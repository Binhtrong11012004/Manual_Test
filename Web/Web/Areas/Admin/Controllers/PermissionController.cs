using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Attributes;
using Web.Models;
using Web.Models.Meta;

namespace Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [PemisitonAttribute("Quyền")]
    public class PermissionController : BaseController
    {
        // GET: Admin/Permission
        public ActionResult Index()
        {
            var list = Db.PhanQuyens.ToList();
            return View(list);
        }

        public ActionResult Add()
        {
            return View(new QuyenMeta());
        }

        [HttpPost]
        public ActionResult Add(QuyenMeta model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.PhanQuyens.FirstOrDefault(x => x.TenQuyen == model.TenQuyen);
                    if (obj == null)
                    {
                        var phanQuyen = new PhanQuyen
                        {
                            TenQuyen = model.TenQuyen,
                            DanhSach = "," + string.Join(",", model.DanhSach) + ","
                        };
                        Db.PhanQuyens.Add(phanQuyen);
                        Db.SaveChanges();
                        TempData["notice"] = "Thêm thành công!";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Tên quyền đã tồn tại!";
                    }
                }
                catch
                {
                    TempData["notice"] = "Thêm không thành công!";
                }
            }

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            var model = Db.PhanQuyens.FirstOrDefault(x => x.MaQuyen == id);
            var data = new QuyenMeta
            {
                MaQuyen = model.MaQuyen,
                TenQuyen = model.TenQuyen,
                DanhSach = model.DanhSach.Split(',')
            };
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(QuyenMeta model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var obj = Db.PhanQuyens.FirstOrDefault(x => x.MaQuyen == model.MaQuyen);
                    obj.DanhSach = "," + string.Join(",", model.DanhSach) + ",";

                    Db.PhanQuyens.Attach(obj);
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

        public ActionResult Delete(int? id)
        {
            try
            {
                var model = Db.PhanQuyens.FirstOrDefault(x => x.MaQuyen == id);
                Db.PhanQuyens.Attach(model);
                Db.Entry(model).State = EntityState.Deleted;
                Db.PhanQuyens.Remove(model);
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