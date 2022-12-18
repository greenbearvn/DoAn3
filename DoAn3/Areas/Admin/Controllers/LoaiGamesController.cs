using DoAn3.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace DoAn3.Areas.Admin.Controllers
{
    public class LoaiGamesController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/LoaiGames
        public async Task<ActionResult> Index()
        {
            return View(await db.LoaiGame.ToListAsync());
        }


        public JsonResult GetDataLoaiGame()
        {
            var query = (from loaigame in db.LoaiGame select new {loaigame.TenLoai,loaigame.MaLoai,loaigame.MoTa }).ToList();
            return Json(query,JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/LoaiGames/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiGame loaiGame = await db.LoaiGame.FindAsync(id);
            if (loaiGame == null)
            {
                return HttpNotFound();
            }
            return View(loaiGame);
        }

        // GET: Admin/LoaiGames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLG([Bind(Include = "MaLoai,TenLoai,MoTa")] LoaiGame loaiGame)
        {
            loaiGame.TenLoai = loaiGame.TenLoai;
            loaiGame.MoTa = loaiGame.MoTa;

            if (ModelState.IsValid)
            {
                try
                {
                    db.LoaiGame.Add(loaiGame);
                    db.SaveChanges();
                    return Json(new { msg = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { msg = false }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { msg = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/LoaiGames/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiGame loaiGame = await db.LoaiGame.FindAsync(id);
            if (loaiGame == null)
            {
                return HttpNotFound();
            }
            return View(loaiGame);
        }



        public JsonResult EditDetailData(int id)
        {
            var query = (from lg in db.LoaiGame where lg.MaLoai == id select new {lg.MaLoai,lg.TenLoai,lg.MoTa}).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult EditLoaiGame([Bind(Include = "MaLoai,TenLoai,MoTa")] LoaiGame loaiGame)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loaigame = (from lg in db.LoaiGame where lg.MaLoai == loaiGame.MaLoai select lg).FirstOrDefault();
                    loaigame.MaLoai = loaiGame.MaLoai;
                    loaigame.TenLoai = loaiGame.TenLoai;
                    loaigame.MoTa = loaiGame.MoTa;
                    db.SaveChanges();
                    return Json(new { msg = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { msg = false }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { msg = false }, JsonRequestBehavior.AllowGet);
        }








        // GET: Admin/LoaiGames/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiGame loaiGame = await db.LoaiGame.FindAsync(id);
            if (loaiGame == null)
            {
                return HttpNotFound();
            }
            return View(loaiGame);
        }

        
     
        public JsonResult DeleteConfirmed(int id)
        {
            var query = (from lg in db.LoaiGame where lg.MaLoai == id select lg).FirstOrDefault();
            db.LoaiGame.Remove(query);
            db.SaveChanges();

            var data = (from loaigame in db.LoaiGame select new { loaigame.TenLoai, loaigame.MaLoai, loaigame.MoTa }).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
