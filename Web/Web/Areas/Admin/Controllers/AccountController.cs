using Web.Attributes;
using Web.Models.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            HttpCookie cookie = Request.Cookies.Get("HoTenTaiKhoan");
            if (cookie != null)
            {
                return Redirect("/Admin/Home");
            }
            return View(new DangNhapMeta());
        }

        [HttpPost]
        public ActionResult Login(DangNhapMeta model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = Db.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == model.TaiKhoan && x.MatKhau == model.MatKhau);

                if (taiKhoan != null)
                {
                    HttpCookie cookie = new HttpCookie("HoTenTaiKhoan", Server.UrlEncode(taiKhoan.HoTen));
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(cookie);

                    HttpCookie maTaiKhoan = new HttpCookie("MaTaiKhoan", taiKhoan.MaTaiKhoan.ToString());
                    maTaiKhoan.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Add(maTaiKhoan);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return Redirect("/Admin/Account/Login");
                }
                else
                {
                    ModelState.AddModelError("MatKhau", "Sai tên đăng nhập hoặc mật khẩu");
                    return View(model);
                }
            }
            return View(model);
        }

        [AdminAuthorize]
        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies.Get("HoTenTaiKhoan");
            cookie.Expires = DateTime.Now;
            Response.Cookies.Add(cookie);

            HttpCookie maTaiKhoan = Request.Cookies.Get("MaTaiKhoan");
            maTaiKhoan.Expires = DateTime.Now;
            Response.Cookies.Add(maTaiKhoan);
            return Redirect("/Admin/Account/Login");
        }
    }
}