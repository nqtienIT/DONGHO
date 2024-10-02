using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CuaHangDongHo.Areas.Admin.Filters;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using CuaHangDongHo.Utilities;
using static CuaHangDongHo.Defines.Enums;

namespace CuaHangDongHo.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        // GET: Admin/Products
        [AdminFilter]
        public ActionResult Index()
        {
            if (TempData[ERR_DATA] != null && !String.IsNullOrEmpty(TempData[ERR_DATA].ToString()))
            {
                ViewBag.ErrMsg = TempData[ERR_DATA].ToString();
            }

            if (TempData[SUCCESS_DATA] != null && !String.IsNullOrEmpty(TempData[SUCCESS_DATA].ToString()))
            {
                ViewBag.SuccessMsg = TempData[SUCCESS_DATA].ToString();
            }

            List<Product> lstProducts = GetListProducts();

            return View(lstProducts);
        }

        // GET: Admin/Products/Details/5
        [AdminFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                TempData[ERR_DATA] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index");
            }

            product.BrandName = db.Brands.Find(product.BrandId).Name;
            product.PriceVND = FormatString.FormatMoneyVND(product.Price);
            product.PriceSaleVND = FormatString.FormatMoneyVND(product.PriceSale ?? 0);
            //product.StatusProduct = ((int)product.Status == 0) ? "Đang kinh doanh" : "Đã ngừng kinh doanh";

            return View(product);
        }

        // GET: Admin/Products/Create
        [AdminFilter]
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AdminFilter]
        public ActionResult Create(HttpPostedFileBase img, IEnumerable<HttpPostedFileBase> lstImg, Product product)
        {
            if (img == null)
            {
                ModelState.AddModelError("img", "Vui lòng chọn ảnh");
            }

            if (lstImg == null)
            {
                ModelState.AddModelError("lstImg", "Vui lòng chọn ảnh");
            }

            if (ModelState.IsValid)
            {
                // upload image
                if (img != null)
                {
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(img.FileName);
                    string extension = System.IO.Path.GetExtension(img.FileName);
                    fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/products/"), fileName);
                    img.SaveAs(path);
                    product.Img = fileName;
                }

                if (lstImg != null)
                {
                    int lastIdProduct = db.Products.OrderByDescending(u => u.Id).FirstOrDefault().Id;
                    foreach (var item in lstImg)
                    {
                        if (item != null)
                        {
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(item.FileName);
                            string extension = System.IO.Path.GetExtension(item.FileName);
                            fileName = String.Format("{0}_{1}{2}", fileName, DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
                            string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/products/"), fileName);
                            item.SaveAs(path);

                            Picture picture = new Picture
                            {
                                ProductId = lastIdProduct + 1,
                                Name = fileName,
                                Created_at = DateTime.Now,
                                Updated_at = DateTime.Now
                            };
                            db.Pictures.Add(picture);
                        }
                    }
                }

                product.Created_at = DateTime.Now;
                product.Updated_at = DateTime.Now;

                db.Products.Add(product);
                db.SaveChanges();

                TempData[SUCCESS_DATA] = "Thêm thành công!";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Admin/Products/Edit/5
        [AdminFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                TempData[ERR_DATA] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AdminFilter]
        public ActionResult Edit(HttpPostedFileBase NewImg, Product product)
        {
            Product entry = db.Products.Find(product.Id);
            if (entry == null)
            {
                TempData[ERR_DATA] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                // upload image
                if (NewImg != null)
                {
                    // lay ten file
                    string fileName = System.IO.Path.GetFileName(NewImg.FileName);
                    // path luu file
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/img/products/"), fileName);
                    NewImg.SaveAs(path);

                    // xoa file cu
                    string oldFileName = System.IO.Path.Combine(Server.MapPath("~/Content/img/products/"), entry.Img);
                    if (System.IO.File.Exists(oldFileName))
                    {
                        System.IO.File.Delete(oldFileName);
                    }

                    product.Img = fileName;
                }

                product.Created_at = entry.Created_at;
                product.Updated_at = DateTime.Now;

                ((IObjectContextAdapter)db).ObjectContext.Detach(entry);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Get: Admin/Products/Delete/5
        [AdminFilter]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                TempData[ERR_DATA] = "Sản phẩm không tồn tại!";
            }
            else
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData[SUCCESS_DATA] = "Xóa thành công!";
            }

            return RedirectToAction("Index");
        }

        public List<Product> GetListProducts()
        {
            List<Product> lstProducts = new List<Product>();

            lstProducts = db.Products.ToList().Select(p => new Product
            {
                Id = p.Id,
                CateName = db.Categories.Where(a => a.Id == p.Id).Select(a => a.Name).FirstOrDefault(),
                Name = p.Name,
                Slug = p.Slug,
                Img = p.Img,
                Number = p.Number,
                PriceVND = FormatString.FormatMoneyVND(p.Price),
                PriceSaleVND = p.PriceSale == null ? "0" : FormatString.FormatMoneyVND((double)p.PriceSale),
                StatusProduct = p.Status.GetEnumDisplayName()

            }).ToList();

            return lstProducts;
        }

        //public static SelectList GetListStatusProduct()
        //{
        //    var EntityState = new SelectList(Enum.GetValues(typeof(StatusProduct)).Cast<StatusProduct>().Select(v => new SelectListItem
        //    {
        //        Text = GetDisplayName(v),
        //        Value = ((int)v).ToString(),
        //        Selected = (int)v == 0
        //    }).ToList(), "Value", "Text");

        //    return EntityState;
        //}

        //private static string GetDisplayName(object value)
        //{
        //    var type = value.GetType();
        //    if (!type.IsEnum)
        //    {
        //        throw new ArgumentException(string.Format("Type {0} is not an enum", type));
        //    }

        //    // Get the enum field.
        //    var field = type.GetField(value.ToString());
        //    if (field == null)
        //    {
        //        return value.ToString();
        //    }

        //    // Gets the value of the Name property on the DisplayAttribute, this can be null.
        //    var attributes = field.GetCustomAttribute<DisplayAttribute>();
        //    return attributes != null ? attributes.Name : value.ToString();
        //}


        [HttpPost]
        public ActionResult AjaxCreateSlugName(string str)
        {
            SlugStr slugStr = new SlugStr();
            str = slugStr.CreateSlug(str, true);
            return Json(new { result = str });
        }
    }
}
