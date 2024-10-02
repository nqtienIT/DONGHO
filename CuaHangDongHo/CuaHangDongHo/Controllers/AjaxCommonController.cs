using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using CuaHangDongHo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Controllers
{
    public class AjaxCommonController : Controller
    {
        private EntrySetContext db = new EntrySetContext();

        [HttpPost]
        public ActionResult FormatMoneyVND(string money)
        {
            double m = 0;
            try
            {
                m = Convert.ToDouble(money);
            }
            catch (Exception ex)
            {
            }
            string result = FormatString.FormatMoneyVND(m);
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult FormatNumber(string number)
        {
            double m = 0;
            try
            {
                m = Convert.ToDouble(number);
            }
            catch (Exception ex)
            {
            }
            string result = FormatString.FormatNumber(m);
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult AjaxCreateSlugName(string str)
        {
            SlugStr slugStr = new SlugStr();
            str = slugStr.CreateSlug(str, true);
            return Json(new { result = str });
        }

        [HttpPost]
        public ActionResult AjaxGetDistricts(string str)
        {
            var lstDistricts = db.Districts.Where(d => d.ProvinceId == str)
                                            .Select(a => new
                                            {
                                                a.Id,
                                                a.Name
                                            })
                                            .OrderBy(a => a.Name).ToList();

            return Json(lstDistricts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AjaxGetCommunes(string str)
        {
            var lstCommunes = db.Communes.Where(d => d.DistrictId == str)
                                            .Select(a => new
                                            {
                                                a.Id,
                                                a.Name
                                            })
                                            .OrderBy(a => a.Name).ToList();

            return Json(lstCommunes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AjaxGetDashboardAdmin()
        {
            VMDashboard vmDashboard = new VMDashboard();

            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTimePrev = DateTime.Now.AddMonths(-1);

            List<User> lstUsers = db.Users.ToList();
            List<Product> lstProducts = db.Products.ToList();

            List<Order> lstOrders = db.Orders.ToList();
            List<Post> lstPosts = db.Posts.ToList();

            // lay tat ca cac ngay trong thang hien tai
            List<string> lstDayInThisMonth = DateTimeMgrs.GetDaysInMonth(dateTimeNow.Year, dateTimeNow.Month);
            // lay tat ca cac ngay trong thang truoc
            List<string> lstDayInPrevMonth = DateTimeMgrs.GetDaysInMonth(dateTimePrev.Year, dateTimePrev.Month);

            // lay thong tin list user dang ky moi trong thang hien tai
            List<User> lstUsersCreatInThisMonth = lstUsers.Where(a => (a.Created_at.Value.Month == dateTimeNow.Month
                                                                        && a.Created_at.Value.Year == dateTimeNow.Year))
                                                            .ToList();

            // lay thong tin list user dang ky moi trong thang truoc
            List<User> lstUsersCreatInPrevMonth = lstUsers.Where(a => (a.Created_at.Value.Month == dateTimePrev.Month
                                                                        && a.Created_at.Value.Year == dateTimePrev.Year))
                                                            .ToList();

            vmDashboard.NewAccountsInThisMonth = MakeResult(lstUsersCreatInThisMonth, lstDayInThisMonth);
            vmDashboard.NewAccountsInPrevMonth = MakeResult(lstUsersCreatInPrevMonth, lstDayInPrevMonth);
            vmDashboard.RateRegisterAccount = Calculate_Rate(lstUsersCreatInThisMonth.Count(), lstUsersCreatInPrevMonth.Count());
            vmDashboard.CountNewAccount = lstUsersCreatInThisMonth.Count();
            vmDashboard.TotalAccount = lstUsers.Count();

            vmDashboard.TotalProduct = lstProducts.Count();

            vmDashboard.TotalOrders = lstOrders.Count();

            // lay thong tin list post trong thang hien tai
            List<Post> lstPostInMonth = lstPosts.Where(a => (a.Created_at.Value.Month == dateTimeNow.Month
                                                                        && a.Created_at.Value.Year == dateTimeNow.Year))
                                                            .ToList();
            vmDashboard.PostInMonth = MakeResultPost(lstPostInMonth, lstDayInThisMonth);
            vmDashboard.TotalPosts = lstPosts.Count();

            return Json(vmDashboard, JsonRequestBehavior.AllowGet);
        }

        public List<RegisterNewAccount> MakeResult(List<User> lstUser, List<string> lstDayInThisMonth)
        {
            List<RegisterNewAccount> result = lstUser.GroupBy(x => x.Created_at.Value.Date)
                                            .Select(x => new RegisterNewAccount
                                            {
                                                Count = x.Count(),
                                                Day = x.Key.Day < 10 ? "0" + x.Key.Day.ToString() : x.Key.Day.ToString()
                                            }).ToList();
            foreach (var day in lstDayInThisMonth)
            {
                if (result.Where(a => a.Day == day).FirstOrDefault() == null)
                {
                    result.Add(new RegisterNewAccount
                    {
                        Count = 0,
                        Day = day
                    });
                }
            }

            return result.OrderBy(a => a.Day).ToList();
        }

        public List<PostInMonth> MakeResultPost(List<Post> lstPost, List<string> lstDayInThisMonth)
        {
            List<PostInMonth> result = lstPost.GroupBy(x => x.Created_at.Value.Date)
                                            .Select(x => new PostInMonth
                                            {
                                                Count = x.Count(),
                                                Day = x.Key.Day < 10 ? "0" + x.Key.Day.ToString() : x.Key.Day.ToString()
                                            }).ToList();
            foreach (var day in lstDayInThisMonth)
            {
                if (result.Where(a => a.Day == day).FirstOrDefault() == null)
                {
                    result.Add(new PostInMonth
                    {
                        Count = 0,
                        Day = day
                    });
                }
            }

            return result.OrderBy(a => a.Day).ToList();
        }



        public float Calculate_Rate(Int64 numberFisrt, Int64 numberSecond)
        {
            try
            {
                return ((float)((numberFisrt - numberSecond) / numberSecond * 100));
            }
            catch
            {
                return 0;
            }
        }
    }
}