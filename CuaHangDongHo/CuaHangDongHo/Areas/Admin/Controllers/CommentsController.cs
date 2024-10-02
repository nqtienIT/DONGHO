using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuaHangDongHo.Areas.Admin.Filters;
using CuaHangDongHo.Models;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Defines;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class CommentsController : BaseController
    {
        [AdminFilter]
        public ActionResult Post()
        {
            var data = (from c in db.Comments
                        join u in db.Users on c.Created_by equals u.Id
                        join p in db.Posts on c.PostId equals p.Id
                        where c.Type == Enums.CommentType.Post
                        select new
                        {
                            cmtId = c.Id,
                            userId = u.Id,
                            userName = u.FullName,
                            postId = p.Id,
                            postTitle = p.Title,
                            detail = c.Detail,
                            created_at = c.Created_at,
                            status = c.Status
                        }).OrderBy(c => c.status).ThenByDescending(c => c.cmtId).ToList();

            List<Comment> comments = new List<Comment>();

            foreach (var item in data)
            {
                Comment comment = new Comment
                {
                    Id = item.cmtId,
                    UserComment = new UserComment
                    {
                        UserName = item.userName,
                        UserUrl = Url.Action("Details", "Users", new { area = "Admin", Id = item.userId })
                    },
                    PostComment = new PostComment
                    {
                        PostTitle = item.postTitle,
                        PostUrl = Url.Action("Detail", "Post", new { Area = "", Id = item.postId })
                    },
                    Detail = item.detail,
                    Created_at = item.created_at,
                    Status = item.status,
                    StatusDetail = item.status.GetEnumDisplayName()
                };
                comments.Add(comment);
            }

            return View(comments);
        }

        [AdminFilter]
        public ActionResult Product()
        {
            var data = (from c in db.Comments
                        join u in db.Users on c.Created_by equals u.Id
                        join p in db.Products on c.ProductId equals p.Id
                        where c.Type == Enums.CommentType.Product
                        select new
                        {
                            cmtId = c.Id,
                            userId = u.Id,
                            userName = u.FullName,
                            productId = p.Id,
                            productName = p.Name,
                            productSlug = p.Slug,
                            detail = c.Detail,
                            created_at = c.Created_at,
                            status = c.Status
                        }).OrderBy(c => c.status).ThenByDescending(c => c.cmtId).ToList();

            List<Comment> comments = new List<Comment>();

            foreach (var item in data)
            {
                Comment comment = new Comment
                {
                    Id = item.cmtId,
                    UserComment = new UserComment
                    {
                        UserName = item.userName,
                        UserUrl = Url.Action("Details", "Users", new { area = "Admin", Id = item.userId })
                    },
                    ProductComment = new ProductComment
                    {
                        ProductName = item.productName,
                        ProductUrl = Url.Action("Details", "Product", new { area = "", Id = String.Format("{0}-p{1}", item.productSlug, item.productId) })
                    },
                    Detail = item.detail,
                    Created_at = item.created_at,
                    Status = item.status,
                    StatusDetail = item.status.GetEnumDisplayName()
                };
                comments.Add(comment);
            }

            return View(comments);
        }

        public ActionResult Update(int CommentId, Enums.CommentType type)
        {
            Comment comment = db.Comments.Find(CommentId);
            if (comment != null)
            {
                comment.Status = Enums.StatusComment.Accept;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (type == Enums.CommentType.Post)
            {
                return RedirectToAction("Post");
            }
            else if(type == Enums.CommentType.Product)
            {
                return RedirectToAction("Product");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Admin/Comments/Delete/5
        public ActionResult Delete(int CommentId, Enums.CommentType type)
        {
            Comment comment = db.Comments.Find(CommentId);
            if (comment == null)
            {
                return HttpNotFound();
            }
            db.Comments.Remove(comment);
            db.SaveChanges();

            if (type == Enums.CommentType.Post)
            {
                return RedirectToAction("Post");
            }
            else if (type == Enums.CommentType.Product)
            {
                return RedirectToAction("Product");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
