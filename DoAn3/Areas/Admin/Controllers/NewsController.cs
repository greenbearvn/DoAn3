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
using System.Data.Entity.Validation;

namespace DoAn3.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/News
        public async Task<ActionResult> Index()
        {
            var news = db.New.Include(n => n.LoaiGame).Include(n => n.User);
            return View(await news.ToListAsync());
        }

        // GET: Admin/News/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.New.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
           
            return View();
        }

        


        public bool CreateNews([Bind(Include = "NewsID,Title,Content,Banner,PublicDate,MaLoai,UserID")] News news)
        {
            if (ModelState.IsValid)
            {
                DateTime today = DateTime.Today;
                news.PublicDate = today;
                db.New.Add(news);
                db.SaveChanges();
                return true;


                //try
                //{
                       
                //}
                //catch(DbEntityValidationException e)
                //{
                //    Console.WriteLine(e);
                //}

                //return false;
            }
            else { return false; }
        }

        // GET: Admin/News/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.New.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
           
            return View(news);
        }

        
        public bool EditNews([Bind(Include = "NewsID,Title,Content,Banner,PublicDate,MaLoai,UserID")] News news)
        {
            if (ModelState.IsValid)
            {
                var data = (from ne in db.New where ne.NewsID == news.NewsID select ne).FirstOrDefault();

                data.NewsID = news.NewsID;
                data.Title = news.Title;
                data.UserID = news.UserID;
                data.MaLoai = news.MaLoai;
                data.PublicDate = news.PublicDate;
                data.Banner = news.Banner;
                data.Content = news.Content;
                data.UserID = news.UserID;

                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET: Admin/News/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.New.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

   
        public bool DeleteConfirmed(int? id)
        {
            if(id != null)
            {
                var news = (from n in db.New where n.NewsID == id select n).FirstOrDefault();
                db.New.Remove(news);
                db.SaveChanges();
                return true;
            }
            else
            {
                return true;
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

        public JsonResult GetAllNews()
        {
            var data = (from n in db.New 
                        join lg in db.LoaiGame 
                        on n.MaLoai equals lg.MaLoai
                        join us in db.User
                        on n.UserID equals us.UserID
                        select new
                        {
                            us.UserName,
                            n.NewsID,
                            n.Title,
                            n.Content,
                            n.PublicDate,
                            n.Banner,
                            lg.TenLoai
                        }).ToList();

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailNews( int id)
        {
            var data = (from n in db.New
                        join lg in db.LoaiGame
                        on n.MaLoai equals lg.MaLoai
                        join us in db.User
                        on n.UserID equals us.UserID
                        where n.NewsID == id
                        select new
                        {
                            n.UserID,
                            n.NewsID,
                            n.Title,
                            n.Content,
                            n.PublicDate,
                            n.Banner,
                            n.MaLoai
                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DetailNew(int id)
        {
            var data = (from n in db.New
                        join lg in db.LoaiGame
                        on n.MaLoai equals lg.MaLoai
                        join us in db.User
                        on n.UserID equals us.UserID
                        where n.NewsID == id
                        select new
                        {
                            n.UserID,
                            n.NewsID,
                            n.Title,
                            n.Content,
                            n.PublicDate,
                            n.Banner,
                            lg.TenLoai,
                            us.UserName
                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoaiGame()
        {
            var data = (from lg in db.LoaiGame
                        select new
                        {
                            lg.MaLoai,
                            lg.TenLoai
                        }).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllUser()
        {
            var data = (from us in db.User 
                        select new
                        {
                            us.UserID,
                            us.UserName
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
