using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn3.Models;

namespace DoAn3.Controllers
{
    public class NewsFrontController : Controller
    {
        DoAn3Entities db = new DoAn3Entities();
        // GET: News
        public ActionResult Index()
        {

            
            return View();
        }

        public JsonResult ListNews()
        {
            var query = (from n in db.New select new {n.NewsID,n.Title,n.Banner,n.Content,n.PublicDate}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListMachine()
        {
            var query = (from lm in db.LoaiMay select new {lm.MaMay, lm.TenMay}).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListGame()
        {
            var query = (from game in db.Game select new { game.MaGame,game.TenGame, game.AnhGame ,game.GiaTien}).Take(5).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Detail(int id)
        {
            return View();
        }


        public JsonResult DetaiNews(int id)
        {
            var query = (from post in db.New 
                         join us in db.User on post.UserID equals us.UserID 
                         join loaigame in db.LoaiGame on post.MaLoai equals loaigame.MaLoai where post.NewsID == id select new {post.NewsID,post.Title,us.UserName,post.Content,post.Banner,post.PublicDate, loaigame.TenLoai}).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RecommendPost()
        {
            var query = (from post in db.New select new { post.NewsID, post.Title, post.Banner,post.PublicDate }).Take(3).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PageSideBar()
        {
            return PartialView();
        }

        public JsonResult GetAllGameType()
        {
            var data = (from lg in db.LoaiGame select new { lg.MaLoai, lg.TenLoai }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);    
        }
    }
}