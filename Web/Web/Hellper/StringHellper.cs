using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Hellper
{
    public static class StringHellper
    {
        public static string RemoveHtmlTagsUsingCharArray(this string htmlString)
        {
            var array = new char[htmlString.Length];
            var arrayIndex = 0;
            var inside = false;

            foreach (var @let in htmlString)
            {
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (inside) continue;
                array[arrayIndex] = let;
                arrayIndex++;
            }
            return new string(array, 0, arrayIndex);
        }

        public static string ConvertDescriptionString(this string htmlString)
        {
            var str = htmlString.RemoveHtmlTagsUsingCharArray();
            if (str.Length < 150)
            {
                return str;
            }
            return (str.Substring(0, 130) + "...");
        }

        public static bool CheckPer(string quyen, int ma)
        {
            var db = new WebDatPhongEntities();
            var item = db.TaiKhoans.FirstOrDefault(x => x.PhanQuyen.DanhSach.Contains("," + quyen + ",") && x.MaTaiKhoan == ma);
            if (item == null)
            {
                return false;
            }
            return true;
        }
    }
}