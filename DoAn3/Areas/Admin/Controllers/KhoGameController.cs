using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn3.Models;

namespace DoAn3.Areas.Admin.Controllers
{
    public class KhoGameController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/KhoGame
        public async Task<ActionResult> Index()
        {
            var khoGames = db.KhoGame.Include(k => k.User);
            return View(await khoGames.ToListAsync());
        }

        // GET: Admin/KhoGame/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoGame khoGame = await db.KhoGame.FindAsync(id);
            if (khoGame == null)
            {
                return HttpNotFound();
            }
            return View(khoGame);
        }

        // GET: Admin/KhoGame/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        // POST: Admin/KhoGame/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaKhoGame,UserID")] KhoGame khoGame)
        {
            if (ModelState.IsValid)
            {
                db.KhoGame.Add(khoGame);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", khoGame.UserID);
            return View(khoGame);
        }

        // GET: Admin/KhoGame/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoGame khoGame = await db.KhoGame.FindAsync(id);
            if (khoGame == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", khoGame.UserID);
            return View(khoGame);
        }

        // POST: Admin/KhoGame/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaKhoGame,UserID")] KhoGame khoGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khoGame).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.User, "UserID", "UserName", khoGame.UserID);
            return View(khoGame);
        }

        // GET: Admin/KhoGame/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoGame khoGame = await db.KhoGame.FindAsync(id);
            if (khoGame == null)
            {
                return HttpNotFound();
            }
            return View(khoGame);
        }

        // POST: Admin/KhoGame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            KhoGame khoGame = await db.KhoGame.FindAsync(id);
            db.KhoGame.Remove(khoGame);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetDataKhoGame()
        {
            var data = (from kg in db.KhoGame 
                        join us in db.User on kg.UserID equals us.UserID
                        join kh in db.KhachHang on us.UserID equals kh.MaKH
                        select new
                        {
                            kg.MaKhoGame,
                            us.UserName,
                            kh.TenKH,
                        }).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataDetail(int id)
        {
            var data = (from kg in db.KhoGame
                        join us in db.User on kg.UserID equals us.UserID
                        join kh in db.KhachHang on us.UserID equals kh.MaKH
                        where kg.MaKhoGame == id
                        select new
                        {
                            kg.MaKhoGame,
                            kg.UserID,
                            us.UserName,
                            kh.TenKH,
                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataDetailGameStore(int id)
        {
            var data = (from ctkg in db.ChiTietKhoGame 
                        join game in db.Game on ctkg.MaGame equals game.MaGame
                        
                        where ctkg.MaKhoGame == id 

                        select new
                        {
                            ctkg.MaKhoGame,
                            game.MaGame,
                            game.TenGame,
                            ctkg.NgayThem,
                            ctkg.TinhTrang
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCart(int id)
        {
            var games = db.Game.FirstOrDefault(s => s.MaGame == id);

            if (Session["cartAdmin"] != null)
            {
                List<ChiTietKhoGame> cart = (List<ChiTietKhoGame>)Session["cartAdmin"];
                var kt = cart.FirstOrDefault(s => s.MaGame == id);
                if (kt == null)
                {
                    ChiTietKhoGame game = new ChiTietKhoGame() { MaGame = id, Game = games };
                    cart.Add(game);
                }
                Session["cartAdmin"] = cart;
            }
            else
            {
                List<ChiTietKhoGame> cart = new List<ChiTietKhoGame>();
                ChiTietKhoGame game = new ChiTietKhoGame() { MaGame = id, Game = games };
                cart.Add(game);
                Session["cartAdmin"] = cart;
            }

            return RedirectToAction("Create", "KhoGame");
        }

        public PartialViewResult ViewCart()
        {
            {
                List<ChiTietKhoGame> cart = (List<ChiTietKhoGame>)Session["cartAdmin"];



                List<ChiTietKhoGame> ds = new List<ChiTietKhoGame>();
                if (cart != null)
                {
                    foreach (var c in cart)
                    {
                        ds.Add(c);
                    }
                }


                return PartialView(ds);
            }
        }
        public ActionResult RemoveCartItem(int id)
        {

            List<ChiTietKhoGame> cart = (List<ChiTietKhoGame>)Session["cartAdmin"];
            if (cart != null)
            {

                cart.RemoveAll(s => s.MaGame == id);
                Session["cartAdmin"] = cart;

            }
            return RedirectToAction("Create", "KhoGame");
        }

        public void RemoveSession()
        {
            List<ChiTietKhoGame> lstCTKG = (List<ChiTietKhoGame>)Session["cartAdmin"];
            List<ChiTietKhoGame> editctkg = (List<ChiTietKhoGame>)Session["cartEditAdmin"];
         
            if (lstCTKG!=null || editctkg!=null)
            {

                Session["cartAdmin"] = null;
                Session["cartEditAdmin"] = null;

            }
        }
        public bool AddEditCart(int id, int MaKG)
        {
            var games = db.Game.FirstOrDefault(s => s.MaGame == id);

            if (Session["cartEditAdmin"] != null)
            {
                List<ChiTietKhoGame> cart = (List<ChiTietKhoGame>)Session["cartEditAdmin"];
                var kt = cart.FirstOrDefault(s => s.MaGame == id);
                if (kt == null)
                {
                    ChiTietKhoGame game = new ChiTietKhoGame() { MaKhoGame = MaKG, MaGame = id, Game = games };
                    cart.Add(game);
                }
                Session["cartEditAdmin"] = cart;

                return true;
            }
            else
            {
                List<ChiTietKhoGame> cart = new List<ChiTietKhoGame>();
                ChiTietKhoGame game = new ChiTietKhoGame() { MaKhoGame = MaKG, MaGame = id, Game = games };
                cart.Add(game);
                Session["cartEditAdmin"] = cart;

                return true;
            }
        }

        public ActionResult RemoveCartEditItem(int id, int mkg)
        {

            List<ChiTietKhoGame> cart = (List<ChiTietKhoGame>)Session["cartEditAdmin"];
            if (cart != null)
            {

                cart.RemoveAll(s => s.MaGame == id);
                Session["cartEditAdmin"] = cart;
                return RedirectToAction("Edit", "KhoGame", new { id = mkg });

            }
            else
            {
                return RedirectToAction("Edit", "KhoGame", new { id = mkg });
            }

        }

        public PartialViewResult ViewEditCart()
        {
            {
                List<ChiTietKhoGame> cart = (List<ChiTietKhoGame>)Session["cartEditAdmin"];



                List<ChiTietKhoGame> ds = new List<ChiTietKhoGame>();
                if (cart != null)
                {
                    foreach (var c in cart)
                    {
                        ds.Add(c);
                    }
                }
                return PartialView(ds);
            }
        }

        //public bool CreateKG(KhoGame kg)
        //{
        //    if (kg != null)
        //    {
                


        //        db.KhoGame.Add(kg);
        //        db.SaveChanges();


        //        RemoveSession();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}


        public bool addCTKG(List<ChiTietKhoGame> litsCart,int usid)
        {

            KhoGame kg = new KhoGame();
            kg.UserID = usid;

            db.KhoGame.Add(kg);

            if (litsCart != null)
            {
                foreach (var item in litsCart)
                {
                    int intOrderId = kg.MaKhoGame;

                    //Chi tiet don hang
                    ChiTietKhoGame ctkg = new ChiTietKhoGame();
                    ctkg.MaKhoGame = intOrderId;
                    ctkg.MaGame = item.MaGame;
                    DateTime today = DateTime.Today;
                    ctkg.NgayThem = today;
                    ctkg.TinhTrang = false;
                    db.ChiTietKhoGame.Add(ctkg);
                    db.SaveChanges();

                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditKG(List<ChiTietKhoGame> litsCart, int makhogame,int userid)
        {
            if (makhogame > 0)
            {
                var khogame = (from khoGame in db.KhoGame where khoGame.MaKhoGame == makhogame select khoGame).FirstOrDefault();
                khogame.UserID = userid;
                db.SaveChanges();

                if (litsCart != null)
                {
                    foreach (var item in litsCart)
                    {
                        int intOrderId = makhogame;
                        //Chi tiet don hang
                        ChiTietKhoGame ctkg = new ChiTietKhoGame();
                        ctkg.MaKhoGame = intOrderId;
                        ctkg.MaGame = item.MaGame;
                        DateTime today = DateTime.Today;
                        ctkg.NgayThem = today;
                        ctkg.TinhTrang = false;
                        db.ChiTietKhoGame.Add(ctkg);
                        db.SaveChanges();

                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult RemoveGameStore(int? id)
        {
            if(id != null)
            {
                var khogame = (from kg in db.KhoGame where kg.MaKhoGame == id select kg).FirstOrDefault();
                if (khogame != null)
                {
                    db.KhoGame.Remove(khogame);
                    db.SaveChanges();
                    var data = (from kg in db.KhoGame
                                join us in db.User on kg.UserID equals us.UserID
                                join kh in db.KhachHang on us.UserID equals kh.MaKH
                                select new
                                {
                                    kg.MaKhoGame,
                                    us.UserName,
                                    kh.TenKH,
                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = (from kg in db.KhoGame
                                join us in db.User on kg.UserID equals us.UserID
                                join kh in db.KhachHang on us.UserID equals kh.MaKH
                                select new
                                {
                                    kg.MaKhoGame,
                                    us.UserName,
                                    kh.TenKH,
                                }).ToList();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                var data = (from kg in db.KhoGame
                            join us in db.User on kg.UserID equals us.UserID
                            join kh in db.KhachHang on us.UserID equals kh.MaKH
                            select new
                            {
                                kg.MaKhoGame,
                                us.UserName,
                                kh.TenKH,
                            }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
