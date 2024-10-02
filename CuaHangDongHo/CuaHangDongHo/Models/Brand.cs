namespace CuaHangDongHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Brand
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên")]
        [MaxLength(255)]
        public String Name { get; set; }
        [MaxLength(255)]

        [Display(Name = "Ảnh")]
        public String Img { get; set; }

        [Display(Name = "Ghi chú")]
        public String Description { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        public int Status { get; set; }

        [NotMapped]
        public String NewImg { get; set; }

        [NotMapped]
        public int ProductCount { get; set; }

        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }
    }
}
