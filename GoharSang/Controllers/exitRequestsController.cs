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
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        var result = GetExitOrder();
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

        private object GetExitOrder()
        {

            long i = 0;
            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         join re in db.Record_the_entry
                         on reo.IdRecordEntry equals re.Id
                         where exo.StateDelete == 0
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
    }
}