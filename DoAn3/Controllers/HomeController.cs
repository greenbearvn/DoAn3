using DoAn3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DoAn3.Controllers
{

    public class HomeController : Controller
    {
        DoAn3Entities db = new DoAn3Entities();
        Random ran = new Random();

        public ActionResult Index()
        {
            var dbSQL = db.Game.ToList();

            ViewBag.Category1 = (from loaiGame in db.LoaiGame where loaiGame.MaLoai == 1 select loaiGame.TenLoai).FirstOrDefault();

            ViewBag.Category2 = (from loaiGame in db.LoaiGame where loaiGame.MaLoai == 2 select loaiGame.TenLoai).FirstOrDefault();

            ViewBag.Category3 = (from loaiGame in db.LoaiGame where loaiGame.MaLoai == 3 select loaiGame.TenLoai).FirstOrDefault();

            return View(dbSQL);

        }

        public ActionResult getdb()
        {
            var dbSQL = db.Game.Select(s => new { s.MaGame, s.TenGame, s.GiaTien, s.AnhGame }).Take(12).ToList();
            return Json(dbSQL, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(int id)
        {
            var dbDetail = db.Game.FirstOrDefault(s => s.MaGame == id);

            ViewBag.Category = (from lg in db.LoaiGame where lg.MaLoai == id select lg.TenLoai).FirstOrDefault();

            ViewBag.LoaiMay = db.LoaiMay.Find(dbDetail.MaMay);
            dbDetail.Views += 1;
            db.SaveChanges();
            
            return View(dbDetail);
        }

        // get game from category
        // category 1
        public ActionResult CateGory1()
        {
            var listLCategory1 = (from sp in db.Game where sp.MaLoai == 1 select new { sp.TenGame, sp.GiaTien, sp.AnhGame }).Take(4).ToList();
            return Json(listLCategory1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CateGory2()
        {
            var listLCategory2 = (from sp in db.Game where sp.MaLoai == 2 select new { sp.TenGame, sp.GiaTien, sp.AnhGame }).Take(4).ToList();
            return Json(listLCategory2, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CateGory3()
        {
            var listLCategory3 = (from sp in db.Game where sp.MaLoai == 3 select new { sp.TenGame, sp.GiaTien ,sp.AnhGame }).Take(4).ToList();
            return Json(listLCategory3, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>

        public PartialViewResult LoaiMay()
        {
            var listLM = db.LoaiMay.ToList();


            return PartialView(listLM);
        }

        public ActionResult LaySPLoaiMay()
        {
            var loaiMay = (from sp in db.Game join lg in db.LoaiGame on sp.MaLoai equals lg.MaLoai select new { lg.TenLoai, sp.TenGame, sp.GiaTien, sp.AnhGame }).ToList();



            return Json(loaiMay, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LayLoaimay()
        {
            var loaiMay = (from lm in db.LoaiMay select new { lm.MaMay, lm.TenMay }).ToList();
            return Json(loaiMay, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LaySPTheoLoaiMay(int? id)
        { 
            if(id == null)
            {
                var query = (from game in db.Game orderby game.Views descending  select new { game.TenGame, game.GiaTien, game.AnhGame }).Take(6).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            else if(id == 0)
            {
                var query = (from game in db.Game  orderby game.MaGame descending select new { game.TenGame, game.GiaTien, game.AnhGame }).Take(6).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var query = (from game in db.Game where game.MaMay == id select new { game.TenGame, game.GiaTien, game.AnhGame }).Take(6).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            
            
        }


        public ActionResult AddCart(int id)
        {
            var games = db.Game.FirstOrDefault(s => s.MaGame == id);

            if (Session["cart"] != null)
            {
                List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cart"];
                var kt = cart.FirstOrDefault(s => s.MaGame == id);
                if (kt == null)
                {
                    ChiTietDonHang game = new ChiTietDonHang() { MaGame = id,  Game = games };
                    cart.Add(game);
                }
                Session["cart"] = cart;
                CountItemIncart();
            }
            else
            {
                List<ChiTietDonHang> cart = new List<ChiTietDonHang>();
                ChiTietDonHang game = new ChiTietDonHang() { MaGame = id, Game = games };
                cart.Add(game);
                Session["cart"] = cart;
                CountItemIncart();
            }

            return RedirectToAction("Index");
        }


        public void CountItemIncart()
        {
            int total = 0;
            List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
            {
                total += 1;
            }
            Session["totalItem"] = total;

        }

        public JsonResult DetailView(int id)
        {
            var query = (from game in db.Game 
                         join lg in db.LoaiGame 
                         on game.MaLoai equals lg.MaLoai
                         join lm in db.LoaiMay
                         on game.MaMay equals lm.MaMay
                         join nph in db.NhaPhatHanh
                         on game.MaNPH equals nph.MaNPH
                         where game.MaGame == id select new { game.MaGame, game.TenGame, game.AnhGame,game.MoTa, game.GiaTien,game.Views ,lg.MaLoai,lg.TenLoai,lm.TenMay,nph.TenNPH }).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CateProduct()
        {
           

            return View();
        }


        public JsonResult GetGameMostView()
        {
            var query = (from game in db.Game select new {game.MaGame,game.TenGame,game.AnhGame,game.GiaTien,game.Views}).Take(8).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RelateGame(int id)
        {
            var loaiMay = (from game in db.Game where game.MaGame == id select game).FirstOrDefault();
            int maloai = loaiMay.MaLoai.GetValueOrDefault();

            var data  = (from game in db.Game where game.MaLoai == maloai orderby game.MaGame descending select new {game.MaGame,game.TenGame,game.AnhGame,game.GiaTien}).Take(8).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult dataSearch()
        {
            var data = (from game in db.Game select new { game.MaGame, game.TenGame }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public int Like(Like likes) 
        {
            var checkLike = (from like in db.Like where like.UserID == likes.UserID && like.MaGame == likes.MaGame select like).FirstOrDefault();
            if (checkLike == null)
            {
                likes.TinhTrang = "Da like";
                db.Like.Add(likes);
                db.SaveChanges();
                var numberLike  = ( from like in db.Like where like.MaGame == likes.MaGame select like).ToList();
                var total = numberLike.Count();
                return total;
            }
            else
            {
                var lik = (from like in db.Like where like.UserID == likes.UserID && like.MaGame == likes.MaGame select like).FirstOrDefault();
                db.Like.Remove(lik);
                db.SaveChanges();
                var numberLike = (from like in db.Like where like.MaGame == likes.MaGame select like).ToList();
                var total = numberLike.Count();
                return total;
            }
        }

        public int LikeNumber(int id)
        {
            var data = (from like in db.Like where like.MaGame == id select like).ToList();
            var number = data.Count();
            return number;
        }



        public JsonResult getUser()
        {
            if(Session["UserID"] != null)
            {
                var user = Session["UserID"].ToString();
                var usss = int.Parse(user);
                var us = (from users in db.User where users.UserID == usss select new { users.UserID}).FirstOrDefault();
                return Json(us, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("");
            }
        }


        public bool CheckLike(int magame, int userid)
        {
            var data = (from like in db.Like where like.MaGame  == magame && like.UserID == userid select like).FirstOrDefault();
            if(data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
             
    }
}