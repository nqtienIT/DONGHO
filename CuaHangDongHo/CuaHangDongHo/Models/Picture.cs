namespace CuaHangDongHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Picture
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public DateTime? Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        public int? Status { get; set; }
    }
}
