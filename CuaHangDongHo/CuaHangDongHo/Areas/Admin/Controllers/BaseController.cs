using CuaHangDongHo.Defines;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected EntrySetContext db = new EntrySetContext();

        protected const string ERR_DATA = "err_data";
        protected const string SUCCESS_DATA = "success_data";

        protected User LoginUser { get; private set; } = null;

        public bool IsAdmin()
        {
            LoadSession();
            bool isExist = false;

            try
            {
                if (LoginUser == null)
                {
                    return isExist;
                }

                if (LoginUser.Id < 1 || LoginUser.Access != (int)RoleType.Admin)
                {
                    return isExist;
                }

                ViewBag.LoginUser = LoginUser;

                isExist = true;
                return isExist;
            }
            catch (Exception ex)
            {
                isExist = false;
                return isExist;
            }
            finally
            {
                if (!isExist)
                {
                    DestroySession();
                }
            }
        }

        public void SetOpenLeftMenu()
        {
            string controllerName = ControllerContext.RouteData.Values["controller"].ToString();
            string actionName = ControllerContext.RouteData.Values["action"].ToString();

            int parentOpen = 1;
            int viewOpen = 0;

            switch (controllerName)
            {
                case nameof(ControllerName.Home):
                    parentOpen = (int)ControllerName.Home;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.HomeIndex;
                    }
                    break;

                case nameof(ControllerName.Users):
                    parentOpen = (int)ControllerName.Users;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.UserIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.UserCreate;
                    }
                    break;

                case nameof(ControllerName.Brands):
                    parentOpen = (int)ControllerName.Brands;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.BrandIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.BrandCreate;
                    }
                    break;

                case nameof(ControllerName.Categories):
                    parentOpen = (int)ControllerName.Categories;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.CategoryIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.CategoryCreate;
                    }
                    break;

                case nameof(ControllerName.Products):
                    parentOpen = (int)ControllerName.Products;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.ProductIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.ProductCreate;
                    }
                    break;

                case nameof(ControllerName.Suppliers):
                    parentOpen = (int)ControllerName.Suppliers;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.SupplierIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.SupplierCreate;
                    }
                    break;

                case nameof(ControllerName.WareHouses):
                    parentOpen = (int)ControllerName.WareHouses;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.WareHouseIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.WareHouseCreate;
                    }
                    break;

                case nameof(ControllerName.Sliders):
                    parentOpen = (int)ControllerName.Sliders;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.SliderIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.SliderCreate;
                    }
                    break;
                case nameof(ControllerName.Posts):
                    parentOpen = (int)ControllerName.Posts;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.PostIndex;
                    }
                    else if (actionName == nameof(ActionName.Create))
                    {
                        viewOpen = (int)ViewOpen.PostCreate;
                    }
                    break;

                case nameof(ControllerName.Contacts):
                    parentOpen = (int)ControllerName.Contacts;
                    if (actionName == nameof(ActionName.Index))
                    {
                        viewOpen = (int)ViewOpen.ContactIndex;
                    }
                    else if (actionName == nameof(ActionName.Info))
                    {
                        viewOpen = (int)ViewOpen.ContactInfo;
                    }
                    break;

                case nameof(ControllerName.Comments):
                    parentOpen = (int)ControllerName.Comments;
                    if (actionName == nameof(ActionName.Post))
                    {
                        viewOpen = (int)ViewOpen.CommentPost;
                    }
                    else if (actionName == nameof(ActionName.Product))
                    {
                        viewOpen = (int)ViewOpen.CommentProduct;
                    }
                    break;

                default:
                    break;
            }

            ViewBag.ParentOpen = parentOpen;
            ViewBag.ViewOpen = viewOpen;
        }

        public void SetBreadCrumb()
        {
            string ctrlName = ControllerContext.RouteData.Values["controller"].ToString();
            string actName = ControllerContext.RouteData.Values["action"].ToString();

            const string ADD_NEW = "Thêm mới";
            const string EDIT = "Chỉnh sửa";
            const string DETAILS = "Chi tiết";

            VMBreadCrumb breadCrum_lv1 = new VMBreadCrumb();
            VMBreadCrumb breadCrum_lv2 = new VMBreadCrumb();

            switch (ctrlName)
            {
                case nameof(ControllerName.Home):
                    break;

                #region Users
                case nameof(ControllerName.Users):
                    breadCrum_lv1.BreadCrumbName = "Users";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Create))
                    {
                        breadCrum_lv2.BreadCrumbName = ADD_NEW;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Create.ToString();
                    }
                    else if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    else if (actName == nameof(ActionName.Edit))
                    {
                        breadCrum_lv2.BreadCrumbName = EDIT;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    break;
                #endregion

                #region Brands
                case nameof(ControllerName.Brands):
                    breadCrum_lv1.BreadCrumbName = "Nhãn hiệu";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Create))
                    {
                        breadCrum_lv2.BreadCrumbName = ADD_NEW;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Create.ToString();
                    }
                    else if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    else if (actName == nameof(ActionName.Edit))
                    {
                        breadCrum_lv2.BreadCrumbName = EDIT;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    break;
                #endregion

                #region Posts
                case nameof(ControllerName.Posts):
                    breadCrum_lv1.BreadCrumbName = "Bài viết";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Create))
                    {
                        breadCrum_lv2.BreadCrumbName = ADD_NEW;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Create.ToString();
                    }
                    else if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    else if (actName == nameof(ActionName.Edit))
                    {
                        breadCrum_lv2.BreadCrumbName = EDIT;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    break;
                #endregion

                #region Category
                case nameof(ControllerName.Categories):
                    breadCrum_lv1.BreadCrumbName = "Chuyên mục";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Create))
                    {
                        breadCrum_lv2.BreadCrumbName = ADD_NEW;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Create.ToString();
                    }
                    else if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    else if (actName == nameof(ActionName.Edit))
                    {
                        breadCrum_lv2.BreadCrumbName = EDIT;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    break;
                #endregion

                #region Products
                case nameof(ControllerName.Products):
                    breadCrum_lv1.BreadCrumbName = "Sản phẩm";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Create))
                    {
                        breadCrum_lv2.BreadCrumbName = ADD_NEW;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Create.ToString();
                    }
                    else if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    else if (actName == nameof(ActionName.Edit))
                    {
                        breadCrum_lv2.BreadCrumbName = EDIT;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    break;
                #endregion

                #region Sliders
                case nameof(ControllerName.Sliders):
                    breadCrum_lv1.BreadCrumbName = "Sliders";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Create))
                    {
                        breadCrum_lv2.BreadCrumbName = ADD_NEW;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Create.ToString();
                    }
                    else if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    else if (actName == nameof(ActionName.Edit))
                    {
                        breadCrum_lv2.BreadCrumbName = EDIT;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    break;
                #endregion

                #region Suppliers
                case nameof(ControllerName.Suppliers):
                    breadCrum_lv1.BreadCrumbName = "Nhà cung cấp";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Create))
                    {
                        breadCrum_lv2.BreadCrumbName = ADD_NEW;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Create.ToString();
                    }
                    else if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    else if (actName == nameof(ActionName.Edit))
                    {
                        breadCrum_lv2.BreadCrumbName = EDIT;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Edit.ToString();
                    }
                    break;
                #endregion

                #region Comments
                case nameof(ControllerName.Comments):
                    if (actName == nameof(ActionName.Post))
                    {
                        breadCrum_lv1.BreadCrumbName = "Bình luận Bài viết";
                        breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv1.BreadCrumbAct = ActionName.Post.ToString();
                    }
                    else if (actName == nameof(ActionName.Product))
                    {
                        breadCrum_lv1.BreadCrumbName = "Bình luận Sản phẩm";
                        breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv1.BreadCrumbAct = ActionName.Product.ToString();
                    }
                    break;
                #endregion

                #region Contacts
                case nameof(ControllerName.Contacts):
                    breadCrum_lv1.BreadCrumbName = "Liên hệ";
                    breadCrum_lv1.BreadCrumbCtrl = ctrlName;
                    breadCrum_lv1.BreadCrumbAct = ActionName.Index.ToString();
                    if (actName == nameof(ActionName.Details))
                    {
                        breadCrum_lv2.BreadCrumbName = DETAILS;
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Details.ToString();
                    }
                    else if (actName == nameof(ActionName.Info))
                    {
                        breadCrum_lv2.BreadCrumbName = "Thông tin Shop";
                        breadCrum_lv2.BreadCrumbCtrl = ctrlName;
                        breadCrum_lv2.BreadCrumbAct = ActionName.Info.ToString();
                    }
                    break;
                #endregion

                default:
                    break;
            }

            ViewBag.BreadCrumb_lv1 = breadCrum_lv1;
            ViewBag.BreadCrumb_lv2 = breadCrum_lv2;
        }

        protected void SaveSession(User user)
        {
            //Session["id"] = user.Id;
            //Session["username"] = user.UserName;
            //Session["fullname"] = user.FullName;
            //Session["access"] = user.Access;
            //Session["status"] = user.Status;
            Session["user"] = user;
        }

        protected void LoadSession()
        {
            if (LoginUser == null)
            {
                LoginUser = new User();
            }

            //var sessionTmp = Session["id"];
            //if (sessionTmp != null)
            //{
            //    LoginUser.Id = (int)sessionTmp;
            //}

            //sessionTmp = Session["username"];
            //if (sessionTmp != null)
            //{
            //    LoginUser.UserName = sessionTmp.ToString();
            //}

            //sessionTmp = Session["access"];
            //if (sessionTmp != null)
            //{
            //    LoginUser.Access = (RoleType)sessionTmp;
            //}

            //sessionTmp = Session["status"];
            //if (sessionTmp != null)
            //{
            //    LoginUser.Status = (StatusAccountType)sessionTmp;
            //}

            LoginUser = (User)Session["user"];
        }

        protected void DestroySession()
        {
            Session.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

    public class VMBreadCrumb
    {
        public string BreadCrumbName { get; set; }

        public string BreadCrumbCtrl { get; set; }

        public string BreadCrumbAct { get; set; }
    }
}