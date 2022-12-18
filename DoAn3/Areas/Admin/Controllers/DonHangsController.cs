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
    public class DonHangsController : Controller
    {
        private DoAn3Entities db = new DoAn3Entities();

        // GET: Admin/DonHangs
        public async Task<ActionResult> Index()
        {
            var donHangs = db.DonHang.Include(d => d.KhachHang);
            return View(await donHangs.ToListAsync());
        }

        // GET: Admin/DonHangs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = await db.DonHang.FindAsync(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // GET: Admin/DonHangs/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHang, "MaKH", "TenKH");
            return View();
        }


        public bool CreateDH( DonHang donHang)
        {
            if (donHang != null)
            {
                var litsCart = (List<ChiTietDonHang>)Session["cartAdmin"];

               
                db.DonHang.Add(donHang);
                db.SaveChanges();

                if(litsCart != null)
                {
                    foreach (var item in litsCart)
                    {
                        int intOrderId = donHang.MaDH;

                        //Chi tiet don hang
                        ChiTietDonHang ctdh = new ChiTietDonHang();
                        ctdh.MaDH = intOrderId;
                        ctdh.MaGame = item.Game.MaGame;
                        ctdh.Gia = item.Game.GiaTien;
                        db.ChiTietDonHang.Add(ctdh);
                        db.SaveChanges();

                    }
                }
                
                RemoveSession();
                return true;
            }
            else
            {
                return false;
            }

        }


        // GET: Admin/DonHangs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = await db.DonHang.FindAsync(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KhachHang, "MaKH", "TenKH", donHang.MaKH);
            return View(donHang);
        }


            
       
        //public bool EditDH([Bind(Include = "MaDH,MaKH,NgayLap,TinhTrang,Tongtien")] DonHang donHang, int moneyBefore)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var litsCart = (List<ChiTietDonHang>)Session["cartEditAdmin"];

        //        var data = (from dh in db.DonHang where dh.MaDH == donHang.MaDH select dh).FirstOrDefault();
        //        data.MaDH = donHang.MaDH;
        //        data.MaKH = donHang.MaKH;
        //        data.NgayLap = donHang.NgayLap;
        //        data.TinhTrang = donHang.TinhTrang;
        //        data.Tongtien = donHang.Tongtien + moneyBefore;
        //        db.SaveChanges();

        //        if(litsCart != null)
        //        {
        //            foreach (var item in litsCart)
        //            {
        //                int intOrderId = donHang.MaDH;
        //                //Chi tiet don hang
        //                ChiTietDonHang ctdh = new ChiTietDonHang();
        //                ctdh.MaDH = intOrderId;
        //                ctdh.MaGame = item.Game.MaGame;
        //                ctdh.Gia = item.Game.GiaTien;
        //                db.ChiTietDonHang.Add(ctdh);
        //                db.SaveChanges();

        //            }
        //        }
                
        //        RemoveSession();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        public bool EditDH(DonHang dh, int moneyBefore)
        {
            if(dh != null)
            {
                var donhang = (from donHang in db.DonHang where donHang.MaDH == dh.MaDH select donHang).FirstOrDefault();
                donhang.MaKH = dh.MaKH;
                
                donhang.NgayLap = dh.NgayLap;
                donhang.TinhTrang = dh.TinhTrang;
                donhang.Tongtien = dh.Tongtien + moneyBefore;
                db.SaveChanges();
                var litsCart = (List<ChiTietDonHang>)Session["cartEditAdmin"];

                if (litsCart != null)
                {
                    foreach (var item in litsCart)
                    {
                        int intOrderId = dh.MaDH;
                        //Chi tiet don hang
                        ChiTietDonHang ctdh = new ChiTietDonHang();
                        ctdh.MaDH = intOrderId;
                        ctdh.MaGame = item.Game.MaGame;
                        ctdh.Gia = item.Game.GiaTien;
                        db.ChiTietDonHang.Add(ctdh);
                        db.SaveChanges();

                    }
                }

                RemoveSession();
                return true;
            }
            else
            {
                return false;
            }
        }


        public JsonResult ChangStatus(int MaDH,bool tt)
        {
            var donhang = (from dh in db.DonHang where dh.MaDH == MaDH && dh.TinhTrang == tt select dh).FirstOrDefault();
            if (tt == true)
            {
                donhang.TinhTrang = false;
                db.SaveChanges();

            }

            if (tt == false)
            {
                donhang.TinhTrang = true;
                db.SaveChanges();

            }

            var query = (from dh in db.DonHang
                         join kh in db.KhachHang
                         on dh.MaKH equals kh.MaKH
                         select new { dh.MaDH, kh.TenKH, dh.NgayLap, dh.TinhTrang, dh.Tongtien }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);

        } 



        
        public void RemoveSession()
        {
            List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartAdmin"];
            List<ChiTietDonHang> cartEdit = (List<ChiTietDonHang>)Session["cartEditAdmin"];
            if (cart != null || cartEdit != null)
            {

                Session["cartAdmin"] = null;
                Session["cartEditAdmin"] = null;

            }
        }

        // GET: Admin/DonHangs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = await db.DonHang.FindAsync(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }


        public string DeleteConfirmed(int id)
        {
            if (id > 0)
            {
                var donHang = (from dh in db.DonHang where dh.MaDH == id select dh).FirstOrDefault();
                db.DonHang.Remove(donHang);
                db.SaveChanges();
                return "Xóa đơn hàng thành công";
            }
            else
            {
                return "Giá trị rỗng";

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
            var query = (from dh in db.DonHang
                         join kh in db.KhachHang
                         on dh.MaKH equals kh.MaKH
                         select new { dh.MaDH, kh.TenKH, dh.NgayLap, dh.TinhTrang, dh.Tongtien }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getKhachHang()
        {
            var query = (from kh in db.KhachHang select new {
                kh.MaKH,
                kh.TenKH
            }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OrderDetail(int id)
        {
            var query = (from dh in db.DonHang
                         join kh in db.KhachHang
                         on dh.MaKH equals kh.MaKH
                         where dh.MaDH == id
                         select new { dh.MaDH,kh.MaKH, kh.TenKH, dh.NgayLap, dh.TinhTrang, dh.Tongtien }).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DataEditCart(int id)
        {
            var data = (from ctdh in db.ChiTietDonHang join game in db.Game on ctdh.MaGame equals game.MaGame where ctdh.MaDH == id select new
            {
                ctdh.MaDH,
                ctdh.MaGame,
                game.TenGame,
                game.AnhGame,
                game.TrangThai,
                ctdh.Gia
            }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCTDH(int id)
        {
            var query = (from cthdh in db.ChiTietDonHang join game in db.Game on cthdh.MaGame equals game.MaGame where cthdh.MaDH == id select new { cthdh.MaDH, game.TenGame, cthdh.Gia }).ToList();
            
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public bool checkCtDH(int id)
        {
            var query = (from cthdh in db.ChiTietDonHang join game in db.Game on cthdh.MaGame equals game.MaGame where cthdh.MaDH == id select new { cthdh.MaDH, game.TenGame, cthdh.Gia }).ToList();

            if(query.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }


        public ActionResult AddCart(int id)
        {
            var games = db.Game.FirstOrDefault(s => s.MaGame == id);

            if (Session["cartAdmin"] != null)
            {
                List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartAdmin"];
                var kt = cart.FirstOrDefault(s => s.MaGame == id);
                if (kt == null)
                {
                    ChiTietDonHang game = new ChiTietDonHang() { MaGame = id, Game = games };
                    cart.Add(game);
                }
                Session["cartAdmin"] = cart;
            }
            else
            {
                List<ChiTietDonHang> cart = new List<ChiTietDonHang>();
                ChiTietDonHang game = new ChiTietDonHang() { MaGame = id, Game = games };
                cart.Add(game);
                Session["cartAdmin"] = cart;
            }

            return RedirectToAction("Create","DonHangs");
        }

        public JsonResult DataCart()
        {
            List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartAdmin"];
            List<ChiTietDonHang> ds = new List<ChiTietDonHang>();
            if (cart != null)
            {
                foreach (var c in cart)
                {
                    ds.Add(c);
                }
                return Json(ds,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Vui long them thong tin moi", JsonRequestBehavior.AllowGet);
            }
           
        }

        public PartialViewResult ViewCart()
        {
            {
                List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartAdmin"];

             

                List<ChiTietDonHang> ds = new List<ChiTietDonHang>();
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

            List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartAdmin"];
            if (cart != null)
            {

                cart.RemoveAll(s => s.MaGame == id);
                Session["cartAdmin"] = cart;

            }
            return RedirectToAction("Create", "DonHangs");
        }

        public ActionResult RemoveCartEditItem(int id, int mhd)
        {

            List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartEditAdmin"];
            if (cart != null)
            {

                cart.RemoveAll(s => s.MaGame == id);
                Session["cartEditAdmin"] = cart;
                return RedirectToAction("Edit", "DonHangs", new { id = mhd });

            }
            else
            {
                return RedirectToAction("Edit", "DonHangs", new {id = mhd }); 
            }
            
        }

        public bool AddEditCart(int id, int MaDH)
        {
            var games = db.Game.FirstOrDefault(s => s.MaGame == id);

            if (Session["cartEditAdmin"] != null)
            {
                List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartEditAdmin"];
                var kt = cart.FirstOrDefault(s => s.MaGame == id);
                if (kt == null)
                {
                    ChiTietDonHang game = new ChiTietDonHang() {MaDH = MaDH, MaGame = id, Game = games };
                    cart.Add(game);
                }
                Session["cartEditAdmin"] = cart;

                return true; 
            }
            else
            {
                List<ChiTietDonHang> cart = new List<ChiTietDonHang>();
                ChiTietDonHang game = new ChiTietDonHang() { MaDH = MaDH, MaGame = id, Game = games };
                cart.Add(game);
                Session["cartEditAdmin"] = cart;

                return true;
            }
        }

        public PartialViewResult ViewEditCart()
        {
            {
                List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cartEditAdmin"];



                List<ChiTietDonHang> ds = new List<ChiTietDonHang>();
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

        public bool RemoveCartItem(int? id, int? magame)
        {
            if (id != null && magame != null)
            {
                var chiTietDonHang = (from cdth in db.ChiTietDonHang where cdth.MaDH == id && cdth.MaGame == magame select cdth).FirstOrDefault();
                var donHang = (from dh in db.DonHang where dh.MaDH == id select dh).FirstOrDefault();
                var tinhtien = donHang.Tongtien - chiTietDonHang.Gia;
                donHang.Tongtien = tinhtien;
                db.ChiTietDonHang.Remove(chiTietDonHang);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
