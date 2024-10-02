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

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class BrandsController : BaseController
    {
        // GET: Admin/Brands
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

            List<Brand> brands = GetBrands();

            return View(brands);
        }

        // GET: Admin/Brands/Details/5
        [AdminFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Admin/Brands/Create
        [AdminFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AdminFilter]
        public ActionResult Create(HttpPostedFileBase img, Brand brand)
        {
            if (img == null)
            {
                ModelState.AddModelError("img", "Vui lòng chọn ảnh");
            }

            if (ModelState.IsValid)
            {
                // upload image
                if (img != null)
                {
                    string fileName = String.Format("{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(img.FileName),
                                                                DateTime.Now.ToString("MMddyyyyHHmmss"));

                    string extension = System.IO.Path.GetExtension(img.FileName);

                    fileName += extension;

                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/brands/"), fileName);
                    img.SaveAs(path);
                    brand.Img = fileName;
                }

                brand.Created_at = DateTime.Now;
                brand.Updated_at = DateTime.Now;

                db.Brands.Add(brand);
                db.SaveChanges();

                TempData[SUCCESS_DATA] = Msg.ADD_DATA_SUCCESS;

                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Admin/Brands/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Brand entry = db.Brands.Find(id);
            if (entry == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.BRAND);
                return RedirectToAction("Index");
            }

            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AdminFilter]
        public ActionResult Edit(HttpPostedFileBase NewImg, Brand brand)
        {

            Brand entry = db.Brands.Find(brand.Id);
            if (entry == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.BRAND);
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                // upload image
                if (NewImg != null)
                {
                    // lay ten file
                    string fileName = String.Format("{0}_{1}", System.IO.Path.GetFileNameWithoutExtension(NewImg.FileName),
                                                                DateTime.Now.ToString("MMddyyyyHHmmss"));
                    string extension = System.IO.Path.GetExtension(NewImg.FileName);

                    fileName += extension;

                    string pathFile = Server.MapPath("~/Content/img/brands/");
                    // path luu file
                    string path = System.IO.Path.Combine(pathFile, fileName);
                    NewImg.SaveAs(path);

                    // xoa file cu
                    string oldFileName = System.IO.Path.Combine(pathFile, entry.Img);
                    if (System.IO.File.Exists(oldFileName))
                    {
                        System.IO.File.Delete(oldFileName);
                    }

                    brand.Img = fileName;
                }

                brand.Updated_at = DateTime.Now;

                ((IObjectContextAdapter)db).ObjectContext.Detach(entry);
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Admin/Brands/Delete/5
        [AdminFilter]
        public ActionResult Delete(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public List<Brand> GetBrands()
        {
            List<Brand> result = db.Brands.ToList();
            return result;
        }
    }
}
