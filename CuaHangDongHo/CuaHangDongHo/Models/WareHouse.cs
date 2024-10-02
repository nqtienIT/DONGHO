using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Models
{
    public partial class WareHouse
    {
        private EntrySetContext db = new EntrySetContext();

        [Key]
        public int Id { get; set; }

        public String Name { get; set; }

        public WareHoueType Type { get; set; }

        [NotMapped]
        public String TypeName { get; set; }

        public int ProductId { get; set; }

        [NotMapped]
        public String ProductName { get; set; }

        public int QuantityChange { get; set; }

        public String Description { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        public int Created_by { get; set; }

        public int? SupplierId { get; set; }

        [NotMapped]
        public String SupplierName { get; set; }

        [NotMapped]
        public List<SelectListItem> lstProducts
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Chọn Sản phẩm --" }
                };

                List<Product> lstProducts = db.Products.OrderBy(p => p.Name).ToList();
                foreach (Product item in lstProducts)
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
        public List<SelectListItem> lstSuppliers
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Chọn Sản phẩm --" }
                };

                List<Supplier> lstSuppliers = db.Suppliers.OrderBy(p => p.Name).ToList();
                foreach (Supplier item in lstSuppliers)
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
    }
}