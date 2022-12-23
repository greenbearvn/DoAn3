using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn3.Models;
using System.Security.Cryptography;
using System.Text;

namespace DoAn3.Controllers
{
    public class UsersController : Controller
    {
        DoAn3Entities db = new DoAn3Entities();

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                
                var check = (from user in db.User where user.UserName == _user.UserName select user.UserName).FirstOrDefault();
                if (check == null)
                {
                    _user.Role = "user";
                    _user.Password = GetMD5(_user.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.User.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login","Users");
                }
                else
                {
                    ViewBag.error = "Username already exists";
                    return View();
                }


            }
            return View();


        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = db.User.Where(s => s.UserName.Equals(username) && s.Password.Equals(f_password)).ToList();
                var user = (from us in db.User where us.UserName == username && us.Password == f_password select us).FirstOrDefault();
                if (data.Count() > 0)
                {
                    //add session
                    
                    Session["username"] = data.FirstOrDefault().UserName;
                    Session["UserID"] = data.FirstOrDefault().UserID;
                    string role = user.Role;
                    if(role == "admin")
                    {
                        Session["Admin"] = user.Role;
                    }

                    if(role == "user")
                    {
                        Session["User"] = user.Role;
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        //create a string MD5
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

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }


    }
}