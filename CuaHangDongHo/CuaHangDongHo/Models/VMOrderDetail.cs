using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Models
{
    public class VMOrderDetail
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        [Column(TypeName = "date")]

        public string ExportDate { get; set; }

        public string Status { get; set; }
        
        public string Description { get; set; }

        public string Created_at { get; set; }

        public List<ItemOrder> ItemOrders { get; set; }

        public string TotalPrice { get; set; }

        public VMOrderDetail()
        {
            ItemOrders = new List<ItemOrder>();
        }
    }

}