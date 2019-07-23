using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoharSang.Models;

namespace GoharSang.Controllers
{
    public class LogInController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Users _user)
        {
            var pass = CreatHash.HashPass(_user.Password);
           
            Users admin = null;
            try
            {
                admin = db.Users.FirstOrDefault(p => p.UserName == _user.UserName);
                if (admin != null)
                {
                    if (admin.Password == pass)
                    {
                        string Id = CreatHash.Encrypt(admin.Id.ToString());



                        HttpCookie UserIdcookie = new HttpCookie("UserId");
                        UserIdcookie.Value = Id;
                        UserIdcookie.Expires = DateTime.Now.AddMinutes(30);
                        Response.Cookies.Add(UserIdcookie);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.WrongPassword = "1";
                        return View();

                    }
                }
                else
                {
                    ViewBag.WrongPassword = "1";

                    return View();

                }
            }
            catch (Exception ee)
            {
                return View();

            }
        }
    }
}