using CuaHangDongHo.Defines;
using CuaHangDongHo.Helpers;
using CuaHangDongHo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CuaHangDongHo.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Details(string Id)
        {
            //string test = "day-la-san-pham-123-cua-a-p123";

            VMDetailProduct result = new VMDetailProduct();

            if (String.IsNullOrEmpty(Id))
            {
                return View(result);
            }

            string idStr = Id.Substring(Id.LastIndexOf("-p") + 2);
            int id = Convert.ToInt32(idStr);

            var slug = Id.Substring(0, Id.LastIndexOf("-p"));

            Product product = db.Products.Find(id);
            if (product != null)
            {
                if (product.Slug != slug)
                {
                    product = new Product();
                }

                result = GetInfoProduct(product);

                ViewBag.Comments = GetComments(id);
            }

            return View(result);
        }

        public VMDetailProduct GetInfoProduct(Product product)
        {
            VMDetailProduct result = new VMDetailProduct
            {
                Id = product.Id,
                NameProduct = product.Name,
                DescriptionProduct = product.Description,
                PriceVND = FormatString.FormatMoneyVND(product.Price),
                PriceSaleVND = product.PriceSale == null ? null : FormatString.FormatMoneyVND((double)product.PriceSale),
                DetailProduct = product.Detail,
                MainPicture = product.Img,
                WatchesType = product.Type == null ? String.Empty : product.Type.GetEnumDisplayName(),
                WatchesStrap = product.Strap == null ? String.Empty : product.Strap.GetEnumDisplayName(),
            };

            List<Picture> pictures = db.Pictures.Where(p => p.ProductId == product.Id).ToList();

            if (pictures.Count > 0)
            {
                foreach (var item in pictures)
                {
                    result.Pictures.Add(item.Name);
                }
            }

            var infoBrand = db.Brands.Where(b => b.Id == product.BrandId).FirstOrDefault();
            if (infoBrand != null)
            {
                result.DescriptionBrand = infoBrand.Description;
                result.NameBrand = infoBrand.Name;
                result.DetailBrand = infoBrand.Detail;
            }

            return result;
        }

        public List<Comment> GetComments(int prodcutId)
        {
            var data = (from c in db.Comments
                        join u in db.Users on c.Created_by equals u.Id
                        where c.Type == Enums.CommentType.Product &&
                        c.ProductId == prodcutId &&
                        c.Status == Enums.StatusComment.Accept
                        select new
                        {
                            id = c.Id,
                            fullName = u.FullName,
                            detail = c.Detail,
                            created_at = c.Created_at
                        }).OrderBy(c => c.id).ToList();

            List<Comment> comments = new List<Comment>();
            foreach (var item in data)
            {
                Comment comment = new Comment
                {
                    Detail = item.detail,
                    Created_at_Format = DateTimeMgrs.FormatDateTimeVNese((DateTime)item.created_at),
                    UserComment = new UserComment
                    {
                        UserName = item.fullName
                    }
                };
                comments.Add(comment);
            }

            return comments;
        }
    }
}