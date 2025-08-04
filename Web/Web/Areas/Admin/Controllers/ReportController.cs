using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [PemisitonAttribute("Báo cáo")]
    public class ReportController : BaseController
    {
        // GET: Admin/Report
        public ActionResult Index(DateTime? TuNgay, DateTime? DenNgay)
        {

            TuNgay = TuNgay ?? DateTime.Now.AddDays(-30);
            DenNgay = DenNgay ?? DateTime.Now;
            var viewAll = new ViewAll
            {
                TuNgay = TuNgay,
                DenNgay = DenNgay
            };

            var theoNgay = new List<ViewRe>();
            var theoThang = new List<ViewRe>();

            foreach (DateTime day in EachCalendarDay(TuNgay, DenNgay))
            {
                var list = Db.PhongKhachHangs.Where(x => x.NgayDi.Value.Year == day.Year && x.NgayDi.Value.Month == day.Month && x.NgayDi.Value.Day == day.Day && x.TrangThai == "Đã thanh toán").ToList();
                int? tong = 0;
                int? phong = 0;
                int? dv = 0;
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var songay = ((int)(item.NgayDi - item.NgayDen).Value.TotalDays) == 0 ? 1 : ((int)(item.NgayDi - item.NgayDen).Value.TotalDays);
                        phong += (songay * item.Phong.LoaiPhong.DonGia);
                        dv += item.SuDungDichVus.Sum(x => x.DichVu.Gia * x.SoLuong);
                    }
                }
                tong = phong + dv;
                var re = new ViewRe
                {
                    Ngay = day,
                    TienDichVu = dv,
                    TienPhong = phong,
                    Tong = tong
                };
                theoNgay.Add(re);
            }

            viewAll.TheoNgay = theoNgay.OrderByDescending(x => x.Ngay).ToList();

            DateTime? date = null;
            foreach (var item in theoNgay)
            {
                if (date == null)
                {
                    var re = new ViewRe
                    {
                        Ngay = new DateTime(item.Ngay.Value.Year, item.Ngay.Value.Month, item.Ngay.Value.Day),
                        TienDichVu = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.TienDichVu),
                        TienPhong = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.TienPhong),
                        Tong = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.Tong)
                    };
                    theoThang.Add(re);
                    date = item.Ngay;
                }
                else
                {
                    if (new DateTime(date.Value.Year, date.Value.Month, 1) != new DateTime(item.Ngay.Value.Year, item.Ngay.Value.Month, 1))
                    {
                        var re = new ViewRe
                        {
                            Ngay = new DateTime(item.Ngay.Value.Year, item.Ngay.Value.Month, item.Ngay.Value.Day),
                            TienDichVu = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.TienDichVu),
                            TienPhong = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.TienPhong),
                            Tong = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.Tong)
                        };
                        theoThang.Add(re);
                        date = item.Ngay;
                    }
                }
            }

            viewAll.TheoThang = theoThang.OrderByDescending(x => x.Ngay).ToList();

            return View(viewAll);
        }

        public IEnumerable<DateTime> EachCalendarDay(DateTime? startDate, DateTime? endDate)
        {
            for (var date = startDate.Value.Date; date.Date <= endDate.Value.Date; date = date.AddDays(1)) yield
            return date;
        }
    }

    public class ViewRe
    {
        public DateTime? Ngay { get; set; }
        public int? Tong { get; set; }
        public int? TienPhong { get; set; }
        public int? TienDichVu { get; set; }
    }

    public class ViewAll
    {
        public List<ViewRe> TheoNgay { get; set; }
        public List<ViewRe> TheoThang { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }
}