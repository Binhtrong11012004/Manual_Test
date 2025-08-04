using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Attributes;

namespace Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [PemisitonAttribute("Thống kê")]
    public class StatisticController : BaseController
    {
        // GET: Admin/Statistic
        public ActionResult Index(DateTime? TuNgay, DateTime? DenNgay)
        {

            TuNgay = TuNgay ?? DateTime.Now.AddDays(-30);
            DenNgay = DenNgay ?? DateTime.Now;
            var viewAll = new ViewAllF
            {
                TuNgay = TuNgay,
                DenNgay = DenNgay
            };

            var theoNgay = new List<ViewReF>();
            var theoThang = new List<ViewReF>();

            foreach (DateTime day in EachCalendarDay(TuNgay, DenNgay))
            {
                var list = Db.PhongKhachHangs.Where(x => x.NgayDi.Value.Year == day.Year && x.NgayDi.Value.Month == day.Month && x.NgayDi.Value.Day == day.Day && x.TrangThai == "Đã thanh toán").ToList();

                var re = new ViewReF
                {
                    Ngay = day,
                    LuongPhong = list.Count(),
                    LuongKhach = list.Count() + list.Sum(x => x.KhachDiKems.Count)
                };
                theoNgay.Add(re);
            }

            viewAll.TheoNgay = theoNgay.OrderByDescending(x => x.Ngay).ToList();

            DateTime? date = null;
            foreach (var item in theoNgay)
            {
                if (date == null)
                {
                    var re = new ViewReF
                    {
                        Ngay = new DateTime(item.Ngay.Value.Year, item.Ngay.Value.Month, item.Ngay.Value.Day),
                        LuongKhach = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.LuongKhach),
                        LuongPhong = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.LuongPhong),
                    };
                    theoThang.Add(re);
                    date = item.Ngay;
                }
                else
                {
                    if (new DateTime(date.Value.Year, date.Value.Month, 1) != new DateTime(item.Ngay.Value.Year, item.Ngay.Value.Month, 1))
                    {
                        var re = new ViewReF
                        {
                            Ngay = new DateTime(item.Ngay.Value.Year, item.Ngay.Value.Month, item.Ngay.Value.Day),
                            LuongKhach = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.LuongKhach),
                            LuongPhong = theoNgay.Where(x => x.Ngay.Value.Month == item.Ngay.Value.Month && x.Ngay.Value.Year == item.Ngay.Value.Year).Sum(x => x.LuongPhong)
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

    public class ViewReF
    {
        public DateTime? Ngay { get; set; }
        public int? LuongPhong { get; set; }
        public int? LuongKhach { get; set; }
    }

    public class ViewAllF
    {
        public List<ViewReF> TheoNgay { get; set; }
        public List<ViewReF> TheoThang { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }
}