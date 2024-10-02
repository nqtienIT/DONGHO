using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Helpers
{
    public class FormatString
    {
        public static string FormatMoneyVND(double money)
        {
            if (money <= 0)
            {
                return "0VND";
            }
            return string.Format("{0:#,###.## VND}", money);
        }

        public static string FormatNumber(double number)
        {
            if (number <= 0)
            {
                return "0";
            }
            return string.Format("{0:#,###.##}", number);
        }

        public static string FormatAddress(string detail, string communes, string district, string province)
        {
            return string.Format("{0}, {1}, {2}, {3}", detail, communes, district, province);
        }
    }
}