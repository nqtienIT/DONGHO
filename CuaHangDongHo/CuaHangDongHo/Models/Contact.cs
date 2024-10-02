namespace CuaHangDongHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using static CuaHangDongHo.Defines.Enums;

    public partial class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Tên")]
        public string FullName { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        [EnumDataType(typeof(StatusContact))]
        [Display(Name = "Trạng thái")]
        public StatusContact? Status { get; set; }

        [NotMapped]
        [Display(Name = "Trạng thái")]
        public string StatusDetail { get; set; }
    }
}
