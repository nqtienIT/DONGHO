using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuaHangDongHo.Areas.Admin.Filters;
using CuaHangDongHo.Models;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class PostsController : BaseController
    {
        // GET: Admin/Posts
        [AdminFilter]
        public ActionResult Index()
        {
            if (TempData[ERR_DATA] != null && !String.IsNullOrEmpty(TempData[ERR_DATA].ToString()))
            {
                ViewBag.ErrMsg = TempData[ERR_DATA].ToString();
            }

            if (TempData[SUCCESS_DATA] != null && !String.IsNullOrEmpty(TempData[SUCCESS_DATA].ToString()))
            {
                ViewBag.SuccessMsg = TempData[SUCCESS_DATA].ToString();
            }
            return View(db.Posts.ToList());
        }

        // GET: Admin/Posts/Details/5
        [AdminFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Admin/Posts/Create
        [AdminFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AdminFilter]
        public ActionResult Create(HttpPostedFileBase Img, Post post)
        {
            if (Img == null)
            {
                ModelState.AddModelError("img", "Vui lòng chọn ảnh");
            }

            if (ModelState.IsValid)
            {
                // upload image
                if (Img != null)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(Img.FileName);
                    string extension = System.IO.Path.GetExtension(Img.FileName);
                    fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/posts/"), fileName);
                    Img.SaveAs(path);
                    post.Img = fileName;
                }

                post.Created_by = LoginUser.Id;
                post.Created_at = DateTime.Now;
                post.Updated_at = DateTime.Now;

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [AdminFilter]
        public ActionResult Edit(HttpPostedFileBase Img, Post post)
        {
            Post entry = db.Posts.Find(post.Id);

            if (Img == null)
            {
                ModelState.AddModelError("img", "Vui lòng chọn ảnh");
            }

            if (ModelState.IsValid)
            {
                // upload image
                if (Img != null)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(Img.FileName);
                    string extension = System.IO.Path.GetExtension(Img.FileName);
                    fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/posts/"), fileName);
                    Img.SaveAs(path);
                    post.Img = fileName;
                }

                post.Created_by = LoginUser.Id;
                post.Created_at = entry.Created_at;
                post.Updated_at = DateTime.Now;

                ((IObjectContextAdapter)db).ObjectContext.Detach(entry);
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        public ActionResult Delete(int id)
        {
            List<Comment> comments = db.Comments.Where(c => c.PostId == id).ToList();
            db.Comments.RemoveRange(comments);

            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
