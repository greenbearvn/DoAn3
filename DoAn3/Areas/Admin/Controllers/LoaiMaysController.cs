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
    public class LoaiMaysController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/LoaiMays
        public async Task<ActionResult> Index()
        {
            return View(await db.LoaiMay.ToListAsync());
        }


        public JsonResult GetDataLoaiMay()
        {
            var query = (from lm in db.LoaiMay select new {lm.MaMay,lm.TenMay,lm.MoTa}).ToList();
            return Json(query,JsonRequestBehavior.AllowGet);
        }



        // GET: Admin/LoaiMays/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiMay loaiMay = await db.LoaiMay.FindAsync(id);
            if (loaiMay == null)
            {
                return HttpNotFound();
            }
            return View(loaiMay);
        }

        // GET: Admin/LoaiMays/Create
        public ActionResult Create()
        {
            return View();
        }

       
        
        public bool CreateLM( LoaiMay loaiMay)
        {
            if (loaiMay!=null)
            {
                
                db.LoaiMay.Add(loaiMay);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

           
        }

        // GET: Admin/LoaiMays/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiMay loaiMay = await db.LoaiMay.FindAsync(id);
            if (loaiMay == null)
            {
                return HttpNotFound();
            }
            return View(loaiMay);
        }


        public JsonResult EditData(int id)
        {
            var query = (from lm in db.LoaiMay where lm.MaMay == id select new {lm.MaMay,lm.TenMay,lm.MoTa}).FirstOrDefault();
            return Json(query,JsonRequestBehavior.AllowGet);
        }
       
        public bool EditLM(LoaiMay loaiMay)
        {
            if (loaiMay != null)
            {
                
                LoaiMay cate = (from lm in db.LoaiMay where lm.MaMay == loaiMay.MaMay select lm).FirstOrDefault();
                cate.TenMay = loaiMay.TenMay;
                cate.MoTa = loaiMay.MoTa;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }


        }

        // GET: Admin/LoaiMays/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiMay loaiMay = await db.LoaiMay.FindAsync(id);
            if (loaiMay == null)
            {
                return HttpNotFound();
            }
            return View(loaiMay);
        }

      
        public JsonResult DeleteConfirmed(int id)
        {
            var loaimay = (from lm in db.LoaiMay where lm.MaMay == id select lm).FirstOrDefault();
            if (loaimay != null)
            {
                db.LoaiMay.Remove(loaimay);
                db.SaveChanges();
                var query = (from lm in db.LoaiMay select new { lm.MaMay, lm.TenMay, lm.MoTa }).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var query = (from lm in db.LoaiMay select new { lm.MaMay, lm.TenMay, lm.MoTa }).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
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
    }
}
