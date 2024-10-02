namespace CuaHangDongHo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using static CuaHangDongHo.Defines.Enums;

    public partial class Order
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExportDate { get; set; }

        public string Description { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        [Required]
        public int CustomerAddress { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public string CustomerEmail { get; set; }

        public int Created_by { get; set; }

        public DateTime Created_at { get; set; }

        public DateTime? Updated_at { get; set; }

        [NotMapped]
        public int Quantity { get; set; }

        [NotMapped]
        public string TotalPrice { get; set; }

        [NotMapped]
        public string StatusOrder { get; set; }
    }
}
