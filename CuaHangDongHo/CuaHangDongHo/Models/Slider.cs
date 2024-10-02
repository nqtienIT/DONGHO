namespace CuaHangDongHo.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Slider
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "Tiêu đề Chính")]
        public string ShortTitle { get; set; }

        [StringLength(255)]
        [Display(Name = "Tiêu đề Phụ")]
        public string LongTitle { get; set; }

        [Required]
        [Display(Name = "Chi tiết")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Ảnh chính")]
        public string Img { get; set; }

        [Display(Name = "Ảnh phụ")]
        public string SmallImg { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }
    }
}
