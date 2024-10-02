using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuaHangDongHo.Areas.Admin.Filters;
using CuaHangDongHo.Defines;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using CuaHangDongHo.Utilities;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        // GET: Admin/Users
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

            List<User> users = db.Users.OrderBy(u => u.Access).ThenBy(u => u.FullName).ToList();

            return View(users);
        }

        // GET: Admin/Users/Details/5
        [AdminFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.USER);
                return RedirectToAction("Index");
            }

            var addressInfo = (from a in db.AddressInfoes
                               join p in db.Provinces on a.ProvinceId equals p.Id
                               join d in db.Districts on a.DistrictId equals d.Id
                               join c in db.Communes on a.CommuneId equals c.Id
                               where a.Id == user.Address
                               select new
                               {
                                   Provinces = p.Name,
                                   Districts = d.Name,
                                   Communes = c.Name,
                                   detail = a.Detail
                               }).FirstOrDefault();
            if (addressInfo != null)
            {
                user.AddressInfo = FormatString.FormatAddress(addressInfo.detail, addressInfo.Communes, addressInfo.Districts, addressInfo.Provinces);
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        [AdminFilter]
        public ActionResult Create()
        {
            User user = new User();
            return View(user);
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Create(User user)
        {
            if (user.Password != user.PasswordAgain)
            {
                ModelState.AddModelError("PasswordAgain", "Vui lòng nhập mật khẩu chính xác!");
            }

            if (ModelState.IsValid)
            {
                AddressInfo addressInfo = new AddressInfo
                {
                    ProvinceId = user.ProvinceId,
                    DistrictId = user.DistricstId,
                    CommuneId = user.CommuneId,
                    Detail = user.DetailAddress
                };

                db.AddressInfoes.Add(addressInfo);
                db.SaveChanges();

                Encryptor encty = new Encryptor();
                user.Password = encty.MD5Hash(user.Password);

                user.Address = addressInfo.Id;
                user.Created_at = DateTime.Now;
                user.Updated_at = DateTime.Now;

                db.Users.Add(user);
                db.SaveChanges();

                TempData[SUCCESS_DATA] = Msg.ADD_DATA_SUCCESS;
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.USER);
                return RedirectToAction("Index");
            }
            user.PasswordAgain = user.Password;

            var addressInfo = (from u in db.Users
                               join a in db.AddressInfoes on u.Address equals a.Id
                               join p in db.Provinces on a.ProvinceId equals p.Id
                               join d in db.Districts on a.DistrictId equals d.Id
                               join c in db.Communes on a.CommuneId equals c.Id
                               where u.Id == user.Id
                               select new
                               {
                                   provinceId = p.Id,
                                   districtId = d.Id,
                                   communeId = c.Id,
                                   detail = a.Detail
                               }).FirstOrDefault();

            if (addressInfo != null)
            {
                user.ProvinceId = addressInfo.provinceId;
                user.DistricstId = addressInfo.districtId;
                user.CommuneId = addressInfo.communeId;
                user.DetailAddress = addressInfo.detail;
            }

            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminFilter]
        public ActionResult Edit(User user)
        {
            User entry = db.Users.Find(user.Id);
            if (entry == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.USER);
                return RedirectToAction("Index");
            }

            if (user.Password != user.PasswordAgain)
            {
                ModelState.AddModelError("PasswordAgain", "Vui lòng nhập mật khẩu chính xác!");
            }

            if (ModelState.IsValid)
            {
                if (user.Password != entry.Password)
                {
                    Encryptor encty = new Encryptor();
                    user.Password = encty.MD5Hash(user.Password);
                }

                user.Created_at = entry.Created_at;
                user.Updated_at = DateTime.Now;

                AddressInfo addressInfo = db.AddressInfoes.Where(a => a.ProvinceId == user.ProvinceId &&
                                                                      a.DistrictId == user.DistricstId &&
                                                                      a.CommuneId == user.CommuneId &&
                                                                      a.Detail == user.DetailAddress).FirstOrDefault();

                try
                {
                    if (addressInfo == null)
                    {
                        AddressInfo addressInfo1 = new AddressInfo
                        {
                            ProvinceId = user.ProvinceId,
                            DistrictId = user.DistricstId,
                            CommuneId = user.CommuneId,
                            Detail = user.DetailAddress
                        };
                        db.AddressInfoes.Add(addressInfo1);
                        db.SaveChanges();

                        user.Address = addressInfo1.Id;
                    }
                    else
                    {
                        user.Address = addressInfo.Id;
                    }

                    // Detach entry
                    ((IObjectContextAdapter)db).ObjectContext.Detach(entry);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData[SUCCESS_DATA] = Msg.EDIT_DATA_SUCCESS;

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        string err = String.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        string[] abc = { };
                        foreach (var ve in eve.ValidationErrors)
                        {
                            string a = String.Format("- Property: \"{0}\", Error: \"{1}\"",
                                 ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
            }
            return View(user);
        }
        // GET: Admin/Users/Delete/5
        [AdminFilter]
        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                TempData[ERR_DATA] = String.Format(Msg.DATA_NOT_EXISTS, Fields.USER);
                return RedirectToAction("Index");
            }

            AddressInfo address = db.AddressInfoes.Find(user.Address);
            if (address != null)
            {
                db.AddressInfoes.Remove(address);
                db.SaveChanges();
            }

            List<Comment> comments = db.Comments.Where(c => c.Created_by == user.Id).ToList();
            foreach (var item in comments)
            {
                db.Comments.Remove(item);
                db.SaveChanges();
            }

            List<Order> orders = db.Orders.Where(o => o.Created_by == user.Id).ToList();
            foreach (var item in orders)
            {
                List<OrderDetail> orderDetails = db.OrderDetails.Where(a => a.OrderId == item.Id).ToList();
                foreach(var i in orderDetails)
                {
                    db.OrderDetails.Remove(i);
                    db.SaveChanges();
                }

                db.Orders.Remove(item);
                db.SaveChanges();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            TempData[SUCCESS_DATA] = Msg.DELETE_DATA_SUCCESS;

            return RedirectToAction("Index");
        }

    }
}
