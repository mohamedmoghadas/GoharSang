using GoharSang.Models;
using GoharSang.Models.vmModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class exitRequestsController : Controller
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
                    List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();


                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (usr.Where(p=>p.IdRole == 7).Any())
                        {
                            var result = GetExitOrder();
                            return View(result);
                        }
                        else
                        {
                            return RedirectToAction("AccessDenied", "Error");


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

        private object GetExitOrder()
        {

            long i = 0;
            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         join re in db.Record_the_entry
                         on reo.IdRecordEntry equals re.Id
                         where exo.StateDelete == 0  && reo.StateExit == false && exo.IdState == 4
                         select new { exo, reo, re }).ToList()
                         .Select(p => new listRecordEntryExitOrder
                         {
                             Id = p.exo.Id,
                             CustomerFullName = p.exo.CustomerFullName,
                             Uploaddate = clsPersianDate.MiladiToShamsi(p.exo.Uploaddate),
                             StoreName = p.exo.Store.Name,
                             RecordEntryExitOrderCount = p.exo.RecordEntryExitOrder.Count,
                             stateName = p.exo.State.Name,
                             Weight = p.re.Weight,
                             Countmandeh=p.reo.StateExit==false?i++:0
                         }).ToList();


            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;

            return _vmReportBargirt;
        }


       
        public ActionResult Index(listRecordEntryExitOrder vmr)
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

                    List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (usr.Where(p=>p.IdRole ==7).Any())
                        {
                            var result = SGetExitOrder(vmr);
                            return View(result);
                        }
                        else
                        {
                            return RedirectToAction("AccessDenied", "Error");


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

        private object SGetExitOrder(listRecordEntryExitOrder vmr)
        {

            long i = 0;
            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         join re in db.Record_the_entry
                         on reo.IdRecordEntry equals re.Id
                         where exo.StateDelete == 0  && reo.StateExit == false && exo.IdState == 4
                         select new { exo, reo, re }).ToList()
                         .Select(p => new listRecordEntryExitOrder
                         {
                             Id = p.exo.Id,
                             CustomerFullName = p.exo.CustomerFullName,
                             Uploaddate = clsPersianDate.MiladiToShamsi(p.exo.Uploaddate),
                             StoreName = p.exo.Store.Name,
                             RecordEntryExitOrderCount = p.exo.RecordEntryExitOrder.Count,
                             stateName = p.exo.State.Name,
                             Weight = p.re.Weight,
                             Countmandeh = p.reo.StateExit == false ? i++ : 0
                         }).ToList();



            if (vmr.Uploaddate != "" || vmr.Uploaddate != null)
            {
                lists = lists.Where(p => p.Uploaddate == vmr.Uploaddate).ToList();
            }

            if (vmr.CustomerFullName != null)
            {
                lists = lists.Where(p => p.CustomerFullName.Contains(vmr.CustomerFullName)).ToList();
            }
            if (vmr.StoreName != null)
            {
                lists = lists.Where(p => p.StoreName.Contains(vmr.StoreName)).ToList();
            }

            if (vmr.RecordEntryExitOrderCount != 0)
            {
                lists = lists.Where(p => p.RecordEntryExitOrderCount == vmr.RecordEntryExitOrderCount).ToList();
            }
            if (vmr.stateName != null)
            {
                lists = lists.Where(p => p.stateName.Contains(vmr.stateName)).ToList();
            }

            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;

            return _vmReportBargirt;
        }
    }
}