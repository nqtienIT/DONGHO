using CuaHangDongHo.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    [RouteArea("Admin")]
    public class HomeController : BaseController
    {
        // GET: Admin/Home

        [AdminFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}