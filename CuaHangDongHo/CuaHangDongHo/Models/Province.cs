using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Models
{
    public partial class Province
    {
        [Key]
        [MaxLength(5)]
        public String Id { get; set; }
        
        [MaxLength(100)]
        public String Name { get; set; }

        [MaxLength(30)]
        public String Type { get; set; }
    }
}