using CuaHangDongHo.Defines;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CuaHangDongHo.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
        public ActionResult Index()
        {
            if (TempData[SUCCESS_DATA] != null && !String.IsNullOrEmpty(TempData[SUCCESS_DATA].ToString()))
            {
                ViewBag.SuccessMsg = TempData[SUCCESS_DATA].ToString();
            }

            VMContact model = new VMContact();
            if (LoginUser != null && LoginUser.Id > 0)
            {
                model.FullNameContact = LoginUser.FullName;
                model.PhoneContact = LoginUser.Phone;
                model.EmailContact = LoginUser.Email;
            }

            var contact = (from ct in db.ContactInfoes
                           join a in db.AddressInfoes on ct.Address equals a.Id
                           join p in db.Provinces on a.ProvinceId equals p.Id
                           join d in db.Districts on a.DistrictId equals d.Id
                           join c in db.Communes on a.CommuneId equals c.Id
                           select new
                           {
                               ct,
                               province = p.Name,
                               district = d.Name,
                               commune = c.Name,
                               detail = a.Detail
                           }).FirstOrDefault();
            model.ResponsibleShop = contact.ct.Responsible;
            model.PhoneShop = contact.ct.Phone;
            model.EmailShop = contact.ct.Email;
            model.AddressShop = FormatString.FormatAddress(contact.detail, contact.commune, contact.district, contact.province);
            model.MapShop = contact.ct.Map;
            model.FacebookShop = contact.ct.Facebook;
            model.TwitterShop = contact.ct.Twitter;
            model.ZaloShop = contact.ct.Zalo;
            model.FaxShop = contact.ct.Fax;

            return View(model);
        }

        public ActionResult Create(VMContact model)
        {
            if (ModelState.IsValid)
            {
                Contact contact = new Contact
                {
                    FullName = model.FullNameContact,
                    Phone = model.PhoneContact,
                    Email = model.EmailContact,
                    Title = model.TitleContact,
                    Detail = model.DetailContact,
                    Created_at = DateTime.Now,
                    Status = Enums.StatusContact.Pending
                };
                db.Contacts.Add(contact);
                db.SaveChanges();
                TempData[SUCCESS_DATA] = "Them thanh cong";
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }
    }
}