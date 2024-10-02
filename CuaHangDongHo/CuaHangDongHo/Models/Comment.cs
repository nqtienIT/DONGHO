using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Nội dung")]
        public string Detail { get; set; }

        public int? PostId { get; set; }

        public int? ProductId { get; set; }

        [EnumDataType(typeof(CommentType))]
        public CommentType Type { get; set; }

        [Required]
        public int Created_by { get; set; }

        public DateTime? Created_at { get; set; }

        [NotMapped]
        public string Created_at_Format { get; set; }

        [EnumDataType(typeof(StatusComment))]
        public StatusComment? Status { get; set; }

        [NotMapped]
        [Display(Name ="Trạng thái")]
        public string StatusDetail { get; set; }

        [NotMapped]
        public UserComment UserComment { get; set; }

        [NotMapped]
        public PostComment PostComment { get; set; }

        [NotMapped]
        public ProductComment ProductComment { get; set; }
    }

    public class UserComment
    {
        [Display(Name = "Người dùng")]
        public string UserName { get; set; }

        public string UserUrl { get; set; }
    }

    public class PostComment
    {
        [Display(Name = "Tiêu đề")]
        public string PostTitle { get; set; }

        public string PostUrl { get; set; }
    }

    public class ProductComment
    {
        [Display(Name = "Sản phẩm")]
        public string ProductName { get; set; }

        public string ProductUrl { get; set; }
    }
}