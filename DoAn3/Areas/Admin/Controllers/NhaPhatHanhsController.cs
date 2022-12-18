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
    public class NhaPhatHanhsController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/NhaPhatHanhs
        public async Task<ActionResult> Index()
        {
            return View(await db.NhaPhatHanh.ToListAsync());
        }

        // GET: Admin/NhaPhatHanhs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaPhatHanh nhaPhatHanh = await db.NhaPhatHanh.FindAsync(id);
            if (nhaPhatHanh == null)
            {
                return HttpNotFound();
            }
            return View(nhaPhatHanh);
        }

        // GET: Admin/NhaPhatHanhs/Create
        public ActionResult Create()
        {
            return View();
        }

        
        public bool CreateNPH([Bind(Include = "MaNPH,TenNPH,TruSo")] NhaPhatHanh nhaPhatHanh)
        {
            if (ModelState.IsValid)
            {

                db.NhaPhatHanh.Add(nhaPhatHanh);
                db.SaveChanges();
                return true;
                
            }
            else
            {
                return false;
            }
        }

        // GET: Admin/NhaPhatHanhs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaPhatHanh nhaPhatHanh = await db.NhaPhatHanh.FindAsync(id);
            if (nhaPhatHanh == null)
            {
                return HttpNotFound();
            }
            return View(nhaPhatHanh);
        }

        
        public bool EditNPH([Bind(Include = "MaNPH,TenNPH,TruSo")] NhaPhatHanh nhaPhatHanh)
        {
            if (ModelState.IsValid)
            {
                var data = (from nph in db.NhaPhatHanh where nph.MaNPH == nhaPhatHanh.MaNPH select nph).FirstOrDefault();
                data.MaNPH = nhaPhatHanh.MaNPH;
                data.TenNPH = nhaPhatHanh.TenNPH;
                data.TruSo = nhaPhatHanh.TruSo;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Admin/NhaPhatHanhs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaPhatHanh nhaPhatHanh = await db.NhaPhatHanh.FindAsync(id);
            if (nhaPhatHanh == null)
            {
                return HttpNotFound();
            }
            return View(nhaPhatHanh);
        }

        
        public bool DeleteConfirmed(int? id)
        {
           if(id != null)
            {
                var nhaPhatHanh = (from nph in db.NhaPhatHanh where nph.MaNPH == id select nph).FirstOrDefault();
                db.NhaPhatHanh.Remove(nhaPhatHanh);
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

        /////
        public JsonResult GetAllNPH()
        {
            var data = (from nph in db.NhaPhatHanh select new {nph.MaNPH, nph.TenNPH,nph.TruSo}).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailNPH(int id)
        {
            var data = (from nph in db.NhaPhatHanh where nph.MaNPH == id select new {nph.MaNPH,nph.TenNPH,nph.TruSo}).FirstOrDefault();
            return Json(data,JsonRequestBehavior.AllowGet);
        }
    }
}
