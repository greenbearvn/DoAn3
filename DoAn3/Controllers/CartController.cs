using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn3.Models;

namespace DoAn3.Controllers
{
    public class CartController : Controller
    {

        DoAn3Entities db = new DoAn3Entities();

        public ActionResult ViewCart()
        {
            {
                List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cart"];

                if(Session["UserID"] != null)
                {
                    int KhID = (int)Session["UserID"];
                    Session["KhachHangID"] = (from kh in db.KhachHang where kh.MaKH == KhID select kh).FirstOrDefault();
                }

                List <ChiTietDonHang> ds = new List<ChiTietDonHang>();
                if(cart != null)
                {
                    foreach (var c in cart)
                    {
                        ds.Add(c);
                    }
                }
                
                
                return View(ds);
            }
        }

        public void RemoveAllCart()
        {
            List<ChiTietDonHang> lstDetail = (List<ChiTietDonHang>)Session["cart"];
            if (lstDetail != null)
            {
                lstDetail = null;
            }
            
        }

        public string Payment(DonHang dh )
        {

            if (Session["UserID"] == null)
            {
                return "xxx";
            }
            else
            {
                var litsCart = (List<ChiTietDonHang>)Session["cart"];
                int UserID = int.Parse(Session["UserID"].ToString());
                

                int DataUsID = (from us in db.User where us.UserID == UserID select us.UserID).FirstOrDefault();
                

                var lstDetail = (List<ChiTietDonHang>)Session["cart"];
                foreach (var item in lstDetail)
                {


                    var checkKg = ( from kg in db.KhoGame where kg.UserID == UserID select kg.MaKhoGame).FirstOrDefault();
                    var checkGame = (from ctkg in db.ChiTietKhoGame where ctkg.MaKhoGame == checkKg && ctkg.MaGame == item.Game.MaGame select ctkg ).FirstOrDefault();

                    if (checkGame != null)
                    {
                        return "Có game đã tồn tại trong kho lưu trữ game của bạn, vui lòng mua những game chưa có!!!";
                    }
                    else
                    {
                        ///Don hang
                        DonHang donhang = new DonHang();
                        donhang.MaKH = DataUsID;
                        donhang.NgayLap = DateTime.Now;
                        donhang.TinhTrang = false;
                        donhang.Tongtien = dh.Tongtien;
                        db.DonHang.Add(donhang);

                        int MaKG = (from kg in db.KhoGame where kg.UserID == UserID select kg.MaKhoGame).FirstOrDefault();
                        /// kho game 

                        if (MaKG <= 0)
                        {
                            KhoGame khoGame = new KhoGame();

                            khoGame.UserID = DataUsID;
                            db.KhoGame.Add(khoGame);
                            db.SaveChanges();
                        }
                        db.SaveChanges();

                        if (lstDetail != null)
                        {
                            foreach (var items in lstDetail)
                            {
                                int intOrderId = donhang.MaDH;


                                //Chi tiet don hang
                                ChiTietDonHang ctdh = new ChiTietDonHang();
                                ctdh.MaDH = intOrderId;
                                ctdh.MaGame = items.Game.MaGame;
                                ctdh.Gia = items.Game.GiaTien;
                                db.ChiTietDonHang.Add(ctdh);

                                int mkg = (from kg in db.KhoGame where kg.UserID == UserID select kg.MaKhoGame).FirstOrDefault();

                                // Chi tiet kho game
                                ChiTietKhoGame chiTietKhoGame = new ChiTietKhoGame();
                                chiTietKhoGame.MaKhoGame = mkg;
                                chiTietKhoGame.MaGame = items.Game.MaGame;
                                chiTietKhoGame.NgayThem = DateTime.Now;
                                db.ChiTietKhoGame.Add(chiTietKhoGame);
                                db.SaveChanges();
                            }
                        }

                        RemoveSession();



                        return "Thanh toán thành công";
                    }

                }
                return " ";


            }
 
        }

        public void RemoveSession()
        {
            List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cart"];
            if (cart != null )
            {

                Session["cart"] = null;

            }
        }

        public ActionResult RemoveCartItem(int id)
        {

            List<ChiTietDonHang> cart = (List<ChiTietDonHang>)Session["cart"];
            if (cart != null)
            {   
                
                cart.RemoveAll(s  => s.MaGame == id);
                Session["cart"] = cart;

            }
            return RedirectToAction("ViewCart","Cart");
        }

       

        public ActionResult Success()
        {
            return View();
        }

        public string CustomerInfor(KhachHang kh)
        {

            int checkKh = (from cs in db.KhachHang where cs.MaKH == kh.MaKH select cs.MaKH).FirstOrDefault();

            if(checkKh <= 0)
            {
                KhachHang khachHang = new KhachHang();
                khachHang.MaKH = kh.MaKH;
                khachHang.TenKH = kh.TenKH;
                khachHang.DiaChi = kh.DiaChi;
                khachHang.SDT = kh.SDT;
                khachHang.SoTK = kh.SoTK;
                db.KhachHang.Add(khachHang);
                db.SaveChanges();
                return "Them Thanh cong";
            }

            return "Da ton tai";

        }
    }
}