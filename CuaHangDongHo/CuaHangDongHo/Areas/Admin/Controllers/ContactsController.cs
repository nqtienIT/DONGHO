using CuaHangDongHo.Areas.Admin.Filters;
using CuaHangDongHo.Defines;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class ContactsController : BaseController
    {
        // danh sach contact
        [AdminFilter]
        public ActionResult Index()
        {
            List<Contact> contacts = db.Contacts.OrderBy(c => c.Status)
                                                .ThenByDescending(c => c.Id).ToList();
            foreach (var item in contacts)
            {
                item.StatusDetail = item.Status.GetEnumDisplayName();
            }

            return View(contacts);
        }

        [AdminFilter]
        public ActionResult Details (int id)
        {
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return RedirectToAction("Index");
            }

            contact.Status = Defines.Enums.StatusContact.Watched;
            db.Entry(contact).State = EntityState.Modified;
            db.SaveChanges();

            return View(contact);
        }

        public ActionResult Delete(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // quan ly thong tin shop
        [AdminFilter]
        public ActionResult Info()
        {
            LoadSession();

            ContactInfo model = new ContactInfo();

            var contact = (from ct in db.ContactInfoes
                           join a in db.AddressInfoes on ct.Address equals a.Id
                           join p in db.Provinces on a.ProvinceId equals p.Id
                           join d in db.Districts on a.DistrictId equals d.Id
                           join c in db.Communes on a.CommuneId equals c.Id
                           select new
                           {
                               ct,
                               province = p.Name,
                               provinceId = p.Id,
                               district = d.Name,
                               districtId = d.Id,
                               commune = c.Name,
                               communeId = c.Id,
                               detail = a.Detail
                           }).FirstOrDefault();

            if (contact != null)
            {
                model.Responsible = contact.ct.Responsible;
                model.Phone = contact.ct.Phone;
                model.Email = contact.ct.Email;
                model.AddressInfo = FormatString.FormatAddress(contact.detail, contact.commune, contact.district, contact.province);
                model.Map = contact.ct.Map;
                model.Facebook = contact.ct.Facebook;
                model.Twitter = contact.ct.Twitter;
                model.Zalo = contact.ct.Zalo;
                model.Fax = contact.ct.Fax;
                model.ProvinceId = contact.provinceId;
                model.DistricstId = contact.districtId;
                model.CommuneId = contact.communeId;
                model.DetailAddress = contact.detail;
            };

            if (LoginUser != null && LoginUser.Access == Enums.RoleType.Admin)
            {
                ViewBag.Role = Enums.RoleType.Admin.ToString();
            }
            else
            {
                ViewBag.Role = Enums.RoleType.Staff.ToString();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ContactInfo model)
        {
            if (ModelState.IsValid)
            {
                List<ContactInfo> contacts = db.ContactInfoes.ToList();
                if (contacts.Count == 0)
                {
                    AddressInfo addressInfo = new AddressInfo
                    {
                        ProvinceId = model.ProvinceId,
                        DistrictId = model.DistricstId,
                        CommuneId = model.CommuneId,
                        Detail = model.DetailAddress
                    };

                    db.AddressInfoes.Add(addressInfo);
                    db.SaveChanges();

                    model.Address = addressInfo.Id;

                    db.ContactInfoes.Add(model);
                    db.SaveChanges();
                    TempData[SUCCESS_DATA] = "Them thanh cong";
                }
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[SUCCESS_DATA] = "Sua thanh cong";
                }
                return RedirectToAction("Info");
            }
            if (LoginUser != null && LoginUser.Access == Enums.RoleType.Admin)
            {
                ViewBag.Role = Enums.RoleType.Admin.ToString();
            }
            else
            {
                ViewBag.Role = Enums.RoleType.Staff.ToString();
            }
            //
            ViewBag.Role = Enums.RoleType.Admin.ToString();
            return View("Info", model);
        }
    }
}