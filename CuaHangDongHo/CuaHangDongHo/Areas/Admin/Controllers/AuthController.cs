using CuaHangDongHo.Defines;
using CuaHangDongHo.Models;
using CuaHangDongHo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class AuthController : BaseController
    {
        // GET: Admin/Auth
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValidField("UserName") && ModelState.IsValidField("Password"))
            {
                User entry = db.Users.Where(u => u.UserName == user.UserName
                                            && (u.Access == Enums.RoleType.Admin || u.Access == Enums.RoleType.Staff))
                                    .FirstOrDefault();
                if (entry == null)
                {
                    ModelState.AddModelError("UserName", "User không tồn tại.");
                    return View("Login", user);
                }

                Encryptor encryptor = new Encryptor();
                string pwdEncrypt = encryptor.MD5Hash(user.Password);

                if (pwdEncrypt != entry.Password)
                {
                    ModelState.AddModelError("Password", "Password không chính xác.");
                    return View("Login", user);
                }

                SaveSession(entry);
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            DestroySession();
            return RedirectToAction("Login");
        }
    }
}