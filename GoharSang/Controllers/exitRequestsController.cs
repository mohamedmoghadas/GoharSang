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
                        if (usr.Where(p => p.IdRole == 7).Any())
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
            long j = 0;

          

            var lists2 = db.RecordEntryExitOrder.Where(p => p.Exitorder.StateDelete == 0 && p.Exitorder.IdState == 4).ToList();
            List<listRecordEntryExitOrder> list = new List<listRecordEntryExitOrder>();

            listRecordEntryExitOrder _p = null;

            foreach (var item in lists2)
            {
                i++;
                _p = new listRecordEntryExitOrder();
                _p.Id = item.Exitorder.Id;
                _p.CustomerFullName = item.Exitorder.CustomerFullName;
                _p.Uploaddate = clsPersianDate.MiladiToShamsi(item.Exitorder.Uploaddate);
                _p.StoreName = item.Exitorder.Store.Name;



                _p.RecordEntryExitOrderCount = i;


                _p.stateName = item.Exitorder.State.Name;
                _p.Weight = item.Record_the_entry.Weight;

                if (item.StateExit == false)
                {
                    j++;
                }
                _p.Countmandeh = j;

            }
            if (_p!=null)
            {

                list.Add(_p);
            }

           


            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = list;

            return _vmReportBargirt;
        }




        public ActionResult SIndex(listRecordEntryExitOrder vmr)
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
                        if (usr.Where(p => p.IdRole == 7).Any())
                        {
                            var result = SGetExitOrder(vmr);
                            ViewBag.PageNumber = 1;
                            ViewBag.AllPage = 1;

                            ViewBag.CustomerFullName = vmr.CustomerFullName;
                            ViewBag.StoreName = vmr.StoreName;
                            ViewBag.RecordEntryExitOrderCount = vmr.RecordEntryExitOrderCount;
                            ViewBag.stateName = vmr.stateName;
                            ViewBag.Uploaddate = vmr.Uploaddate;

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
            long j = 0;



            var lists2 = db.RecordEntryExitOrder.Where(p => p.Exitorder.StateDelete == 0 && p.Exitorder.IdState == 4).ToList();
            List<listRecordEntryExitOrder> lists = new List<listRecordEntryExitOrder>();

            listRecordEntryExitOrder _p = null;

            foreach (var item in lists2)
            {
                i++;
                _p = new listRecordEntryExitOrder();
                _p.Id = item.Exitorder.Id;
                _p.CustomerFullName = item.Exitorder.CustomerFullName;
                _p.Uploaddate = clsPersianDate.MiladiToShamsi(item.Exitorder.Uploaddate);
                _p.StoreName = item.Exitorder.Store.Name;



                _p.RecordEntryExitOrderCount = i;


                _p.stateName = item.Exitorder.State.Name;
                _p.Weight = item.Record_the_entry.Weight;

                if (item.StateExit == false)
                {
                    j++;
                }
                _p.Countmandeh = j;

            }
            if (_p != null)
            {

                lists.Add(_p);
            }


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