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
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

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


        public bool CreateDH( List<ChiTietDonHang> ListCTDH , int makh,  bool tinhtrang ,decimal tongtien)
        {
            if (makh >0)
            {

                DonHang donHang = new DonHang();
                donHang.MaKH = makh;
                DateTime today = DateTime.Today;
                donHang.NgayLap = today;
                donHang.TinhTrang = tinhtrang;
                donHang.Tongtien = tongtien;
                db.DonHang.Add(donHang);

                db.SaveChanges();

                if (ListCTDH != null)
                {
                    foreach (var item in ListCTDH)
                    {
                        int intOrderId = donHang.MaDH;

                        //Chi tiet don hang
                        ChiTietDonHang ctdh = new ChiTietDonHang();
                        ctdh.MaDH = intOrderId;
                        ctdh.MaGame = item.MaGame;
                        ctdh.Gia = item.Gia;

                        db.ChiTietDonHang.Add(ctdh);
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

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EditAddOrderDetail(List<ChiTietDonHang> ListCTDH, int madh)
        {
            if (ListCTDH != null)
            {
                foreach (var item in ListCTDH)
                {
                    int intOrderId = madh;
                    //Chi tiet don hang
                    ChiTietDonHang ctdh = new ChiTietDonHang();
                    ctdh.MaDH = intOrderId;
                    ctdh.MaGame = item.MaGame;
                    ctdh.Gia = item.Gia;
                    db.ChiTietDonHang.Add(ctdh);
                    db.SaveChanges();

                }
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
                         orderby dh.MaDH descending
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


        public JsonResult DeleteConfirmed(int id)
        {
            if (id > 0)
            {
                var donHang = (from dh in db.DonHang where dh.MaDH == id select dh).FirstOrDefault();
                db.DonHang.Remove(donHang);
                db.SaveChanges();
                var query = (from dh in db.DonHang
                             join kh in db.KhachHang
                             on dh.MaKH equals kh.MaKH
                             orderby dh.MaDH descending
                             select new { dh.MaDH, kh.TenKH, dh.NgayLap, dh.TinhTrang, dh.Tongtien }).ToList();
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var query = (from dh in db.DonHang
                             join kh in db.KhachHang
                             on dh.MaKH equals kh.MaKH
                             orderby dh.MaDH descending
                             select new { dh.MaDH, kh.TenKH, dh.NgayLap, dh.TinhTrang, dh.Tongtien }).ToList();
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


        public JsonResult DataIndex()
        {
            var query = (from dh in db.DonHang
                         join kh in db.KhachHang
                         on dh.MaKH equals kh.MaKH
                         orderby dh.MaDH descending
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
            var query = (from cthdh in db.ChiTietDonHang join game in db.Game on cthdh.MaGame equals game.MaGame where cthdh.MaDH == id select new { cthdh.MaDH, game.TenGame,game.AnhGame, cthdh.Gia }).ToList();
            
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
            var ds = new List<ChiTietDonHang>();
            if (cart != null)
            {
                foreach (var c in cart)
                {
                    ds.Add(c);
                }
                return Json(ds, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ds, JsonRequestBehavior.AllowGet);
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

       
        public FileResult Export(int id)
        {
            var ctdh = (from CTDH in db.ChiTietDonHang
                        where CTDH.MaDH == id
                        join game in db.Game on CTDH.MaGame equals game.MaGame
                        select new 
                        {
                             CTDH.MaGame,
                            game.TenGame,
                            game.GiaTien
                        }).ToList<object>();

            var donhang = (from dh in db.DonHang
                        where dh.MaDH == id
                        join kh in db.KhachHang on dh.MaKH equals kh.MaKH
                        select new
                        {
                           dh.MaDH,
                           kh.TenKH,
                           dh.NgayLap,
                           dh.Tongtien
                        }).FirstOrDefault<object>();

            //Building an HTML string.
            StringBuilder sb = new StringBuilder();
            string donHang = new JavaScriptSerializer().Serialize(donhang);

            //Table start.
            sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial; font-size: 10pt;'>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Order " + id + "</th>");
            sb.Append("<tr>");
            sb.Append("<td style='border: 1px solid #ccc'>");
            sb.Append(donHang);
            sb.Append("</td>");
            sb.Append("</tr>");
            //Building the Header row.
            sb.Append("<tr>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Order Detail of Order " + id+"</th>");
            
            sb.Append("</tr>");

            //Building the Data rows.
            for (int i = 0; i < ctdh.Count; i++)
            {
                string s = new JavaScriptSerializer().Serialize(ctdh[i]);
                string[] chitiet = new[] { s };
                sb.Append("<tr>");
                for (int j = 0; j < chitiet.Length; j++)
                {
                    //Append data.
                    sb.Append("<td style='border: 1px solid #ccc'>");
                    sb.Append(chitiet[j]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }

            //Table end.
            sb.Append("</table>");

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "application/vnd.ms-word", "InHoaDon.doc");
        }

        public ActionResult ExportToWord(int id)
        {
            // get the data from database

            var order = (from dh in db.DonHang
                         join kh in db.KhachHang
                         on dh.MaKH equals kh.MaKH
                         where dh.MaDH == id
                         select new { dh.MaDH, kh.MaKH, kh.TenKH, dh.NgayLap, dh.TinhTrang, dh.Tongtien }).FirstOrDefault();

            var data = (from cthdh in db.ChiTietDonHang join game in db.Game on cthdh.MaGame equals game.MaGame
                        join nph in db.NhaPhatHanh on game.MaNPH equals nph.MaNPH
                        join loaimay in db.LoaiMay on game.MaMay equals loaimay.MaMay
                        join lg in db.LoaiGame on game.MaLoai equals lg.MaLoai
                        where cthdh.MaDH == id select new { cthdh.MaDH, game.TenGame,  cthdh.Gia }).ToList();
            // instantiate the GridView control from System.Web.UI.WebControls namespace
            // set the data source
            //GridView gr = new GridView();
            //gr.DataSource = order;
            //gr.DataBind();
            GridView gridview = new GridView();

            gridview.DataSource = data;
            
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", "attachment;filename = InHoaDon.doc");
            Response.ContentType = "application/ms-word";
            Response.Charset = "";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    //gr.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }

    }
}
