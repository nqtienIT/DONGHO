namespace CuaHangDongHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;
    using static CuaHangDongHo.Defines.Enums;

    public partial class Product
    {
        private EntrySetContext db = new EntrySetContext();

        public int Id { get; set; }

        [Display(Name = "Thương hiệu")]
        public int BrandId { get; set; }

        [Display(Name ="Danh mục")]
        public int CateId { get; set; }

        [Display(Name = "Tên Sản Phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập Tên Sản Phẩm")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Slug")]
        [StringLength(255, ErrorMessage = "Vui lòng nhập trong 255 ký tự.")]
        public string Slug { get; set; }

        [Display(Name = "Ảnh")]
        [Required(ErrorMessage = "Vui lòng chọn Ảnh")]
        [StringLength(255, ErrorMessage = "Vui lòng chọn hình ảnh có tên tối đa 255 ký tự.")]
        public string Img { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Chi tiết Sản Phẩm")]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        [Display(Name = "Số lượng")]
        [Range(minimum: 0, maximum: Int32.MaxValue, ErrorMessage = "Vui lòng nhập số lượng lớn hơn 0.")]
        [Required(ErrorMessage = "Vui lòng nhập Số lượng Sản Phẩm")]
        public int Number { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Giá Sản Phẩm")]
        [Range(minimum: 0, maximum: Int32.MaxValue, ErrorMessage = "Vui lòng nhập giá lớn hơn 0.")]
        public double Price { get; set; }

        [Range(minimum: 0, maximum: Int32.MaxValue, ErrorMessage = "Vui lòng nhập giá lớn hơn 0.")]
        [Display(Name = "Giá giảm")]
        public double? PriceSale { get; set; }

        [Display(Name = "Ghi chú")]
        public String Description { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        [Display(Name = "Trạng thái")]
        [EnumDataType(typeof(StatusProductType))]
        public StatusProductType Status { get; set; }

        [Display(Name="Loại đồng hồ")]
        [EnumDataType(typeof(WatchesType))]
        public WatchesType? Type { get; set; }

        [Display(Name = "Dây đeo")]
        [EnumDataType(typeof(WatchesStrap))]
        public WatchesStrap? Strap { get; set; }

        [Display(Name = "Số lượng đã bán")]
        public int? Sold { get; set; }

        [NotMapped]
        [Display(Name = "Thương hiệu")]
        public string BrandName { get; set; }

        [NotMapped]
        public String CateName { get; set; }

        [Display(Name = "Giá")]
        [NotMapped]
        public String PriceVND { get; set; }

        [NotMapped]
        public String PriceSaleVND { get; set; }

        [NotMapped]
        [Display(Name = "Trạng thái")]
        public String StatusProduct { get; set; }

        [NotMapped]
        public String NewImg { get; set; }

        [NotMapped]
        public List<SelectListItem> lstCategories
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "-- Chọn danh mục --" }
                };

                List<Category> lstCate = db.Categories.Where(c => c.ParentId != 0).ToList();
                foreach (Category cate in lstCate)
                {
                    result.Add(new SelectListItem
                    {
                        Value = cate.Id.ToString(),
                        Text = cate.Name
                    });
                }

                return result;
            }
        }

        [NotMapped]
        public List<SelectListItem> lstBrands
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "-- Chọn thương hiệu --" }
                };

                List<Brand> lstBrands = db.Brands.ToList();
                foreach (Brand item in lstBrands)
                {
                    result.Add(new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name
                    });
                }

                return result;
            }
        }

        [NotMapped]
        public List<String> lstImg { get; set; }
    }
}
