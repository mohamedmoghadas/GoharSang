using GoharSang.Models;
using GoharSang.Models.vmModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class CopeReserveController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();
        public ActionResult Index()
        {
            try
            {
                string UserIdcookie = "";
                if (Request.Cookies.AllKeys.Contains("UserId"))
                {
                    UserIdcookie = Request.Cookies["UserId"].Value;
                    string _Id = UserIdcookie;
                    long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
                    Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        var result = getCopReserve();
                        return View(result);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "LogIn");

                }
            }
            catch (Exception ee)
            {
                return RedirectToAction("Index", "LogIn");

            }



           
        }
         
        private object getCopReserve()
        {
            var list = db.CopsBooking.Where(p => p.StateDelete == 0).ToList()
                .Select(p => new vmcopreserv
                {
                    Id = p.Id,
                    CustomerFullName = p.CustomerFullName,
                    DateExpired = clsPersianDate.MiladiToShamsi(p.DateExpired),
                    StoreMame = p.Store.Name,
                    Reserved = p.RecordEntryCopsBooking.Where(q => q.IdCopsBooking == p.Id).Count()
                }).ToList();

            return list;
        }

       [HttpPost]
       public ActionResult LoadEditCopsBooking(long id)
        {
            try
            {
                string UserIdcookie = "";
                if (Request.Cookies.AllKeys.Contains("UserId"))
                {
                    UserIdcookie = Request.Cookies["UserId"].Value;
                    string _Id = UserIdcookie;
                    long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
                    Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        RecordEntryCopsBooking RecordEntryCopsBooking = db.RecordEntryCopsBooking.Where(p => p.IdCopsBooking == id).FirstOrDefault();

                        var listrecordentry = db.Record_the_entry.Where(p => p.StateDelete == 0).ToList()
                           .Select(p => new vmListRecordEntry
                           {
                               Id = p.Id,
                               minename = p.mine.Name,
                               copname = p.Cops.Name,
                               Dimensions = p.length + "*" + p.width + "*" + p.Height,
                               Weight = p.Weight,
                               CopsCod = p.CopsCod,
                               Transfernumber = p.Transfernumber,
                               image = db.Record_the_Entrry_Image.Where(q => q.Id == p.Id).FirstOrDefault() == null ? ""
                               : db.Record_the_Entrry_Image.Where(q => q.Id == p.Id).FirstOrDefault().Image,
                           }).ToList();


                        vmEditCopReserv _vmEditCopReserv = new vmEditCopReserv();
                        _vmEditCopReserv.RecordEntryCopsBooking = RecordEntryCopsBooking;
                        _vmEditCopReserv.ListRecordEntry = listrecordentry;

                        return View(_vmEditCopReserv);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "LogIn");

                }
            }
            catch (Exception ee)
            {
                return RedirectToAction("Index", "LogIn");

            }


           
        }

     
    }
}