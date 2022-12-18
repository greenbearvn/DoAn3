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
    public class KhachHangsController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/KhachHangs
        public async Task<ActionResult> Index()
        {
            var khachHangs = db.KhachHang.Include(k => k.User);
            return View(await khachHangs.ToListAsync());
        }

        // GET: Admin/KhachHangs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = await db.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.User, "UserID", "UserName");
            return View();
        }

        
    
        public bool CreateKH([Bind(Include = "MaKH,TenKH,DiaChi,SDT,SoTK")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHang.Add(khachHang);
                db.SaveChanges();
               return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Admin/KhachHangs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = await db.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.User, "UserID", "UserName", khachHang.MaKH);
            return View(khachHang);
        }

  
        public bool EditKH([Bind(Include = "MaKH,TenKH,DiaChi,SDT,SoTK")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                var kh = (from khachhang in db.KhachHang where khachhang.MaKH == khachHang.MaKH select khachhang).FirstOrDefault();
                
                kh.TenKH = khachHang.TenKH;
                kh.DiaChi = khachHang.DiaChi;
                kh.SDT = khachHang.SDT;
                kh.SoTK = khachHang.SoTK;   
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Admin/KhachHangs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = await db.KhachHang.FindAsync(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

     
        public bool DeleteConfirmed(int? id)
        {
            if(id != null)
            {
                var khachHang = (from k in db.KhachHang where k.MaKH == id select k).FirstOrDefault();
                db.KhachHang.Remove(khachHang);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
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

        public JsonResult DataIndex()
        {
            var data = (from kh in db.KhachHang
                        join us in db.User
                        on kh.MaKH equals us.UserID  
                        select new 
            {
                kh.MaKH,
                kh.TenKH,
                kh.DiaChi,
                kh.SDT,
                kh.SoTK,
                us.UserName
            }).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDataUser()
        {
            var data = (from us in db.User select new
            {
                us.UserName,
                us.UserID
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        


        public JsonResult EditData(int id)
        {
            var data = (from kh in db.KhachHang where kh.MaKH == id select new {kh.TenKH,kh.DiaChi,kh.SDT,kh.SoTK}).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
