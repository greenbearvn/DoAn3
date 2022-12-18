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
using System.Security.Cryptography;
using System.Text;

namespace DoAn3.Areas.Admin.Controllers
{
    public class UsersAdminController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/UsersAdmin
        public async Task<ActionResult> Index()
        {
            var users = db.User.Include(u => u.KhachHang);
            return View(await users.ToListAsync());
        }

        // GET: Admin/UsersAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/UsersAdmin/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.KhachHang, "MaKH", "TenKH");
            return View();
        }

        // POST: Admin/UsersAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserID,UserName,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.KhachHang, "MaKH", "TenKH", user.UserID);
            return View(user);
        }

        // GET: Admin/UsersAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.KhachHang, "MaKH", "TenKH", user.UserID);
            return View(user);
        }

        // POST: Admin/UsersAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserID,UserName,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.KhachHang, "MaKH", "TenKH", user.UserID);
            return View(user);
        }

        // GET: Admin/UsersAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.User.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/UsersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.User.FindAsync(id);
            db.User.Remove(user);
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

        public JsonResult GetDataUser()
        {
            var data = (from us in db.User  select new { us.UserID, us.UserName, us.Role }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public bool CreateUser(User us)
        {
            if (us != null)
            {
                us.Password = GetMD5(us.Password);
                db.User.Add(us);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult DeleteUser(User usser)
        {
            var user = (from us in db.User where us.UserID == usser.UserID select us).FirstOrDefault();   
            if (user != null)
            {
                db.User.Remove(user);
                db.SaveChanges();
                var data = (from us in db.User select new { us.UserID, us.UserName, us.Role }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = (from us in db.User select new { us.UserID, us.UserName, us.Role }).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetADataUser(int id)
        {
            var data = (from us in db.User where us.UserID == id select new { us.UserID, us.UserName, us.Password, us.Role }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public bool EditUser(User users)
        {
            var user = (from us in db.User where us.UserID == users.UserID select us).FirstOrDefault();
            if(user != null)
            {
                user.UserName = users.UserName;
                user.Password = GetMD5(users.Password);
                user.Role = users.Role;
                db.SaveChanges();
                return true;
            }
            else return false;
        }
    }
}
