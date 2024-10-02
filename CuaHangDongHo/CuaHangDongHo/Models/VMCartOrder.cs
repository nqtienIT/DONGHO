using CuaHangDongHo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Models
{
    public class VMCartOrder
    {
        private readonly EntrySetContext db = new EntrySetContext();

        public string TotalPrice
        {
            get
            {
                //if (ItemOrders.Count > 0)
                //{
                //    double price = 0;
                //    foreach (var item in ItemOrders)
                //    {
                //        Product product = db.Products.Find(item.ProductId);
                //        if (product == null)
                //        {
                //            continue;
                //        }

                //        if (product.PriceSale == null)
                //        {
                //            price += item.Quantity * product.Price;
                //        }
                //        else
                //        {
                //            price += (double)(item.Quantity * product.PriceSale);
                //        }
                //    }
                //    return FormatString.FormatMoneyVND(price);
                //}
                return String.Empty;
            }
        }

        public List<LstItemOrders> AllItemOrder { get; set; }

        public int OrderId { get; set; }

        public string Status { get; set; }

        public VMCartOrder()
        {
            AllItemOrder = new List<LstItemOrders>();
        }
    }

    public class LstItemOrders
    {
        public List<ItemOrder> ItemOrders { get; set; }
        public LstItemOrders()
        {
            ItemOrders = new List<ItemOrder>();
        }
    }

    public class ItemOrder
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Img { get; set; }

        public string ProductName { get; set; }

        public double PriceDouble { get; set; }

        public string PriceItem { get; set; }

        public string PriceAllItem { get; set; }
    }
}