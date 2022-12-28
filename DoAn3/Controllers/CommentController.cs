using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn3.Models;

namespace DoAn3.Controllers
{
    public class CommentController : Controller
    {

        DoAn3Entities db = new DoAn3Entities();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult CreateComment(BinhLuan comment)
        {
            if (comment != null)
            {
                db.BinhLuan.Add(comment);
                db.SaveChanges();

                var allComment = (from cm in db.BinhLuan join us in db.User on cm.UserID equals us.UserID where cm.MaGame == comment.MaGame select new { cm.MaNPH, cm.MaGame, cm.UserID, us.UserName, cm.NoiDung, cm.ThoiGian }).ToList();
                return Json(allComment,JsonRequestBehavior.AllowGet);
            }
            else
            {
                var allComment = (from cm in db.BinhLuan join us in db.User on cm.UserID equals us.UserID where cm.MaGame == comment.MaGame select new { cm.MaNPH, cm.MaGame, cm.UserID, us.UserName, cm.NoiDung, cm.ThoiGian }).ToList();

                return Json(allComment, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCommentByGameID(int id)
        {
            var allComment = (from cm in db.BinhLuan join us in db.User on cm.UserID equals us.UserID where cm.MaGame == id select new {cm.MaNPH,cm.MaGame,cm.UserID, us.UserName, cm.NoiDung, cm.ThoiGian }).ToList();

            return Json(allComment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveComment(int? magame, int? mabinhluan, int? userID)
        {
            var check = (from cm in db.BinhLuan where cm.MaNPH == mabinhluan && cm.MaGame == magame && cm.UserID == userID select cm).FirstOrDefault();

            var allComment = (from cm in db.BinhLuan join us in db.User on cm.UserID equals us.UserID where cm.MaGame == magame select new { cm.MaNPH, cm.MaGame, cm.UserID, us.UserName, cm.NoiDung, cm.ThoiGian }).ToList();
            if (userID == check.UserID)
            {
                var data = (from cm in db.BinhLuan where cm.MaNPH == mabinhluan && cm.UserID == userID && cm.MaGame == magame select cm).FirstOrDefault();
                db.BinhLuan.Remove(data);
                db.SaveChanges();
                var lstComment = (from cm in db.BinhLuan join us in db.User on cm.UserID equals us.UserID where cm.MaGame == magame select new { cm.MaNPH, cm.MaGame, cm.UserID, us.UserName, cm.NoiDung, cm.ThoiGian }).ToList();
                return Json(lstComment, JsonRequestBehavior.AllowGet);
            }
            return Json(allComment, JsonRequestBehavior.AllowGet);

        }


        public bool CheckComment(int? magame, int? mabinhluan, int? userID)
        {
            if(userID != null && Session["UserID"] != null)
            {
                var user = Session["UserID"].ToString();
                var check = (from cm in db.BinhLuan where cm.MaNPH == mabinhluan && cm.MaGame == magame && cm.UserID == userID select cm).FirstOrDefault();
                if(check != null && userID == int.Parse(user))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }
    }
}