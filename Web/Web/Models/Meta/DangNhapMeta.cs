using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models.Meta
{
    public class DangNhapMeta
    {
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string TaiKhoan { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
    }
}