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
using CuaHangDongHo.Defines;
using CuaHangDongHo.Models;
using CuaHangDongHo.Utilities;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        // GET: Admin/Categories
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

            List<Category> categories = GetCategories();
            return View(categories);
        }

        // GET: Admin/Categories/Details/5
        [AdminFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            category.ParentStr = category.ParentId == 0 ? String.Empty : db.Categories.Find(category.ParentId)?.Name ?? String.Empty;

            return View(category);
        }

        // GET: Admin/Categories/Create
        [AdminFilter]
        public ActionResult Create()
        {
            //ViewBag.lstCateParent = GetCategoriesParent();

            Category category = new Category();

            return View(category);
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Create(Category category)
        {
            Category entry = db.Categories.Where(a => a.Name == category.Name).FirstOrDefault();
            if (entry != null)
            {
                ModelState.AddModelError("Name", String.Format(Msg.DATA_IS_DUPPLICATE, Fields.CATEGORY));
            }
            if (ModelState.IsValid)
            {
                category.Created_at = DateTime.Now;
                category.Updated_at = DateTime.Now;

                db.Categories.Add(category);
                db.SaveChanges();
                TempData[SUCCESS_DATA] = Msg.ADD_DATA_SUCCESS;
                return RedirectToAction("Index");
            }
            //category.lstParentItem = GetCategoriesParent();
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category test = db.Categories.Where(a => a.Id == id).FirstOrDefault();
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            //ViewBag.lstCateParent = GetCategoriesParent();
            //ViewBag.ParentId = category.ParentId;

            //category.lstParentItem = GetCategoriesParent();

            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Edit(Category category)
        {
            // check tồn tại của cate
            Category entry = db.Categories.Find(category.Id);
            if(entry == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.CATEGORY);
                return RedirectToAction("Index");
            }

            // check unique name cate
            if(category.Name != entry.Name)
            {
                Category cate = db.Categories.Where(c => c.Name == category.Name).FirstOrDefault();
                if(cate != null)
                {
                    ModelState.AddModelError("Name", String.Format(Msg.DATA_IS_DUPPLICATE, Fields.CATEGORY));
                }
            }

            if (ModelState.IsValid)
            {
                category.Updated_at = DateTime.Now;

                ((IObjectContextAdapter)db).ObjectContext.Detach(entry);
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(category);
        //}

        // GET: Admin/Categories/Delete/5
        [AdminFilter]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData[SUCCESS_DATA] = Msg.DELETE_DATA_SUCCESS;
            return RedirectToAction("Index");
        }

        public List<Category> GetCategories()
        {
            var categories = db.Categories.ToList();
            List<Category> result = categories.Select(m => new Category
            {
                Id = m.Id,
                Name = m.Name,
                ParentStr = m.ParentId == 0 ? String.Empty : db.Categories.Find(m.ParentId)?.Name ?? String.Empty
            }).ToList();
            return result;
        }

        [HttpPost]
        public ActionResult AjaxCreateSlugName(string str)
        {
            SlugStr slugStr = new SlugStr();
            str = slugStr.CreateSlug(str, true);
            return Json(new { result = str });
        }

        public List<SelectListItem> GetCategoriesParent()
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem { Value = "0", Text = "--- Danh mục cha ---" });

            List<Category> lstCateParent = db.Categories.Where(c => c.ParentId == 0).ToList();
            foreach (Category cate in lstCateParent)
            {
                result.Add(new SelectListItem
                {
                    Value = cate.Id.ToString(),
                    Text = cate.Name
                });
            }

            return result;
        }
    }
}
