using CuaHangDongHo.Defines;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Controllers
{
    public class CommentController : BaseController
    {
        [HttpPost]
        public ActionResult PostComment(Comment model)
        {
            if (!String.IsNullOrEmpty(model.Detail))
            {
                model.Type = Enums.CommentType.Post;
                model.Created_at = DateTime.Now;
                model.Status = Enums.StatusComment.Pending;

                db.Comments.Add(model);
                db.SaveChanges();
            }
            return Json(new { result = 0 });
        } 
        
        [HttpPost]
        public ActionResult ProductComment(Comment model)
        {
            if (!String.IsNullOrEmpty(model.Detail))
            {
                model.Type = Enums.CommentType.Product;
                model.Created_at = DateTime.Now;
                model.Status = Enums.StatusComment.Pending;

                db.Comments.Add(model);
                db.SaveChanges();
            }
            return Json(new { result = 0 });
        }

    }
}