using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Attributes;
using Web.Models;

namespace Web.Controllers
{
    [CustomerAuthorize]
    public class BookingController : BaseController
    {
        // GET: Booking
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? khachsan, int? maphong, DateTime? ngayden, DateTime? ngaydi, int? songuoi)
        {
            HttpCookie cookied = Request.Cookies.Get("MaKhach");
            var ma = int.Parse(cookied.Value);
            var kh = db.KhachHangs.FirstOrDefault(x => x.MaKhachHang == ma);
            if (khachsan != null && maphong != null && ngayden != null && ngaydi != null && songuoi != null)
            {
                var donDatPhong = new DonDatPhong
                {
                    NgayDatPhong = DateTime.Now,
                    NgayDen = ngayden,
                    NgayDi = ngaydi,
                    MaKhachHang = ma,
                    HoTen = kh.HoTen,
                    SoDienThoai = kh.SDT,
                    Email = kh.Email,
                };

                Session["DonDatPhong"] = donDatPhong;

                var chiTiet = new ChiTietDonDatPhong
                {
                    MaKhachSan = khachsan.Value,
                    KhachSan = db.KhachSans.FirstOrDefault(x => x.MaKhachSan == khachsan),
                    MaPhong = maphong.Value,
                    Phong = db.Phongs.FirstOrDefault(x => x.MaPhong == maphong),
                    SoNguoi = songuoi
                };
                var listChiTiet = new List<ChiTietDonDatPhong>();
                listChiTiet.Add(chiTiet);
                Session["ChiTietDonDatPhong"] = listChiTiet;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Add(int? khachsan, int? maphong, int? songuoi = 1, string url = "")
        {
            if (khachsan != null && maphong != null)
            {
                var chiTiet = new ChiTietDonDatPhong
                {
                    MaKhachSan = khachsan.Value,
                    KhachSan = db.KhachSans.FirstOrDefault(x => x.MaKhachSan == khachsan),
                    MaPhong = maphong.Value,
                    Phong = db.Phongs.FirstOrDefault(x => x.MaPhong == maphong),
                    SoNguoi = songuoi
                };
                if (Session["ChiTietDonDatPhong"] != null)
                {
                    var chitetdatphongs = Session["ChiTietDonDatPhong"] as List<ChiTietDonDatPhong>;
                    if (chitetdatphongs.Count(x => x.MaPhong == maphong && x.MaKhachSan == khachsan) == 0)
                    {
                        chitetdatphongs.Add(chiTiet);
                        Session["ChiTietDonDatPhong"] = chitetdatphongs;
                        TempData["mess"] = "Thêm thành công";
                    }
                    else
                    {
                        var chitiet = chitetdatphongs.FirstOrDefault(x => x.MaPhong == maphong && x.MaKhachSan == khachsan);
                        chitetdatphongs.Remove(chitiet);
                        chiTiet.SoNguoi =  songuoi;
                        chitetdatphongs.Add(chiTiet);

                        Session["ChiTietDonDatPhong"] = chitetdatphongs;
                        TempData["mess"] = "Thêm thành công";
                    }
                }
                else
                {
                    var listChiTiet = new List<ChiTietDonDatPhong>();
                    listChiTiet.Add(chiTiet);
                    Session["ChiTietDonDatPhong"] = listChiTiet;
                    TempData["mess"] = "Thêm thành công";
                }
            }

            if (!string.IsNullOrEmpty(url))
                return Redirect(url);

            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id, int ks)
        {
            if (Session["ChiTietDonDatPhong"] != null)
            {
                var chitetdatphongs = Session["ChiTietDonDatPhong"] as List<ChiTietDonDatPhong>;
                var chitiet = chitetdatphongs.FirstOrDefault(x => x.MaPhong == id && x.MaKhachSan == ks);
                chitetdatphongs.Remove(chitiet);
                Session["ChiTietDonDatPhong"] = chitetdatphongs;
                TempData["mess"] = "Xóa thành công";
            }
            else
            {
                TempData["mess"] = "Xóa không thành công";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Send(DateTime? ngayden, DateTime? ngaydi, string hoten, string sdt, string email, string ghichu)
        {
            HttpCookie cookied = Request.Cookies.Get("MaKhach");
            var ma = int.Parse(cookied.Value);
            var kh = db.KhachHangs.FirstOrDefault(x => x.MaKhachHang == ma);
            if (Session["ChiTietDonDatPhong"] != null)
            {
                if (ngayden != null && ngaydi != null && hoten != null && sdt != null)
                {
                    if (ngayden > ngaydi)
                    {
                        var donDatPhongA = new DonDatPhong
                        {
                            NgayDen = ngayden,
                            NgayDi = ngaydi,
                            NgayDatPhong = DateTime.Now,
                            Email = email,
                            GhiChu = ghichu,
                            HoTen = hoten,
                            SoDienThoai = sdt,
                            MaKhachHang = ma,
                        };
                        Session["DonDatPhongA"] = donDatPhongA;
                        TempData["mess"] = "Ngày đến phải nhỏ hơn ngày đi";
                        return RedirectToAction("Index");
                    }

                    var chitetdatphongs = Session["ChiTietDonDatPhong"] as List<ChiTietDonDatPhong>;
                    var donDatPhong = new DonDatPhong
                    {
                        NgayDen = ngayden,
                        NgayDi = ngaydi,
                        NgayDatPhong = DateTime.Now,
                        Email = email,
                        GhiChu = ghichu,
                        HoTen = hoten,
                        SoDienThoai = sdt,
                        MaTaiKhoan = 1,
                        TrangThai = "Mới",
                        MaKhachHang = ma,
                    };

                    db.DonDatPhongs.Add(donDatPhong);
                    db.SaveChanges();

                    foreach (var item in chitetdatphongs)
                    {
                        var chitiet = new ChiTietDonDatPhong
                        {
                            MaKhachSan = item.MaKhachSan,
                            MaDonDatPhong = donDatPhong.MaDonDatPhong,
                            MaPhong = item.MaPhong,
                            SoNguoi = item.SoNguoi
                        };

                        db.ChiTietDonDatPhongs.Add(chitiet);
                        db.SaveChanges();
                    }

                    Session["ChiTietDonDatPhong"] = null;
                    Session["DonDatPhongA"] = null;
                    Session["DonDatPhong"] = null;

                    return View("_ThankYou");
                }
                else
                {
                    var donDatPhong = new DonDatPhong
                    {
                        NgayDen = ngayden,
                        NgayDi = ngaydi,
                        NgayDatPhong = DateTime.Now,
                        Email = email,
                        GhiChu = ghichu,
                        HoTen = hoten,
                        SoDienThoai = sdt,
                        MaKhachHang = ma,
                    };
                    Session["DonDatPhongA"] = donDatPhong;

                    TempData["mess"] = "Chưa cung cấp đầy đủ thông tin ngày đến, ngày đi, họ tên, số điện thoại";
                }
            }
            else
            {
                var donDatPhong = new DonDatPhong
                {
                    NgayDen = ngayden,
                    NgayDi = ngaydi,
                    NgayDatPhong = DateTime.Now,
                    Email = email,
                    GhiChu = ghichu,
                    HoTen = hoten,
                    SoDienThoai = sdt,
                    MaKhachHang = ma,
                };
                Session["DonDatPhongA"] = donDatPhong;

                TempData["mess"] = "Chưa cung cấp đầy đủ thông tin loại phòng, số người, số phòng";
            }
            return RedirectToAction("Index");
        }
    }
}