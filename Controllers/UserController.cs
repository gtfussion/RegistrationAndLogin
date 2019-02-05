using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrationAndLogin.Models;
using System.Net;
using System.Web.Security;

namespace RegistrationAndLogin.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            using (MVCEntities db = new MVCEntities())
            {
                return View(db.UserDbs.ToList());
            }
            
        }
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(UserDb u)
        {
            if (ModelState.IsValid)
            {
                #region//Email is already Exist
                var isExist = IsEmailExist(u.EmailID);
                if(isExist)
                {
                    ViewBag.emailexist="email already exist";
                    return View(u);
                }
                #endregion
            }
            #region //save to database
            using (MVCEntities dc = new MVCEntities())
            {
                dc.UserDbs.Add(u);
                dc.SaveChanges();
            }
            #endregion
            ViewBag.success="registered successful";
                return View(u);
        }
        public bool IsEmailExist(string emailID)
        {
            using (MVCEntities dc = new MVCEntities())
            {
                var v = dc.UserDbs.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(UserDb u)
        {
            using (MVCEntities dc = new MVCEntities())
            {
                bool isValid = dc.UserDbs.Any(x => x.EmailID == u.EmailID && x.Password == u.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(u.EmailID, false);
                    return RedirectToAction("Index","Employees");


                }
                ModelState.AddModelError("", "Invalid username and passoword");
                return View();
            }
        }

       
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn");
        }
    }
}