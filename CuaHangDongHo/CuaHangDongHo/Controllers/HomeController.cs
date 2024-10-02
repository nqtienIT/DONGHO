using CuaHangDongHo.Defines;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.NewProducts = GetNewProducts();
            ViewBag.Sliders = GetSliders();
            ViewBag.MostSoldProducts = GetMostSoldProduct();
            ViewBag.WatchesNormalProducts = GetWatchesProducts(Enums.WatchesType.Normal);
            ViewBag.WatchesMechanicalProducts = GetWatchesProducts(Enums.WatchesType.Mechanical);
            ViewBag.WatchesElectronicProducts = GetWatchesProducts(Enums.WatchesType.Electronic);
            ViewBag.WatchesSmartWatchProducts = GetWatchesProducts(Enums.WatchesType.SmartWatch);
            ViewBag.WatchesSpecialVersionProducts = GetWatchesProducts(Enums.WatchesType.SpecialVersion);
            if(TempData["order_success"] != null && TempData["order_success"].ToString() != null)
            {
                ViewBag.OrderSuccess = TempData["order_success"].ToString();
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(HttpPostedFileBase file)
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public List<Product> GetNewProducts()
        {

            //List<Product> newProducts = db.Products.OrderByDescending(a => a.Id).Take(6).ToList().Select(a => new Product
            //{
            //    Id = a.Id,
            //    Name = a.Name,
            //    Slug = a.Slug,
            //    Detail = a.Detail,
            //    Img = a.Img,
            //    PriceVND = FormatString.FormatMoneyVND(a.Price),
            //    PriceSaleVND = a.PriceSale == null ? String.Empty : FormatString.FormatMoneyVND((double)a.PriceSale)
            //}).ToList();
            List<Product> newProducts = db.Products.Where(p => p.Status == Enums.StatusProductType.Still && p.Number > 0)
                                                    .OrderByDescending(p => p.Created_at).Take(6).ToList();
            newProducts.ForEach(a =>
            {
                a.PriceVND = FormatString.FormatMoneyVND(a.Price);
                if (a.PriceSale != null)
                {
                    a.PriceSaleVND = FormatString.FormatMoneyVND((double)a.PriceSale);
                }
            });
            return newProducts;
        }

        public List<Slider> GetSliders()
        {
            return db.Sliders.OrderByDescending(s => s.Id).ToList();
        }

        public List<Product> GetMostSoldProduct()
        {
            //List<Product> mostSold = db.Products.OrderByDescending(p => p.Sold).Take(6).ToList();
            List<Product> mostSold = (from p in db.Products
                                      join od in db.OrderDetails on p.Id equals od.ProductId
                                      where p.Status == Enums.StatusProductType.Still && p.Number > 0
                                      orderby od.Quantity
                                      select p).Take(6).ToList();

            mostSold.ForEach(a =>
            {
                a.PriceVND = FormatString.FormatMoneyVND(a.Price);
                if (a.PriceSale != null)
                {
                    a.PriceSaleVND = FormatString.FormatMoneyVND((double)a.PriceSale);
                }
            });
            return mostSold;
        }

        public List<Product> GetWatchesProducts(Enums.WatchesType type)
        {
            List<Product> products = db.Products.Where(p => p.Type == type && p.Status == Enums.StatusProductType.Still && p.Number > 0)
                                                .OrderByDescending(p => p.Id).Take(4).ToList();
            products.ForEach(a =>
            {
                a.PriceVND = FormatString.FormatMoneyVND(a.Price);
                if (a.PriceSale != null)
                {
                    a.PriceSaleVND = FormatString.FormatMoneyVND((double)a.PriceSale);
                }
            });
            return products;
        }
    }
}