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
    public class SlidersController : BaseController
    {
        // GET: Admin/Sliders
        [AdminFilter]
        public ActionResult Index()
        {
            List<Slider> Slider = db.Sliders.OrderByDescending(s => s.Id).ToList();

            return View(Slider);
        }

        // GET: Admin/Sliders/Details/5
        [AdminFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Admin/Sliders/Create
        [AdminFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Create(Slider slider, HttpPostedFileBase Img, HttpPostedFileBase SmallImg)
        {
            if(Img == null)
            {
                ModelState.AddModelError("Img", "Vui lòng chọn ảnh");
            }
            if (ModelState.IsValid)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(Img.FileName);
                string extension = System.IO.Path.GetExtension(Img.FileName);
                fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/sliders/"), fileName);
                Img.SaveAs(path);

                slider.Img = fileName;

                if(SmallImg != null)
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(SmallImg.FileName);
                    extension = System.IO.Path.GetExtension(SmallImg.FileName);
                    fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                    path = System.IO.Path.Combine(Server.MapPath("~/Content/img/sliders/"), fileName);
                    SmallImg.SaveAs(path);
                    slider.SmallImg = fileName;
                }

                slider.Created_at = DateTime.Now;
                slider.Updated_at = DateTime.Now;

                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Admin/Sliders/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Admin/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Edit(Slider slider, HttpPostedFileBase Img, HttpPostedFileBase SmallImg)
        {
            Slider entry = db.Sliders.Find(slider.Id);

            if (Img == null)
            {
                ModelState.AddModelError("Img", "Vui lòng chọn ảnh");
            }
            if (ModelState.IsValid)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(Img.FileName);
                string extension = System.IO.Path.GetExtension(Img.FileName);
                fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/sliders/"), fileName);
                Img.SaveAs(path);

                slider.Img = fileName;

                if (SmallImg != null)
                {
                    fileName = System.IO.Path.GetFileNameWithoutExtension(SmallImg.FileName);
                    extension = System.IO.Path.GetExtension(SmallImg.FileName);
                    fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                    path = System.IO.Path.Combine(Server.MapPath("~/Content/img/sliders/"), fileName);
                    SmallImg.SaveAs(path);
                    slider.SmallImg = fileName;
                }

                slider.Created_at = entry.Created_at;
                slider.Updated_at = DateTime.Now;

                ((IObjectContextAdapter)db).ObjectContext.Detach(entry);
                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Admin/Sliders/Delete/5
        public ActionResult Delete(int id)
        {
            Slider slider = db.Sliders.Find(id);
            db.Sliders.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
