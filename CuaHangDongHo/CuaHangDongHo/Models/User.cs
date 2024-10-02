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

    public partial class User
    {
        private EntrySetContext db = new EntrySetContext();

        [Key]
        public int Id { get; set; }

        [Display(Name = "Họ và Tên")]
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Display(Name = "Tên Đăng Nhập")]
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Display(Name = "Giới tính")]
        [EnumDataType(typeof(GenderType))]
        public GenderType Gender { get; set; }

        [Display(Name = "Số Điện thoại")]
        [Required]
        [RegularExpression("(03[2|3|4|5|6|7|8|9]|05[2|5|6|8|9]|07[0|6|7|8|9]|08[0-9]|09[0-9])+([0-9]{7})\\b", ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại di động")]
        [StringLength(10, ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại di động")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public int Address { get; set; }

        [NotMapped]
        public string AddressInfo { get; set; }

        [Display(Name = "Quyền")]
        [EnumDataType(typeof(RoleType))]
        public RoleType Access { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        [Display(Name = "Trạng thái")]
        [EnumDataType(typeof(StatusAccountType))]
        public StatusAccountType Status { get; set; }

        [Display(Name = "Loại tài khoản")]
        public AccountType? Type { get; set; }

        [NotMapped]
        [Display(Name = "Nhập lại mật khẩu")]
        [Required]
        [StringLength(255)]
        public string PasswordAgain { get; set; }

        [NotMapped]
        public bool SaveLogin { get; set; }

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

        public User()
        {
            Access = RoleType.User;
            Status = StatusAccountType.InActive;
            Type = AccountType.System;
        }
    }
}
