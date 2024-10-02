using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace CuaHangDongHo.Models
{
    public partial class Supplier
    {
        private EntrySetContext db = new EntrySetContext();

        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public String Name { get; set; }

        [Required]
        public int Adress { get; set; }

        [Required]
        [MaxLength(15)]
        public String Phone { get; set; }

        [MaxLength(255)]
        public String Description { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        [NotMapped]
        [MaxLength(5)]
        [Required]
        public String ProvinceId { get; set; }

        [NotMapped]
        [MaxLength(5)]
        [Required]
        public String DistricstId { get; set; }

        [NotMapped]
        [MaxLength(5)]
        [Required]
        public String CommuneId { get; set; }

        [NotMapped]
        [MaxLength(255)]
        [Required]
        public String DetailAddress { get; set; }

        [NotMapped]
        public List<SelectListItem> lstProvinces
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Chọn Tỉnh/Thành phố --" }
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

        [NotMapped]
        public List<SelectListItem> lstDistricts
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Chọn Quận/Huyện --" }
                };

                // su dung cho create, update bi loi
                if(!String.IsNullOrEmpty(ProvinceId))
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

        [NotMapped]
        public List<SelectListItem> lstCommunes
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- Chọn Xã --" }
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
}