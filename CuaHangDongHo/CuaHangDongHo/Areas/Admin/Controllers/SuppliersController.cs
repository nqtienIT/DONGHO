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
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class SuppliersController : BaseController
    {
        // GET: Admin/Suppliers
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

            List<Supplier> lstSuppliers = GetListSuppliers();

            return View(lstSuppliers);
        }

        // GET: Admin/Suppliers/Details/5
        [AdminFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Admin/Suppliers/Create
        [AdminFilter]
        public ActionResult Create()
        {
            Supplier supplier = new Supplier();
            return View(supplier);
        }

        // POST: Admin/Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                // them dia chi
                AddressInfo adressInfo = new AddressInfo
                {
                    ProvinceId = supplier.ProvinceId,
                    DistrictId = supplier.DistricstId,
                    CommuneId = supplier.CommuneId,
                    Detail = supplier.DetailAddress
                };

                db.AddressInfoes.Add(adressInfo);
                db.SaveChanges();

                supplier.Adress = adressInfo.Id;

                supplier.Created_at = DateTime.Now;
                supplier.Updated_at = DateTime.Now;

                db.Suppliers.Add(supplier);
                db.SaveChanges();

                TempData[SUCCESS_DATA] = Msg.ADD_DATA_SUCCESS;
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: Admin/Suppliers/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.SUPPLIER);
                return RedirectToAction("Index");
            }

            AddressInfo addressInfo = db.AddressInfoes.Find(supplier.Adress);
            if (addressInfo != null)
            {
                supplier.ProvinceId = addressInfo.ProvinceId;
                supplier.DistricstId = addressInfo.DistrictId;
                supplier.CommuneId = addressInfo.CommuneId;
                supplier.DetailAddress = addressInfo.Detail;
            }

            return View(supplier);
        }

        // POST: Admin/Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Edit(Supplier supplier)
        {
            Supplier entry = db.Suppliers.Find(supplier.Id);
            if (entry == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.SUPPLIER);
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                // update address
                AddressInfo addressInfo = db.AddressInfoes.Where(a => a.ProvinceId == supplier.ProvinceId)
                                                            .Where(a => a.DistrictId == supplier.DistricstId)
                                                            .Where(a => a.CommuneId == supplier.CommuneId)
                                                            .Where(a => a.Detail == supplier.DetailAddress)
                                                            .FirstOrDefault();
                if (addressInfo == null)
                {
                    AddressInfo addAddressInfo = new AddressInfo
                    {
                        ProvinceId = supplier.ProvinceId,
                        DistrictId = supplier.DistricstId,
                        CommuneId = supplier.CommuneId,
                        Detail = supplier.DetailAddress
                    };
                    db.AddressInfoes.Add(addAddressInfo);
                    db.SaveChanges();
                    supplier.Adress = addAddressInfo.Id;
                }
                else
                {
                    supplier.Adress = addressInfo.Id;
                }

                supplier.Updated_at = DateTime.Now;
                ((IObjectContextAdapter)db).ObjectContext.Detach(entry);
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Admin/Suppliers/Delete/5
        [AdminFilter]
        public ActionResult Delete(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<Supplier> GetListSuppliers()
        {
            List<Supplier> result = new List<Supplier>();
            var lstSuppliers = (from s in db.Suppliers
                                join a in db.AddressInfoes on s.Adress equals a.Id
                                join c in db.Communes on a.CommuneId equals c.Id
                                join d in db.Districts on a.DistrictId equals d.Id
                                join p in db.Provinces on a.ProvinceId equals p.Id
                                select new
                                {
                                    s.Id,
                                    s.Name,
                                    s.Phone,
                                    s.Description,
                                    detailAddress = a.Detail,
                                    communes = c.Name,
                                    district = d.Name,
                                    province = p.Name
                                }).ToList();

            foreach (var item in lstSuppliers)
            {
                Supplier REF = new Supplier
                {
                    Id = item.Id,
                    Name = item.Name,
                    Phone = item.Phone,
                    Description = item.Description,
                    DetailAddress = FormatString.FormatAddress(item.detailAddress, item.communes, item.district, item.province)
                };
                result.Add(REF);
            }

            return result;
        }

    }
}
