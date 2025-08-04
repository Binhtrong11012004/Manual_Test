using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class SearchBooking
    {
        public List<Phong> ListPhong { get; set; }
        public int? KhachSan { get; set; }
        public int? LoaiPhong { get; set; }
        public string TrangThai { get; set; }
    }
}