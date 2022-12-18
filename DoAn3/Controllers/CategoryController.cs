using DoAn3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DoAn3.Controllers
{
    public class CategoryController : Controller
    {
        DoAn3Entities db = new DoAn3Entities();

        // GET: Category
        public ActionResult Detail(int id)
        {

            var queryData = (from game in db.Game where game.MaMay == id select game).ToList();
            var loaiMayTT = (from loaimay in db.LoaiMay where loaimay.MaMay == id select loaimay.TenMay).FirstOrDefault();
            
            ViewBag.loaiMayTT = loaiMayTT;
            return View(queryData);
        }


        public JsonResult CategoryTitle(int id)
        {
            var data = (from cat in db.LoaiMay where cat.MaMay == id select new {cat.MaMay,cat.TenMay}).FirstOrDefault();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult LaySPTheoLoaiMay(int id)
        {

            var query = (from game in db.Game where game.MaMay == id select new { game.MaGame, game.TenGame, game.AnhGame, game.GiaTien, game.Views }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult FilterGames()
        {
            return PartialView();
        }

        public JsonResult GetBrand()
        {
            var data = (from brand in db.NhaPhatHanh select new { brand.MaNPH, brand.TenNPH}).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTypeGame()
        {
            var data = (from typegame in db.LoaiGame select new { typegame.MaLoai, typegame.TenLoai }).ToList();

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult FitlerGames(int? mamay, int? manph, int? maloai, int? giaThap, int? giaCao)
        {
            if (mamay != null && manph != null && maloai == null && giaThap ==null && giaCao == null)
            {
                var data = (from game in db.Game where game.MaMay == mamay && game.MaNPH == manph select new { game.MaGame, game.TenGame, game.AnhGame, game.GiaTien, game.Views }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if(mamay != null && manph != null && maloai != null && giaThap == null && giaCao == null)
            {
                var data = (from game in db.Game where game.MaMay == mamay && game.MaNPH == manph && game.MaLoai == maloai select new { game.MaGame, game.TenGame, game.AnhGame, game.GiaTien, game.Views }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else if (mamay != null && manph != null && maloai != null && giaThap != null && giaCao != null)
            {
                var data = (from game in db.Game where game.MaMay == mamay && game.MaNPH == manph && game.MaLoai == maloai && giaThap <= game.GiaTien && game.GiaTien <= giaCao select new { game.MaGame, game.TenGame, game.AnhGame, game.GiaTien, game.Views }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var query = (from game in db.Game where game.MaMay == mamay select new { game.MaGame, game.TenGame, game.AnhGame, game.GiaTien, game.Views }).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }
    }
}