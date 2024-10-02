using CuaHangDongHo.Defines;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Controllers
{
    public class CartController : BaseController
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart(int id, int quantity = 1)
        {
            if (LoginUser == null || LoginUser.Id < 1)
            {
                return Json(new { err = 1, msg = "Chua dang nhap!" }, JsonRequestBehavior.AllowGet);
            }

            LoadSessionCart();

            // giỏ rỗng
            if (VMCartItem.CartItems.Count == 0)
            {
                // tạo mới item
                CartItem item = new CartItem
                {
                    ProductId = id,
                    Quantity = quantity
                };


                // thêm vào list
                VMCartItem.CartItems.Add(item);
            }
            // giỏ đã tồn tại
            else
            {
                // sản phẩm đã có trong giỏ
                if (VMCartItem.CartItems.Exists(x => x.ProductId == id))
                {
                    foreach (var item in VMCartItem.CartItems)
                    {
                        // cập nhật số lượng
                        if (item.ProductId == id)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                // sản phẩm chưa có trong giỏ
                else
                {
                    // tạo mới giỏ
                    CartItem item = new CartItem
                    {
                        ProductId = id,
                        Quantity = quantity
                    };
                    VMCartItem.CartItems.Add(item);
                }
            }

            SaveSession(SessionName.Cart, VMCartItem);

            int totalProduct = VMCartItem.CartItems.GroupBy(a => a.ProductId).Count();

            return Json(new { err = 0, msg = totalProduct }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details()
        {
            LoadSessionUser();
            if (LoginUser == null || LoginUser.Id < 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            LoadSessionCart();
            if (VMCartItem.CartItems.Count > 0)
            {
                foreach (var item in VMCartItem.CartItems)
                {
                    Product product = db.Products.Find(item.ProductId);
                    if (product == null)
                    {
                        continue;
                    }

                    item.ProductName = product.Name;
                    item.Img = product.Img;

                    double price = (double)(product.PriceSale == null ? product.Price : product.PriceSale);
                    item.PriceItem = FormatString.FormatMoneyVND(price);
                    item.PriceAllItem = FormatString.FormatMoneyVND(item.Quantity * price);
                }
            }

            VMCartItem.UserInfo.Name = LoginUser.FullName;
            VMCartItem.UserInfo.Email = LoginUser.Email;
            VMCartItem.UserInfo.Phone = LoginUser.Phone;

            //var address = (from u in db.Users
            //               join a in db.AddressInfoes on u.Address equals a.Id
            //               join c in db.Communes on a.CommuneId equals c.Id
            //               join d in db.Districts on a.DistrictId equals d.Id
            //               join p in db.Provinces on a.ProvinceId equals p.Id
            //               select new
            //               {
            //                   detail = a.Detail,
            //                   commune = c.Name,
            //                   distric = d.Name,
            //                   province = p.Name
            //               }).SingleOrDefault();
            //VMCartItem.UserInfo.DetailAddress = address.detail;
            //VMCartItem.UserInfo.CommuneId = address.commune;
            //VMCartItem.UserInfo.DistricstId = address.distric;
            //VMCartItem.UserInfo.ProvinceId = address.province;

            AddressInfo addressInfo = db.AddressInfoes.Find(LoginUser.Address);
            if (addressInfo != null)
            {
                VMCartItem.UserInfo.ProvinceId = addressInfo.ProvinceId;
                VMCartItem.UserInfo.DistricstId = addressInfo.DistrictId;
                VMCartItem.UserInfo.CommuneId = addressInfo.CommuneId;
                VMCartItem.UserInfo.DetailAddress = addressInfo.Detail;
            }

            if (TempData[ERR_DATA] != null && !String.IsNullOrEmpty(TempData[ERR_DATA].ToString()))
            {
                ViewBag.ErrMsg = TempData[ERR_DATA].ToString();
            }

            return View(VMCartItem);
        }

        [HttpPost]
        public ActionResult UpdateCart(int id, int quantity)
        {
            if (LoginUser == null || LoginUser.Id < 1)
            {
                return Json(new { err = 1, msg = "Chua dang nhap!" }, JsonRequestBehavior.AllowGet);
            }

            LoadSessionCart();

            string priceItem = string.Empty;
            if (quantity > 0 && VMCartItem.CartItems.Count > 0)
            {
                if (VMCartItem.CartItems.Exists(x => x.ProductId == id))
                {
                    foreach (var item in VMCartItem.CartItems)
                    {
                        Product product = db.Products.Find(item.ProductId);
                        if (product == null)
                        {
                            continue;
                        }

                        // cập nhật số lượng
                        if (item.ProductId == id)
                        {
                            item.Quantity = quantity;
                            double price = (double)(product.PriceSale == null ? (quantity * product.Price) : (quantity * product.PriceSale));
                            priceItem = FormatString.FormatMoneyVND(price);
                        }
                    }
                }
            }
            SaveSession(SessionName.Cart, VMCartItem);
            return Json(new { err = 0, msg = new { price = priceItem, totalPrice = VMCartItem.TotalPrice } }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteItemCart(int id)
        {
            if (LoginUser == null || LoginUser.Id < 1)
            {
                return Json(new { err = 1, msg = "Chua dang nhap!" }, JsonRequestBehavior.AllowGet);
            }

            LoadSessionCart();
            if (VMCartItem.CartItems.Count > 0)
            {
                VMCartItem.CartItems.RemoveAll(x => x.ProductId == id);
            }

            SaveSession(SessionName.Cart, VMCartItem);
            return Json(new { err = 0, msg = new { totalPrice = VMCartItem.TotalPrice } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Checkout(VMCartItem model)
        {
            if (!ModelState.IsValid)
            {
                TempData[ERR_DATA] = "Vui lòng điền đầy đủ thông tin";
                return RedirectToAction("Details");
            }

            LoadSessionUser();
            if (LoginUser == null || LoginUser.Id < 1)
            {
                return RedirectToAction("Login", "Auth");
            }

            LoadSessionCart();
            if (VMCartItem.CartItems.Count > 0)
            {
                string err = string.Empty;
                foreach (var item in VMCartItem.CartItems)
                {
                    Product product = db.Products.Find(item.ProductId);
                    if(item.Quantity > product.Number)
                    {
                        err += String.Format("Sản phẩm '{0} chỉ còn {1} sản phẩm.<br>", product.Name, product.Number);
                    }
                }

                if(!string.IsNullOrEmpty(err))
                {
                    TempData[ERR_DATA] = err;
                    return RedirectToAction("Details");
                }

                AddressInfo addressInfo = new AddressInfo
                {
                    ProvinceId = model.UserInfo.ProvinceId,
                    DistrictId = model.UserInfo.DistricstId,
                    CommuneId = model.UserInfo.CommuneId,
                    Detail = model.UserInfo.DetailAddress
                };
                db.AddressInfoes.Add(addressInfo);
                db.SaveChanges();

                Order order = new Order
                {
                    CustomerName = model.UserInfo.Name,
                    CustomerEmail = model.UserInfo.Email,
                    CustomerPhone = model.UserInfo.Phone,
                    CustomerAddress = addressInfo.Id,
                    Status = OrderStatus.Pending,
                    Description = model.Description,
                    Created_by = LoginUser.Id,
                    Created_at = DateTime.Now,
                    Updated_at = DateTime.Now
                };

                db.Orders.Add(order);
                db.SaveChanges();

                List<OrderDetail> lstOrderDetail = new List<OrderDetail>();

                foreach (var item in VMCartItem.CartItems)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Created_at = DateTime.Now,
                        Updated_at = DateTime.Now
                    };
                    lstOrderDetail.Add(orderDetail);
                }
                if (lstOrderDetail.Count > 0)
                {
                    db.OrderDetails.AddRange(lstOrderDetail);
                    db.SaveChanges();
                }
                RemoveSession(SessionName.Cart);
            }

            TempData["order_success"] = "Đặt hàng thành công. Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Orders()
        {
            #region v1
            //var result = (from order in db.Orders.Where(a => a.Created_by == LoginUser.Id)
            //              join odt in db.OrderDetails on order.Id equals odt.OrderId
            //              join pd in db.Products on odt.ProductId equals pd.Id
            //              select new
            //              {
            //                  productId = pd.Id,
            //                  productName = pd.Name,
            //                  productImg = pd.Img,
            //                  quantity = odt.Quantity,
            //                  price = pd.PriceSale ?? pd.Price,
            //                  orderId = order.Id,
            //                  status = order.Status,
            //              }).GroupBy(a => a.orderId).ToList();

            //ViewBag.Data = result;
            //VMCartOrder model = new VMCartOrder();
            //foreach (var item in result)
            //{
            //    LstItemOrders lstItemOrders = new LstItemOrders();
            //    foreach (var p in item)
            //    {
            //        ItemOrder itemOrder = new ItemOrder
            //        {
            //            ProductId = p.productId,
            //            ProductName = p.productName,
            //            Img = p.productImg,
            //            PriceDouble = p.price,
            //            Quantity = p.quantity
            //        };
            //        lstItemOrders.ItemOrders.Add(itemOrder);
            //    }
            //    model.AllItemOrder.AddRange(lstItemOrders);
            //}

            #endregion

            // lay danh sach order theo user login
            List<Order> order = db.Orders.Where(o => o.Created_by == LoginUser.Id).ToList();

            // danh sach id order
            List<int> lstOrderId = order.Select(a => a.Id).ToList();

            // lay danh sach order detail theo danh sach id order
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
                o.TotalPrice = FormatString.FormatMoneyVND(price);
                o.StatusOrder = o.Status.GetEnumDisplayName();
            }

            return View(order);
        }

        public ActionResult OrderDetail(int id)
        {
            var orderDetail = (from o in db.Orders
                               join od in db.OrderDetails on o.Id equals od.OrderId
                               where o.Id == id && o.Created_by == LoginUser.Id
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

            return View(model);
        }

        public ActionResult CancelOrder(int id, string description)
        {
            if (String.IsNullOrEmpty(description))
            {
                return Json(new { err = 1, msg = "Vui lòng nhập Ghi chú" }, JsonRequestBehavior.AllowGet);
            }
            Order order = db.Orders.Find(id);

            order.Status = OrderStatus.Cancel;
            order.Description = description;
            order.Updated_at = DateTime.Now;

            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new
            {
                err = 0,
                msg = new
                {
                    status = Enums.OrderStatus.Cancel.GetEnumDisplayName(),
                    noti = "Hủy đơn hàng thành công"
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}