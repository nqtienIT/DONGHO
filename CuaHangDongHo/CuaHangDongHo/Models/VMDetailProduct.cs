using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CuaHangDongHo.Models
{
    public class VMDetailProduct
    {
        public int Id { get; set; }
        public string NameProduct { get; set; }

        public string PriceVND { get; set; }

        public string PriceSaleVND { get; set; }

        public string MainPicture { get; set; }

        public string DetailProduct { get; set; }

        public string DescriptionProduct { get; set; }

        public string NameBrand { get; set; }

        public string DetailBrand { get; set; }

        public string DescriptionBrand { get; set; }

        public string WatchesType { get; set; }

        public string WatchesStrap { get; set; }

        public List<string> Pictures { get; set; }

        public VMDetailProduct()
        {
            Pictures = new List<string>();
        }

    }
}