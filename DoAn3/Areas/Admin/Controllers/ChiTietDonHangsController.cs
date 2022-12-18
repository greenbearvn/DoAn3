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
    public class ChiTietDonHangsController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/ChiTietDonHangs
        public async Task<ActionResult> Index()
        {
            var chiTietDonHangs = db.ChiTietDonHang.Include(c => c.Game).Include(c => c.DonHang);
            return View(await chiTietDonHangs.ToListAsync());
        }

        // GET: Admin/ChiTietDonHangs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = await db.ChiTietDonHang.FindAsync(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaGame = new SelectList(db.Game, "MaGame", "TenGame");
            ViewBag.MaDH = new SelectList(db.DonHang, "MaDH", "MaDH");
            return View();
        }

        // POST: Admin/ChiTietDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaDH,MaGame,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietDonHang.Add(chiTietDonHang);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MaGame = new SelectList(db.Game, "MaGame", "TenGame", chiTietDonHang.MaGame);
            ViewBag.MaDH = new SelectList(db.DonHang, "MaDH", "MaDH", chiTietDonHang.MaDH);
            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = await db.ChiTietDonHang.FindAsync(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGame = new SelectList(db.Game, "MaGame", "TenGame", chiTietDonHang.MaGame);
            ViewBag.MaDH = new SelectList(db.DonHang, "MaDH", "MaDH", chiTietDonHang.MaDH);
            return View(chiTietDonHang);
        }

        // POST: Admin/ChiTietDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaDH,MaGame,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietDonHang).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MaGame = new SelectList(db.Game, "MaGame", "TenGame", chiTietDonHang.MaGame);
            ViewBag.MaDH = new SelectList(db.DonHang, "MaDH", "MaDH", chiTietDonHang.MaDH);
            return View(chiTietDonHang);
        }

        // GET: Admin/ChiTietDonHangs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietDonHang chiTietDonHang = await db.ChiTietDonHang.FindAsync(id);
            if (chiTietDonHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietDonHang);
        }

        


        public string  DeleteConfirmed(int? id, int? magame)
        {
            if(id != null && magame != null)
            {
                var chiTietDonHang = (from cdth in db.ChiTietDonHang where cdth.MaDH == id && cdth.MaGame == magame select cdth).FirstOrDefault();
                var donHang = (from dh in db.DonHang where dh.MaDH == id select dh).FirstOrDefault();
                var tinhtien = donHang.Tongtien - chiTietDonHang.Gia;
                donHang.Tongtien = tinhtien;
                db.ChiTietDonHang.Remove(chiTietDonHang);
                db.SaveChanges();
                return "Xóa thành công";
            }
            else
            {
                return "Xóa không thành công";
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

        public JsonResult GetDataDetailOrder()
        {
            var data  = (from ctdh in db.ChiTietDonHang join game in db.Game on 
                         ctdh.MaGame equals game.MaGame
                         select new {ctdh.MaDH,ctdh.Gia,game.TenGame,game.MaGame}).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}
