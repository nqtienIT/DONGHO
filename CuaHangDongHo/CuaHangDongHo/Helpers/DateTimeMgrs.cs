using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Helpers
{
    public class DateTimeMgrs
    {
        public static List<DateTime> GetDatesInMonth(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
        }

        public static List<string> GetDaysInMonth(int year, int month)
        {
            List<string> lstDate = new List<string>();
            List<DateTime> datesInMonth = GetDatesInMonth(year, month);

            foreach (var date in datesInMonth)
            {
                string day = date.Day.ToString();
                if(date.Day < 10)
                {
                    // them so 0 neu nho hon 10
                    day = "0" + day;
                }
                lstDate.Add(day);
            }
            return lstDate;
        }

        public static string FormatDateTimeVNese(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}