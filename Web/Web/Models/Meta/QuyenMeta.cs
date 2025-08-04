using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Meta
{
    public class QuyenMeta
    {
        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public string[] DanhSach { get; set; }
    }
}