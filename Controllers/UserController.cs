using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrationAndLogin.Models;
using System.Net;
using System.Web.Security;
using System.Web.Helpers;

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
                if (isExist)
                {
                    ViewBag.emailexist = "email already exist";
                    return View(u);
                }
                #endregion
            }

            #region //Password hashing
            /*u.Password = Crypto.Hash(u.Password);
            u.ConfirmPassword = Crypto.Hash(u.ConfirmPassword);*/
            #endregion
            #region //save to database
            using (MVCEntities dc = new MVCEntities())
            {
                dc.UserDbs.Add(u);
                dc.SaveChanges();
            }
            #endregion
            ViewBag.success = "registered successful";
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
            #region password validation
            /*using (MVCEntities dc = new MVCEntities())
            {
                var v = dc.UserDbs.Where(a => a.EmailID == u.EmailID).FirstOrDefault();
                if (v != null)
                {


                    if (string.Compare(Crypto.Hash(u.Password), v.Password) == 0)
                    {
                        int timeout = u.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(u.EmailID, u.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        return RedirectToAction("Index", "Employees", new { username = u.UserName });
                    }
                }
                ModelState.AddModelError("", "Invalid username and passoword");
                return View();*/
            #endregion

            using (MVCEntities dc = new MVCEntities())
                 {
                     bool isValid = dc.UserDbs.Any(x => x.EmailID == u.EmailID && x.Password == u.Password);
                     if (isValid)
                     {
                         Session["username"] = u.EmailID;

                            int timeout = u.RememberMe ? 525600 : 20; // 525600 min = 1 year
                            var ticket = new FormsAuthenticationTicket(u.EmailID, u.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                    FormsAuthentication.SetAuthCookie(u.EmailID, false);
                         return RedirectToAction("Index","Employees",new { username = u.UserName });


                     }
                     ModelState.AddModelError("", "Invalid username and passoword");
                     return View();
            }
        }



            public ActionResult Logout()
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Registration");
            }
        }
    }
