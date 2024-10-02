namespace CuaHangDongHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web.Mvc;

    public partial class Category
    {
        private EntrySetContext db = new EntrySetContext();

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public int ParentId { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        public int? Status { get; set; }

        [NotMapped]
        [Display(Name = "Danh mục cha")]
        public string ParentStr { get; set; }

        [NotMapped]
        public List<SelectListItem> lstParentItem
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "-- Danh mục cha --" }
                };

                List<Category> lstCateParent = db.Categories.Where(c => c.ParentId == 0).ToList();
                foreach (Category cate in lstCateParent)
                {
                    result.Add(new SelectListItem
                    {
                        Value = cate.Id.ToString(),
                        Text = cate.Name
                    });
                }

                return result;
            }
        }
    }
}
