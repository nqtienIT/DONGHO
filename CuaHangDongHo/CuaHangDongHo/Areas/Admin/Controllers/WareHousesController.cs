using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuaHangDongHo.Areas.Admin.Filters;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class WareHousesController : BaseController
    {
        // GET: Admin/WareHouses
        [AdminFilter]
        public ActionResult Index()
        {
            List<WareHouse> lstWareHouses = db.WareHouses.OrderByDescending(a => a.Id).ToList();
            lstWareHouses.ForEach(a =>
            {
                a.ProductName = db.Products.Find(a.ProductId).Name;
                a.SupplierName = a.SupplierId == null ? String.Empty : db.Suppliers.Find(a.SupplierId).Name;
                a.TypeName = a.Type.GetEnumDisplayName();
            });
            return View(lstWareHouses);
        }

        // GET: Admin/WareHouses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WareHouse wareHouse = db.WareHouses.Find(id);
            if (wareHouse == null)
            {
                return HttpNotFound();
            }
            return View(wareHouse);
        }

        // GET: Admin/WareHouses/Create
        [AdminFilter]
        public ActionResult Create()
        {
            WareHouse wareHouse = new WareHouse();
            return View(wareHouse);
        }

        // POST: Admin/WareHouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Create(WareHouse wareHouse)
        {
            if (wareHouse.QuantityChange < 0)
            {
                ModelState.AddModelError("QuantityChange", "Không thể nhập số nhỏ hơn 0");
            }

            Product product = db.Products.Find(wareHouse.ProductId);
            // nhap them san pham
            if (wareHouse.Type == (int)WareHoueType.Import)
            {
                // kiem tra nhap hang tu nha cung cap nao
                if (wareHouse.SupplierId == null)
                {
                    ModelState.AddModelError("SupplierId", "Vui lòng chọn nhà cung cấp");
                }
                else
                {
                    product.Number += wareHouse.QuantityChange;
                }
            }
            else // xuat san pham
            {
                // xuat nhieu hon trong kho hien co
                if (wareHouse.QuantityChange > product.Number)
                {
                    ModelState.AddModelError("QuantityChange", "Số lượng không đủ để xuất");
                }
                else
                {
                    wareHouse.SupplierId = null;
                    product.Number -= wareHouse.QuantityChange;
                }
            }

            if (ModelState.IsValid)
            {

                db.Entry(product).State = EntityState.Modified;

                wareHouse.Created_at = DateTime.Now;
                wareHouse.Updated_at = DateTime.Now;

                db.WareHouses.Add(wareHouse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wareHouse);
        }

        // GET: Admin/WareHouses/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WareHouse wareHouse = db.WareHouses.Find(id);
            if (wareHouse == null)
            {
                return HttpNotFound();
            }
            return View(wareHouse);
        }

        // POST: Admin/WareHouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Edit(WareHouse wareHouse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wareHouse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wareHouse);
        }

        // GET: Admin/WareHouses/Delete/5
        [AdminFilter]
        public ActionResult Delete(int id)
        {
            WareHouse wareHouse = db.WareHouses.Find(id);
            db.WareHouses.Remove(wareHouse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
