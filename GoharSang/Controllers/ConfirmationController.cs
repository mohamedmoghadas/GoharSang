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
    public class ConfirmationController : Controller
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


            var lists = db.Exitorder.Where(p => p.StateDelete == 0)
                .ToList()
                .Select(p => new listRecordEntryExitOrder
                {
                    Id = p.Id,
                    CustomerFullName = p.CustomerFullName,
                    Uploaddate = clsPersianDate.MiladiToShamsi(p.Uploaddate),

                    StoreName = p.Store.Name,
                    stateName = p.State.Name,
                    RecordEntryExitOrderCount = p.RecordEntryExitOrder.Where(q => q.IdExitOrder == p.Id).Count()
                }).ToList();


          


            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;

            return _vmReportBargirt;
        }


        [HttpPost]
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
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        var result = SGetExitOrder(vmr);
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
        private object SGetExitOrder(listRecordEntryExitOrder vmr)
        {


            var lists = db.Exitorder.Where(p => p.StateDelete == 0)
                 .ToList()
                 .Select(p => new listRecordEntryExitOrder
                 {
                     Id = p.Id,
                     CustomerFullName = p.CustomerFullName,
                     Uploaddate = clsPersianDate.MiladiToShamsi(p.Uploaddate),

                     StoreName = p.Store.Name,
                     stateName = p.State.Name,
                     RecordEntryExitOrderCount = p.RecordEntryExitOrder.Where(q => q.IdExitOrder == p.Id).Count()
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



        [HttpPost]
        public async Task<ActionResult> Confirm(long id)
        {
            Exitorder exo = db.Exitorder.Find(id);
            exo.IdState = 4;

            await db.SaveChangesAsync();
            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> NotConfirm(long id)
        {
            Exitorder exo = db.Exitorder.Find(id);
            exo.IdState = 2;

            await db.SaveChangesAsync();
            return Json("Ok", JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public async Task<ActionResult> Checking(long id)
        {
            Exitorder exo = db.Exitorder.Find(id);
            exo.IdState = 3;

            await db.SaveChangesAsync();
            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ShowDetail(long id)
        {


            var list = db.RecordEntryExitOrder.Where(p => p.IdExitOrder == id).ToList().Select(p => new
            {
                p.IdExitOrder,
                CustomerFullName = p.Exitorder.CustomerFullName,
                StoreName = p.Exitorder.Store.Name,
                stateName = p.Exitorder.State.Name,

                Uploaddate = clsPersianDate.MiladiToShamsi(p.Exitorder.Uploaddate),
                p.Record_the_entry.Weight,
                p.Record_the_entry.Transfernumber,
                Dimensions = p.Record_the_entry.length + "*" + p.Record_the_entry.width + "*" + p.Record_the_entry.Height,

            }).ToList();



            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}