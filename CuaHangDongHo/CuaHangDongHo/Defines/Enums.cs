using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Defines
{
    public class Enums
    {
        // trang thai kinh doanh san pham
        public enum StatusProductType : int
        {
            [Display(Name = "Đang kinh doanh")]
            Still = 0,
            [Display(Name = "Đã ngừng kinh doanh")]
            Out
        }

        // quyen user
        public enum RoleType : int
        {
            [Display(Name = "Quản lý")]
            Admin = 0,
            [Display(Name = "Nhân viên")]
            Staff,
            [Display(Name = "Người dùng")]
            User
        }

        // gioi tinh
        public enum GenderType : int
        {
            [Display(Name = "Nam")]
            Male = 0,
            [Display(Name = "Nữ")]
            Female
        }

        // nhap/xuat kho
        public enum WareHoueType : int
        {
            [Display(Name = "Nhập")]
            Import = 0,
            [Display(Name = "Xuất")]
            Export
        }

        // trang thai kich hoat account user
        public enum StatusAccountType : int
        {
            [Display(Name = "Kích hoạt")]
            Active = 0,
            [Display(Name = "Bị khóa")]
            InActive
        }

        // phan loai account
        public enum AccountType : int
        {
            [Display(Name = "Thường")]
            System = 0,
            [Display(Name = "Facebook")]
            Facebook
        }

        // controller
        public enum ControllerName : int
        {
            Home = 1,
            Brands,
            Users,
            Categories,
            Products,
            Suppliers,
            WareHouses,
            Sliders,
            Posts,
            Contacts,
            Comments,
        }

        // action
        public enum ActionName : int
        {
            Default = 0,
            Index,
            Create,
            Delete,
            Edit,
            Info,
            Post,
            Product,
            Details
        }

        // active menu o trang admin
        public enum ViewOpen : int
        {
            HomeIndex = 11,

            BrandIndex = 21,
            BrandCreate = 22,
            
            UserIndex = 31,
            UserCreate = 32,

            ProductIndex = 41,
            ProductCreate = 42,

            CategoryIndex = 51,
            CategoryCreate = 52,

            SupplierIndex = 61,
            SupplierCreate = 62,

            WareHouseIndex = 71,
            WareHouseCreate = 72,

            SliderIndex = 81,
            SliderCreate = 82,

            PostIndex = 91,
            PostCreate = 92,

            ContactIndex = 101,
            ContactInfo = 102,

            CommentPost = 111,
            CommentProduct = 112
        }

        // ten session
        public enum SessionName : int
        {
            User = 1,
            Cart = 2
        }

        
        public enum StatusProduct : int
        {
            [Display(Name = "Còn hàng")]
            OutStock = 0,
            [Display(Name = "Hết hàng")]
            Stocking
        }

        // day dong ho
        public enum WatchesStrap : int
        {
            [Display(Name = "Dây Silicone thường")]
            Normal = 1,

            [Display(Name = "Dây vải")]
            Fabric,

            [Display(Name = "Dây nhựa PU")]
            Polyurethane,

            [Display(Name = "Da thường")]
            NormalLeather,

            [Display(Name = "Da cá sấu")]
            CrocodileLeather,

            [Display(Name = "Dây dù")]
            SmoothRope,

            [Display(Name = "Dây kim loại")]
            MetalWire,

            [Display(Name = "Thép không gỉ")]
            StainlessSteel,

            [Display(Name = "Titanium")]
            Titanium,

            [Display(Name = "Dây cao su")]
            RubberBand,

            [Display(Name = "Khác")]
            Other,
        }

        // loai dong ho
        public enum WatchesType : int
        {
            [Display(Name = "Đồng hồ thường")]
            Normal = 1,

            [Display(Name = "Đồng hồ cơ")]
            Mechanical,

            [Display(Name = "Đồng hồ điện tử")]
            Electronic,

            [Display(Name = "Đồng hồ thông minh")]
            SmartWatch,

            [Display(Name = "Phiên bản đặc biệt")]
            SpecialVersion,
        }

        // tinh trang don hang
        public enum OrderStatus : int
        {
            [Display(Name = "Đang chờ xử lý")]
            Pending = 0,

            [Display(Name = "Đang xử lý")]
            Preparing,

            [Display(Name = "Đang giao hàng")]
            Delivering,

            [Display(Name = "Hoàn tất")]
            Finished,

            [Display(Name = "Huỷ bỏ")]
            Cancel
        }

        public enum StatusComment : int
        {
            [Display(Name = "Chưa duyệt")]
            Pending = 0,

            [Display(Name = "Đã duyệt")]
            Accept
        }

        public enum StatusContact : int
        {
            [Display(Name = "Chưa xem")]
            Pending = 0,

            [Display(Name = "Đã xem")]
            Watched
        }

        public enum CommentType : int
        {
            Post = 1,
            Product
        }
    }
}