using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace CuaHangDongHo.Models
{
    public class ContactInfo
    {
        private EntrySetContext db = new EntrySetContext();

        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "Chịu trách nhiệm")]
        public string Responsible { get; set; }

        [Required]
        [Display(Name = "Địa chỉ bản đồ (HTML)")]
        public string Map { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public int Address { get; set; }

        [MaxLength(50)]
        public string Fax { get; set; }

        [MaxLength(50)]
        [Display(Name = "Địa chỉ Facebook")]
        public string Facebook { get; set; }

        [MaxLength(50)]
        [Display(Name = "Địa chỉ Twitter")]
        public string Twitter { get; set; }

        [MaxLength(50)]
        [Display(Name = "Địa chỉ Zalo")]
        public string Zalo { get; set; }

        [NotMapped]
        public string AddressInfo { get; set; }

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

        [NotMapped]
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

        [NotMapped]
        public List<SelectListItem> lstCommunes
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Xã/Phường" }
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