using CuaHangDongHo.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Models
{
    public class VMCartItem
    {
        protected EntrySetContext db = new EntrySetContext();

        public string TotalPrice
        {
            get
            {
                if (CartItems.Count > 0)
                {
                    double price = 0;
                    foreach (var item in CartItems)
                    {
                        Product product = db.Products.Find(item.ProductId);
                        if (product == null)
                        {
                            continue;
                        }

                        if (product.PriceSale == null)
                        {
                            price += item.Quantity * product.Price;
                        }
                        else
                        {
                            price += (double)(item.Quantity * product.PriceSale);
                        }
                    }
                    return FormatString.FormatMoneyVND(price);
                }
                return String.Empty;
            }
        }

        public List<CartItem> CartItems { get; set; }

        public UserInfo UserInfo { get; set; }

        public string Description { get; set; }

        public VMCartItem()
        {
            UserInfo = new UserInfo();
            CartItems = new List<CartItem>();
        }
    }

    public class UserInfo
    {
        private EntrySetContext db = new EntrySetContext();

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [MaxLength(5)]
        [Required]
        public String ProvinceId { get; set; }

        [MaxLength(5)]
        [Required]
        public String DistricstId { get; set; }

        [MaxLength(5)]
        [Required]
        public String CommuneId { get; set; }

        [MaxLength(255)]
        [Required]
        public String DetailAddress { get; set; }

        public List<SelectListItem> lstProvinces
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Tỉnh/Thành phố" }
                };

                List<Province> lstProvinces = db.Provinces.OrderBy(p => p.Name).ToList();
                foreach (Province item in lstProvinces)
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

        public List<SelectListItem> lstDistricts
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Quận/Huyện" }
                };

                // su dung cho create, update bi loi
                if (!String.IsNullOrEmpty(ProvinceId))
                {
                    List<District> lstDistricts = db.Districts.Where(a => a.ProvinceId == ProvinceId).OrderBy(p => p.Name).ToList();
                    foreach (District item in lstDistricts)
                    {
                        result.Add(new SelectListItem
                        {
                            Value = item.Id.ToString(),
                            Text = item.Name
                        });
                    }
                }
                return result;
            }
        }

        public List<SelectListItem> lstCommunes
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Chọn Xã" }
                };

                // su dung cho create, update bi loi
                if (!String.IsNullOrEmpty(DistricstId))
                {
                    List<Commune> lstCommunes = db.Communes.Where(a => a.DistrictId == DistricstId).OrderBy(p => p.Name).ToList();
                    foreach (Commune item in lstCommunes)
                    {
                        result.Add(new SelectListItem
                        {
                            Value = item.Id.ToString(),
                            Text = item.Name
                        });
                    }
                }
                return result;
            }
        }
    }

    public class CartItem
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Img { get; set; }

        public string ProductName { get; set; }

        public double PriceDouble { get; set; }

        public string PriceItem { get; set; }

        public string PriceAllItem { get; set; }
    }
}