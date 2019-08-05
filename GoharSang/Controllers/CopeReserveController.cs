using GoharSang.Models;
using GoharSang.Models.vmModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class CopeReserveController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();
        int PageOffSet = 10;

        public ActionResult Index(int? PageNumber)
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
                        if (PageNumber == null)
                        {
                            var result = getCopReserve(1);
                            ViewBag.AllPage = (db.CopsBooking.Where(p => p.StateDelete == 0).Count() / 10) + 1;
                            ViewBag.PageNumber = 1;
                            return View(result);
                        }
                        else
                        {
                            var result = getCopReserve((int)PageNumber);
                            ViewBag.AllPage = (db.CopsBooking.Where(p => p.StateDelete == 0).Count() / 10) + 1;

                            ViewBag.PageNumber = (int)PageNumber;

                            return View(result);
                        }
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
         
        private object getCopReserve(int PageNumber)
        {

            if (PageNumber <= 0)
            {
                PageNumber = 1;
            }
            int PageSkip = (PageNumber - 1) * PageOffSet;

            var list = db.CopsBooking.Where(p => p.StateDelete == 0).ToList()
                .Select(p => new vmcopreserv
                {
                    Id = p.Id,
                    CustomerFullName = p.CustomerFullName,
                    DateExpired = clsPersianDate.MiladiToShamsi(p.DateExpired),
                    StoreMame = p.Store.Name,
                    Reserved = p.RecordEntryCopsBooking.Where(q => q.IdCopsBooking == p.Id).Count()
                }).OrderBy(u => u.Id)
                .Skip(PageSkip)
                .Take(PageOffSet)
                .ToList();

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


        [HttpPost]

        public async Task<ActionResult> DeleteCopeReserve(long id)
        {
            CopsBooking cb = db.CopsBooking.Find(id);
            cb.StateDelete = 1;

           await db.SaveChangesAsync();

          RecordEntryCopsBooking recb = db.RecordEntryCopsBooking.Where(p => p.IdCopsBooking == id).FirstOrDefault();
            db.RecordEntryCopsBooking.Remove(recb);
            await db.SaveChangesAsync();

            return Json("Ok",JsonRequestBehavior.AllowGet);
        }


    }
}