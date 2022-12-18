using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn3.Models;

namespace DoAn3.Controllers
{
    public class GameFavouritesController : Controller
    {
        DoAn3Entities db = new DoAn3Entities();
        // GET: GameFavourites
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetGameFavourite(int id)
        {
            var query = (from ctkg in db.ChiTietKhoGame
                         join kg in db.KhoGame on ctkg.MaKhoGame equals kg.MaKhoGame
                         join us in db.User on kg.UserID equals us.UserID
                         join game in db.Game on ctkg.MaGame equals game.MaGame
                         where  us.UserID == id
                         select new
                         {
                             ctkg.MaKhoGame,
                             ctkg.MaGame,
                             game.TenGame,
                             game.AnhGame,
                             ctkg.TinhTrang
                         }).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaKG(int id)
        {
            var query = (from ctkg in db.ChiTietKhoGame
                         join kg in db.KhoGame on ctkg.MaKhoGame equals kg.MaKhoGame
                         join us in db.User on kg.UserID equals us.UserID
                         join game in db.Game on ctkg.MaGame equals game.MaGame
                         where us.UserID == id
                         select new
                         {
                             ctkg.MaKhoGame,
                         }).FirstOrDefault();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProduct(int id)
        {
            var query = (from game in db.Game
                         where game.MaGame == id
                         select new
                         {
                             game.MaGame,
                             game.TenGame,
                             game.GiaTien,
                             game.MoTa,
                             game.AnhGame,
                            
                         });
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GameStore(int id)
        {
            return View();
        }

        public JsonResult CountGameStore(int id)
        {
            var query = (from ctkg in db.ChiTietKhoGame
                         join kg in db.KhoGame on ctkg.MaKhoGame equals kg.MaKhoGame
                         join us in db.User on kg.UserID equals us.UserID
                         join game in db.Game on ctkg.MaGame equals game.MaGame
                         where us.UserID == id
                         select new
                         {

                             ctkg.MaGame,
                             game.TenGame,
                             game.AnhGame,
                             ctkg.TinhTrang
                         }).ToList();
            var i = query.Count();

            return Json(i,JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult Username(int id)
        {
           var data = (from us in db.User where us.UserID == id select new {us.UserName}).FirstOrDefault();

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Tinhtrang(int magame, int makg)
        {
            var data = (from ctkg in db.ChiTietKhoGame
                        where ctkg.MaGame == magame && ctkg.MaKhoGame == makg
                        select new { ctkg.TinhTrang }).FirstOrDefault();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult Downloaded(int makg, int magame)
        {
            var check = (from ctkg in db.ChiTietKhoGame where ctkg.MaKhoGame == makg && ctkg.MaGame == magame && ctkg.TinhTrang == true select ctkg).FirstOrDefault();
            if (check != null)
            {
                var data = (from ctkg in db.ChiTietKhoGame
                            where ctkg.MaGame == magame && ctkg.MaKhoGame == makg
                            select new { ctkg.TinhTrang }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var item = (from ctkg in db.ChiTietKhoGame where ctkg.MaKhoGame == makg && ctkg.MaGame == magame select ctkg).FirstOrDefault();
                item.TinhTrang = true;
                db.SaveChanges();

                var data = (from ctkg in db.ChiTietKhoGame
                            where ctkg.MaGame == magame && ctkg.MaKhoGame == makg
                            select new { ctkg.TinhTrang }).FirstOrDefault();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }

        public bool DeleteGameFavourites(int magame, int makg , int userid)
        {
            var check = (from ctkg in db.ChiTietKhoGame where ctkg.MaKhoGame == makg && ctkg.MaGame == magame && ctkg.TinhTrang == true select ctkg).FirstOrDefault();
            if(check != null)
            {
                db.ChiTietKhoGame.Remove(check);
                db.SaveChanges();

                return true;

            }
            else
            {
                return false;
            }
        }
    }
}