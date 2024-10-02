using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using CuaHangDongHo.Defines;

namespace CuaHangDongHo.Controllers
{
    public class SearchController : BaseController
    {
        // GET: Search
        public ActionResult Index(string q, int page = 1)
        {
            var data = (from p in db.Products
                        orderby p.Id descending
                        where p.Name.Contains(q) && p.Status == Enums.StatusProductType.Still && p.Number > 0
                        select p).ToList();
            ViewBag.CountProducts = data.Count();
            var model = data.ToPagedList(page, 12);

            foreach (var item in model)
            {
                item.PriceVND = FormatString.FormatMoneyVND(item.Price);
                if (item.PriceSale != null)
                {
                    item.PriceSaleVND = FormatString.FormatMoneyVND((double)item.PriceSale);
                }
            }
            ViewBag.TextSearch = q;
            ViewBag.Cate = String.Format("Tìm kiếm: \"{0}\"", q);
            ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();

            return View(model);
        }

        public ActionResult All(int page = 1)
        {
            var data = (from p in db.Products
                        orderby p.Id descending
                        where p.Status == Enums.StatusProductType.Still && p.Number > 0
                        select p).ToList();
            ViewBag.CountProducts = data.Count();
            var model = data.ToPagedList(page, 12);

            foreach (var item in model)
            {
                item.PriceVND = FormatString.FormatMoneyVND(item.Price);
                if (item.PriceSale != null)
                {
                    item.PriceSaleVND = FormatString.FormatMoneyVND((double)item.PriceSale);
                }
            }
            ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();

            return View("Index", model);
        }

        public ActionResult Category(int id, int page = 1)
        {
            var data = (from p in db.Products
                        orderby p.Id descending
                        where p.CateId == id && p.Status == Enums.StatusProductType.Still && p.Number > 0
                        select p).ToList();

            ViewBag.CountProducts = data.Count();
            var model = data.ToPagedList(page, 12);

            foreach (var item in model)
            {
                item.PriceVND = FormatString.FormatMoneyVND(item.Price);
                if (item.PriceSale != null)
                {
                    item.PriceSaleVND = FormatString.FormatMoneyVND((double)item.PriceSale);
                }
            }

            ViewBag.Cate = db.Categories.Find(id).Name;
            ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();

            return View("Index", model);
        }

        public ActionResult Brands(int id, int page = 1)
        {
            var data = (from p in db.Products
                        join b in db.Brands on p.BrandId equals b.Id
                        orderby p.Id descending
                        where b.Id == id && p.Status == Enums.StatusProductType.Still && p.Number > 0
                        select p).ToList();
            ViewBag.CountProducts = data.Count();
            var model = data.ToPagedList(page, 12);

            foreach (var item in model)
            {
                item.PriceVND = FormatString.FormatMoneyVND(item.Price);
                if (item.PriceSale != null)
                {
                    item.PriceSaleVND = FormatString.FormatMoneyVND((double)item.PriceSale);
                }
            }

            Brand brand = db.Brands.Find(id);
            ViewBag.Cate = brand.Name;
            ViewBag.Description = brand.Description;
            ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();

            return View("Index", model);
        }

        public ActionResult WatchesType(int id, int page = 1)
        {
            bool exist = Enum.IsDefined(typeof(Enums.WatchesType), id);
            if (exist)
            {
                var data = (from p in db.Products
                            orderby p.Id descending
                            where p.Type == (Enums.WatchesType)id && p.Status == Enums.StatusProductType.Still && p.Number > 0
                            select p).ToList();

                ViewBag.CountProducts = data.Count();
                var model = data.ToPagedList(page, 12);

                ViewBag.Cate = ((Enums.WatchesType)id).GetEnumDisplayName();
                ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                return View("Index", model);
            }
            return View("Index", null);
        }

        public ActionResult WatchesStrap(int id, int page = 1)
        {
            bool exist = Enum.IsDefined(typeof(Enums.WatchesStrap), id);
            if (exist)
            {
                var data = (from p in db.Products
                            orderby p.Id descending
                            where p.Strap == (Enums.WatchesStrap)id && p.Status == Enums.StatusProductType.Still && p.Number > 0
                            select p).ToList();
                ViewBag.CountProducts = data.Count();
                var model = data.ToPagedList(page, 12);

                ViewBag.Cate = ((Enums.WatchesStrap)id).GetEnumDisplayName();
                ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
                return View("Index", model);
            }
            return View("Index", null);
        }

        public ActionResult PriceFilter(string price_filter, int page = 1)
        {
            int priceMin;
            int priceMax;
            try
            {
                string[] priceFilter = price_filter.Split(',');
                priceMin = Convert.ToInt32(priceFilter[0]);
                priceMax = Convert.ToInt32(priceFilter[1]);
            }
            catch
            {
                priceMin = 0;
                priceMax = 0;
            }

            var data = (from p in db.Products
                        orderby p.Id descending
                        where ((p.Price >= priceMin && p.Price <= priceMax)
                        || (p.PriceSale >= priceMin && p.PriceSale <= priceMax)) && p.Status == Enums.StatusProductType.Still && p.Number > 0
                        select p).ToList();

            ViewBag.CountProducts = data.Count();
            var model = data.ToPagedList(page, 12);

            ViewBag.ActionName = this.ControllerContext.RouteData.Values["action"].ToString();
            ViewBag.PriceFilter = price_filter;
            return View("Index", model);
        }
    }
}