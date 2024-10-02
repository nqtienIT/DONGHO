using CuaHangDongHo.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Areas.Admin.Filters
{
    public class AdminFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ctrl = context.Controller as BaseController;
            if (ctrl == null || !ctrl.IsAdmin())
            {
                context.Result = new RedirectResult("/Admin/Auth/Login");
            }
            ctrl.SetOpenLeftMenu();
            ctrl.SetBreadCrumb();
        }
    }
}