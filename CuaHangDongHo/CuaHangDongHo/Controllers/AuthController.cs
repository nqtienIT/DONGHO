using CuaHangDongHo.Defines;
using CuaHangDongHo.Models;
using CuaHangDongHo.Utilities;
using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Controllers
{
    public class AuthController : BaseController
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url)
                {
                    Query = null,
                    Fragment = null,
                    Path = Url.Action("FacebookCallback")
                };
                return uriBuilder.Uri;
            }
        }

        private Uri RedirectZalo
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url)
                {
                    Query = null,
                    Fragment = null,
                    Path = Url.Action("ZaloCallback")
                };
                return uriBuilder.Uri;
            }
        }

        // GET: Auth
        public ActionResult Login()
        {
            if (TempData[ERR_DATA] != null && !String.IsNullOrEmpty(TempData[ERR_DATA].ToString()))
            {
                ViewBag.ErrMsg = TempData[ERR_DATA].ToString();
            }

            if (TempData[SUCCESS_DATA] != null && !String.IsNullOrEmpty(TempData[SUCCESS_DATA].ToString()))
            {
                ViewBag.SuccessMsg = TempData[SUCCESS_DATA].ToString();
            }

            User user = new User();
            if (Request.Cookies["username"] != null)
            {
                user.UserName = Request.Cookies["username"].Value.ToString();
            }
            if (Request.Cookies["password"] != null)
            {
                user.Password = Request.Cookies["password"].Value.ToString();
            }
            if (Request.Cookies["saveUser"] != null)
            {
                user.SaveLogin = true;
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValidField("UserName") && ModelState.IsValidField("Password"))
            {
                User entry = db.Users.Where(u => u.UserName == user.UserName && u.Access == Enums.RoleType.User).FirstOrDefault();
                if (entry == null)
                {
                    TempData[ERR_DATA] = "User không tồn tại.";
                    return RedirectToAction("Login");
                }

                Encryptor encryptor = new Encryptor();
                string pwdEncrypt = encryptor.MD5Hash(user.Password);

                if (user.Password != entry.Password && pwdEncrypt != entry.Password)
                {
                    TempData[ERR_DATA] = "Password không chính xác.";
                    return RedirectToAction("Login");
                }

                SaveSession(Enums.SessionName.User, entry);

                if (user.SaveLogin)
                {
                    Response.Cookies["username"].Value = entry.UserName;
                    Response.Cookies["password"].Value = entry.Password;
                    Response.Cookies["saveUser"].Value = "1";
                }

                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public ActionResult Register()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (user.Password != user.PasswordAgain)
            {
                ModelState.AddModelError("PasswordAgain", "Vui lòng nhập mật khẩu chính xác!");
            }

            if (ModelState.IsValid)
            {
                // them dia chi
                AddressInfo adressInfo = new AddressInfo
                {
                    ProvinceId = user.ProvinceId,
                    DistrictId = user.DistricstId,
                    CommuneId = user.CommuneId,
                    Detail = user.DetailAddress
                };

                db.AddressInfoes.Add(adressInfo);
                db.SaveChanges();

                Encryptor encty = new Encryptor();
                user.Password = encty.MD5Hash(user.Password);
                user.Address = adressInfo.Id;
                user.Type = Enums.AccountType.System;
                user.Created_at = DateTime.Now;
                user.Updated_at = DateTime.Now;
                user.Status = Enums.StatusAccountType.Active;

                db.Users.Add(user);
                db.SaveChanges();
                TempData[SUCCESS_DATA] = Msg.ADD_DATA_SUCCESS;
                return RedirectToAction("Login");
            }

            return View(user);
        }

        public ActionResult Logout()
        {
            DestroyAllSession();
            return RedirectToAction("Login");
        }

        public ActionResult LoginFb()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (!String.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=name,id,email,birthday,gender,picture");
                string userName = me.id;
                string fullName = me.name;
                string email = me.email;

                User entry = db.Users.Where(u => u.UserName == userName && u.Type == Enums.AccountType.Facebook).FirstOrDefault();
                if (entry == null)
                {
                    Encryptor encry = new Encryptor();
                    User user = new User
                    {
                        FullName = fullName,
                        UserName = userName,
                        Password = encry.MD5Hash(userName),
                        Email = email,
                        Created_at = DateTime.Now,
                        Updated_at = DateTime.Now,
                        Status = Enums.StatusAccountType.Active,
                        Gender = Enums.GenderType.Male,
                        Phone = "0",
                        Access = Enums.RoleType.User,
                        PasswordAgain = "0",
                        Type = Enums.AccountType.Facebook
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    SaveSession(Enums.SessionName.User, user);
                }
                else
                {
                    SaveSession(Enums.SessionName.User, entry);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return null;
            }
        }

        public ActionResult LoginZalo()
        {
            string callback = RedirectZalo.AbsoluteUri;

            return null;
        }
    }
}