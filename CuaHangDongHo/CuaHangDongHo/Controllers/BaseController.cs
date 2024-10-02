using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CuaHangDongHo.Defines.Enums;
using CuaHangDongHo.Helpers;

namespace CuaHangDongHo.Controllers
{
    public class BaseController : Controller
    {
        protected EntrySetContext db = new EntrySetContext();

        protected const string ERR_DATA = "err_data";
        protected const string SUCCESS_DATA = "success_data";

        protected User LoginUser { get; private set; } = null;

        protected VMCartItem VMCartItem { get; set; } = null;

        public BaseController()
        {


            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Brands = db.Brands.Take(10).ToList()
                                        .Select(b => new Brand
                                        {
                                            Id = b.Id,
                                            Name = b.Name,
                                            ProductCount = db.Products.Where(p => p.BrandId == b.Id).Count()
                                        }).ToList();

            LoadSessionCart();
            if (VMCartItem.CartItems != null && VMCartItem.CartItems.Count > 0)
            {
                ViewBag.NavNumProduct = VMCartItem.CartItems.GroupBy(a => a.ProductId).Count();
            }
            else
            {
                ViewBag.NavNumProduct = 0;
            }

            LoadSessionUser();
            if (LoginUser != null && LoginUser.Id > 0)
            {
                // da dang nhap
                ViewBag.NavUser = LoginUser;
            }
            else
            {
                // chua dang nhap
                ViewBag.NavUser = null;
            }
        }

        public void LoadSessionUser()
        {
            if (LoginUser == null)
            {
                LoginUser = new User();
            }
            LoginUser = (User)LoadSession(SessionName.User);
        }

        public void LoadSessionCart()
        {
            VMCartItem = (VMCartItem)LoadSession(SessionName.Cart);

            if (VMCartItem == null)
            {
                VMCartItem = new VMCartItem();
            }
        }


        protected void SaveSession(SessionName sessionName, object obj)
        {
            System.Web.HttpContext.Current.Session[sessionName.ToString()] = obj;
        }

        protected object LoadSession(SessionName sessionName)
        {
            return System.Web.HttpContext.Current.Session[sessionName.ToString()];
        }

        protected void DestroyAllSession()
        {
            System.Web.HttpContext.Current.Session.Clear();
        }

        protected void RemoveSession(SessionName sessionName)
        {
            System.Web.HttpContext.Current.Session.Remove(sessionName.ToString());
        }
    }
}