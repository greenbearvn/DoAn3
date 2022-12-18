using DoAn3.Models;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace DoAn3.Areas.Admin.Controllers
{
    public class GamesController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/Games
        public async Task<ActionResult> Index()
        {
            var game = db.Game.Include(g => g.LoaiGame).Include(g => g.LoaiMay).Include(g => g.NhaPhatHanh);
            return View(await game.ToListAsync());
        }


        public JsonResult GameIndex()
        {
            var query = (from game in db.Game 
                         join lg in db.LoaiGame 
                         on game.MaLoai equals lg.MaLoai 
                         join lm in db.LoaiMay 
                         on game.MaMay equals lm.MaMay 
                         join nph in db.NhaPhatHanh 
                         on game.MaNPH equals nph.MaNPH
                         select new
                         {
                             game.MaGame,
                             game.TenGame,
                             game.AnhGame,
                             game.GiaTien,
                             game.TrangThai,
                             lg.TenLoai,
                             lm.TenMay,
                             nph.TenNPH,

                         }
                         ).ToList();

            return Json(query,JsonRequestBehavior.AllowGet);
        }


        public JsonResult GameDetail(int id)
        {
            var query = (from game in db.Game
                         join lg in db.LoaiGame
                         on game.MaLoai equals lg.MaLoai
                         join lm in db.LoaiMay
                         on game.MaMay equals lm.MaMay
                         join nph in db.NhaPhatHanh
                         on game.MaNPH equals nph.MaNPH
                         where game.MaGame == id
                         select new
                         {
                             game.MaGame,
                             game.TenGame,
                             game.AnhGame,
                             game.GiaTien,
                             game.TrangThai,
                             lg.TenLoai,
                             lm.TenMay,
                             nph.TenNPH,

                         }
                         ).FirstOrDefault();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Games/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = await db.Game.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Admin/Games/Create
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LoaiGame, "MaLoai", "TenLoai");
            ViewBag.MaMay = new SelectList(db.LoaiMay, "MaMay", "TenMay");
            ViewBag.MaNPH = new SelectList(db.NhaPhatHanh, "MaNPH", "TenNPH");
            return View();
        }

 
        public string CreateGame([Bind(Include = "MaGame,TenGame,AnhGame,MoTa,MaLoai,MaMay,MaNPH,SoLuong,GiaTien,Likes,Views,Favouries,TrangThai")] Game game)
        {
            if (game != null)
            {
               
                db.Game.Add(game);
                db.SaveChanges();
                return "Them game thanh cong";


            }
            return "tao khong thanh cong";


        }


        // GET: Admin/Games/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = await db.Game.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoai = new SelectList(db.LoaiGame, "MaLoai", "TenLoai", game.MaLoai);
            ViewBag.MaMay = new SelectList(db.LoaiMay, "MaMay", "TenMay", game.MaMay);
            ViewBag.MaNPH = new SelectList(db.NhaPhatHanh, "MaNPH", "TenNPH", game.MaNPH);
            return View(game);
        }

        

        public bool EditGame([Bind(Include = "MaGame,TenGame,AnhGame,MoTa,MaLoai,MaMay,MaNPH,SoLuong,GiaTien,Likes,Views,Favouries,TrangThai")] Game game)
        {


            if (ModelState.IsValid)
            {
                var Game = (from g in db.Game where g.MaGame == game.MaGame select g).FirstOrDefault();
                if (Game != null)
                {
                    Game.MaGame = game.MaGame;
                    Game.TenGame = game.TenGame;
                    Game.AnhGame = game.AnhGame;
                    Game.MoTa = game.MoTa;
                    Game.MaLoai = game.MaLoai;
                    Game.MaMay = game.MaMay;
                    Game.MaNPH = game.MaNPH;
                    Game.GiaTien = game.GiaTien;
                    Game.Views = game.Views;
                    Game.TrangThai = game.TrangThai;

                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public JsonResult DataEditGame(int id)
        {
            var query = (from game  in db.Game where game.MaGame == id select new{game.MaGame,game.TenGame,game.AnhGame,game.MoTa,game.MaLoai,game.MaMay,game.MaNPH,game.GiaTien,
            game.Views,game.TrangThai}).FirstOrDefault();

            return Json(query,JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Games/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = await db.Game.FindAsync(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Admin/Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Game game = await db.Game.FindAsync(id);
            db.Game.Remove(game);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult DeleteGame(Game games)
        {
            var data = (from game in db.Game
                        join lg in db.LoaiGame
                        on game.MaLoai equals lg.MaLoai
                        join lm in db.LoaiMay
                        on game.MaMay equals lm.MaMay
                        join nph in db.NhaPhatHanh
                        on game.MaNPH equals nph.MaNPH
                        select new
                        {
                            game.MaGame,
                            game.TenGame,
                            game.AnhGame,
                           
                            game.GiaTien,
                            game.TrangThai,
                            lg.TenLoai,
                            lm.TenMay,
                            nph.TenNPH,

                        }
                         ).ToList();
            if (games != null)
            {
                var query = (from game in db.Game where game.MaGame == games.MaGame select game).FirstOrDefault();
                if(query != null)
                {
                    db.Game.Remove(query);
                    db.SaveChanges();
                    var gamess = (from game in db.Game
                                join lg in db.LoaiGame
                                on game.MaLoai equals lg.MaLoai
                                join lm in db.LoaiMay
                                on game.MaMay equals lm.MaMay
                                join nph in db.NhaPhatHanh
                                on game.MaNPH equals nph.MaNPH
                                select new
                                {
                                    game.MaGame,
                                    game.TenGame,
                                    game.AnhGame,
                                    
                                    game.GiaTien,
                                    game.TrangThai,
                                    lg.TenLoai,
                                    lm.TenMay,
                                    nph.TenNPH,

                                }
                         ).ToList();

                    return Json(gamess, JsonRequestBehavior.AllowGet);
                   
                }
                else
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetLoaiMay()
        {
            var query = (from lm in db.LoaiMay select new {lm.MaMay,lm.TenMay}).ToList();
            return Json(query,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoaiGame()
        {
            var query = (from lg in db.LoaiGame select new { lg.MaLoai, lg.TenLoai }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNPH()
        {
            var query = (from nph in db.NhaPhatHanh select new { nph.MaNPH, nph.TenNPH }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataSearch(string name)
        {
            if(name != "undefined")
            {
                var query = (from game in db.Game
                             join lg in db.LoaiGame
                             on game.MaLoai equals lg.MaLoai
                             join lm in db.LoaiMay
                             on game.MaMay equals lm.MaMay
                             join nph in db.NhaPhatHanh
                             on game.MaNPH equals nph.MaNPH
                             where game.TenGame.Contains(name)
                             select new
                             {
                                 game.MaGame,
                                 game.TenGame,
                                 game.AnhGame,
                                
                                 game.GiaTien,
                                 game.TrangThai,
                                 lg.TenLoai,
                                 lm.TenMay,
                                 nph.TenNPH,

                             }
                         ).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var query = (from game in db.Game
                             join lg in db.LoaiGame
                             on game.MaLoai equals lg.MaLoai
                             join lm in db.LoaiMay
                             on game.MaMay equals lm.MaMay
                             join nph in db.NhaPhatHanh
                             on game.MaNPH equals nph.MaNPH
                             select new
                             {
                                 game.MaGame,
                                 game.TenGame,
                                 game.AnhGame,
                                 
                                 game.GiaTien,
                                 game.TrangThai,
                                 lg.TenLoai,
                                 lm.TenMay,
                                 nph.TenNPH,

                             }
                         ).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
