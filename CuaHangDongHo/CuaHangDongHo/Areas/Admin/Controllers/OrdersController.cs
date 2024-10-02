using CuaHangDongHo.Areas.Admin.Filters;
using CuaHangDongHo.Defines;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        // GET: Admin/Orders
        [AdminFilter]
        public ActionResult Index()
        {
            List<Order> orders = GetOrders();
            return View(orders);
        }

        public List<Order> GetOrders()
        {
            // danh sach order theo user 
            List<Order> order = db.Orders.ToList();

            // danh sach id order
            List<int> lstOrderId = order.Select(a => a.Id).ToList();

            var orderDetail = (from od in db.OrderDetails.Where(d => lstOrderId.Contains(d.OrderId))
                               join p in db.Products on od.ProductId equals p.Id
                               select new
                               {
                                   od.OrderId,
                                   od.Quantity,
                                   price = p.PriceSale ?? p.Price
                               }).ToList();

            foreach (var o in order)
            {
                double price = 0;
                foreach (var d in orderDetail)
                {
                    if (o.Id == d.OrderId)
                    {
                        o.Quantity += d.Quantity;
                        price += d.price * d.Quantity;
                    }
                }
                o.Created_at = o.Created_at;
                o.CustomerEmail = o.CustomerName;
                o.TotalPrice = FormatString.FormatMoneyVND(price);
                o.StatusOrder = o.Status.GetEnumDisplayName();
            }

            return order.OrderBy(o => o.Status).ToList();
        }

        [AdminFilter]
        public ActionResult Details(int id)
        {

            VMOrderDetail model = GetOrderDetail(id);
            return View(model);
        }

        [AdminFilter]
        public ActionResult Edit(int id)
        {
            if (TempData[ERR_DATA] != null && TempData[ERR_DATA].ToString() != null)
            {
                ViewBag.Err = TempData[ERR_DATA].ToString();
            }
            VMOrderDetail model = GetOrderDetail(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Enums.OrderStatus status, string description, int id)
        {
            LoadSession();
            Order order = db.Orders.Find(id);
            order.Updated_at = DateTime.Now;
            order.Description = description;
            order.Status = status;
            if (status == Enums.OrderStatus.Delivering)
            {
                order.ExportDate = DateTime.Now;

                List<OrderDetail> orderDetails = db.OrderDetails.Where(o => o.OrderId == id).ToList();
                foreach (var item in orderDetails)
                {
                    Product product = db.Products.Find(item.ProductId);
                    product.Number -= item.Quantity;
                    if (product.Number < 0)
                    {
                        TempData[ERR_DATA] = "Cập nhật thất bại. Không đủ số lượng";
                        return RedirectToAction("Edit", id);
                    }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    WareHouse wareHouse = new WareHouse
                    {
                        Name = "Giao Hàng cho " + order.CustomerName,
                        Type = Enums.WareHoueType.Export,
                        ProductId = item.ProductId,
                        QuantityChange = item.Quantity,
                        Description = String.Format("{0} - {1} - {2}",
                                                     order.CustomerName,
                                                     order.CustomerEmail,
                                                     order.CustomerPhone),
                        Created_at = DateTime.Now,
                        Updated_at = DateTime.Now,
                        Created_by = LoginUser.Id,
                        SupplierId = product.BrandId
                    };
                    db.WareHouses.Add(wareHouse);
                    db.SaveChanges();
                }
            }

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            List<OrderDetail> orderDetails = db.OrderDetails.Where(o => o.OrderId == id).ToList();
            foreach (var item in orderDetails)
            {
                OrderDetail orderDetail = db.OrderDetails.Find(item.Id);
                db.OrderDetails.Remove(orderDetail);
                db.SaveChanges();
            }
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public VMOrderDetail GetOrderDetail(int id)
        {
            var orderDetail = (from o in db.Orders
                               join od in db.OrderDetails on o.Id equals od.OrderId
                               where o.Id == id
                               select new
                               {
                                   o.ExportDate,
                                   o.Status,
                                   o.Description,
                                   o.Created_at,
                                   o.CustomerName,
                                   o.CustomerEmail,
                                   o.CustomerPhone,
                                   o.CustomerAddress,
                                   od.ProductId,
                                   od.Quantity
                               }).ToList();

            int idAddress = orderDetail[0].CustomerAddress;
            var address = (from a in db.AddressInfoes
                           join p in db.Provinces on a.ProvinceId equals p.Id
                           join d in db.Districts on a.DistrictId equals d.Id
                           join c in db.Communes on a.CommuneId equals c.Id
                           where a.Id == idAddress
                           select new
                           {
                               detail = a.Detail,
                               communes = c.Name,
                               district = d.Name,
                               province = p.Name
                           }).FirstOrDefault();

            VMOrderDetail model = new VMOrderDetail
            {
                FullName = orderDetail[0].CustomerName,
                Email = orderDetail[0].CustomerEmail,
                Phone = orderDetail[0].CustomerPhone,
                Address = address == null ? String.Empty : FormatString.FormatAddress(address.detail, address.communes, address.district, address.province),
                Status = orderDetail[0].Status.GetEnumDisplayName(),
                Description = orderDetail[0].Description,
                ExportDate = orderDetail[0].ExportDate == null ? String.Empty : DateTimeMgrs.FormatDateTimeVNese(orderDetail[0].Created_at),
                Created_at = DateTimeMgrs.FormatDateTimeVNese(orderDetail[0].Created_at),
                Id = id
            };

            double totalPrice = 0;
            foreach (var item in orderDetail)
            {
                var product = db.Products.Find(item.ProductId);
                double price = (double)(item.Quantity * (product.PriceSale == null ? product.Price : product.PriceSale));
                ItemOrder itemOrder = new ItemOrder
                {
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId,
                    Img = product.Img,
                    PriceItem = FormatString.FormatMoneyVND((double)(product.PriceSale == null ? product.Price : product.PriceSale)),
                    PriceAllItem = FormatString.FormatMoneyVND(price)
                };
                model.ItemOrders.Add(itemOrder);
                totalPrice += price;
            }
            model.TotalPrice = FormatString.FormatMoneyVND(totalPrice);

            return model;
        }
    }
}