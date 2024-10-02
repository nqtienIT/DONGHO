using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Models
{
    public partial class AddressInfo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(5)]
        public String ProvinceId { get; set; }

        [MaxLength(5)]
        public String DistrictId { get; set; }

        [MaxLength(5)]
        public String CommuneId { get; set; }

        [MaxLength(255)]
        public String Detail { get; set; }
    }
}