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
    public class PostController : BaseController
    {
        // GET: Post
        public ActionResult Index()
        {
            List<Post> posts = db.Posts.OrderByDescending(p => p.Id).ToList();
            return View(posts);
        }

        public ActionResult Detail(int Id)
        {
            Post post = db.Posts.Find(Id);

            ViewBag.NextPost = db.Posts.Where(p => p.Id > Id).OrderBy(p => p.Id).FirstOrDefault();
            ViewBag.PrevPost = db.Posts.Where(p => p.Id < Id).OrderByDescending(p => p.Id).FirstOrDefault();
            var rand = new Random();
            ViewBag.RandomPost = db.Posts.Where(p => p.Id != Id).ToList()
                                        .Skip(rand.Next(0, db.Posts.Count()))
                                        .Take(1).FirstOrDefault();

            ViewBag.Comments = GetComments(Id);
            return View(post);
        }

        public List<Comment> GetComments(int postId)
        {
            var data = (from c in db.Comments
                        join u in db.Users on c.Created_by equals u.Id
                        where c.Type == Enums.CommentType.Post &&
                        c.PostId == postId &&
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