namespace CuaHangDongHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [Required]
        [Display(Name = "Nội dung chính")]

        public string ShortDetail { get; set; }

        [Required]
        [Display(Name = "Nội dung chi tiết")]
        public string Detail { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Hình ảnh")]
        public string Img { get; set; }

        [Required]
        public int Created_by { get; set; }
        
        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        public int? Status { get; set; }
    }
}
