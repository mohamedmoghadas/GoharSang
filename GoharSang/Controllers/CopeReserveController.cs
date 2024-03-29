﻿using GoharSang.Models;
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


            var list = db.CopsBooking.Where(p => p.StateDelete == 0 && p.RecordEntryCopsBooking.FirstOrDefault().Record_the_entry.ExitState != true).ToList()
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

        [HttpGet]
        public ActionResult SIndex(vmcopreserv vmr) {





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
                        
                            var result = SgetCopReserve(vmr);
                            ViewBag.AllPage = 1;

                            ViewBag.PageNumber = 1;

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

        private object SgetCopReserve(vmcopreserv vmr)
        {
            var list = db.CopsBooking.Where(p => p.StateDelete == 0 && p.RecordEntryCopsBooking.FirstOrDefault().Record_the_entry.ExitState!=true).ToList()
              .Select(p => new
              {

                  Id = p.Id,
                  CustomerFullName = p.CustomerFullName,
                  DateExpired = clsPersianDate.MiladiToShamsi(p.DateExpired),
                  StoreMame = p.Store.Name,
                  Reserved = p.RecordEntryCopsBooking.Where(q => q.IdCopsBooking == p.Id).Count()
              }).ToList();

            if (vmr.DateExpired != null )
            {
                list = list.Where(p => p.DateExpired == vmr.DateExpired).ToList();
            }
            if (vmr.CustomerFullName != null)
            {
                list = list.Where(p => p.CustomerFullName.Contains(vmr.CustomerFullName)).ToList();
            }
            if (vmr.StoreMame != null)
            {
                list = list.Where(p => p.StoreMame.Contains(vmr.StoreMame)).ToList();
            }
            if (vmr.Reserved != 0)
            {
                list = list.Where(p => p.Reserved == vmr.Reserved).ToList();
            }

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
                       List<RecordEntryCopsBooking> RecordEntryCopsBooking = db.RecordEntryCopsBooking.Where(p => p.IdCopsBooking == id).ToList();

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
                           }).ToList();

                        List<Store> ListStore = db.Store.Where(p => p.StateDelete == 0).ToList();


                        vmEditCopReserv _vmEditCopReserv = new vmEditCopReserv();
                        _vmEditCopReserv.RecordEntryCopsBooking = RecordEntryCopsBooking;
                        _vmEditCopReserv.ListRecordEntry = listrecordentry;
                        _vmEditCopReserv.ListStore = ListStore;


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

        [HttpPost]
        public ActionResult ShowDetail(long id)
        {


            var list = db.RecordEntryCopsBooking.Where(p => p.IdCopsBooking == id).ToList().Select(p => new
            {
                p.IdCopsBooking,
                CustomerFullName = p.CopsBooking.CustomerFullName,
                StoreName = p.CopsBooking.Store.Name,
                stateName = p.CopsBooking.State.Name,
                copname=p.Record_the_entry.Cops.Name,
                copcod=p.Record_the_entry.CopsCod,
                DateExpired = clsPersianDate.MiladiToShamsi(p.CopsBooking.DateExpired),
                p.Record_the_entry.Weight,
                p.Record_the_entry.Transfernumber,
                Dimensions = p.Record_the_entry.length + "*" + p.Record_the_entry.width + "*" + p.Record_the_entry.Height,

            }).ToList();



            return Json(list, JsonRequestBehavior.AllowGet);
        }


    }
}