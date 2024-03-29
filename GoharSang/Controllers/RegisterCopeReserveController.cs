﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoharSang.Models;
using GoharSang.Models.SelectedProps;
using GoharSang.Models.vmModel;

namespace GoharSang.Controllers
{
    public class RegisterCopeReserveController : Controller
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
                        return View();



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
        public JsonResult getDataRecordEntry()
        {
            var listrecordentry = db.Record_the_entry.Where(p => p.StateDelete == 0 && p.ExitState == false && p.StateCopReserve == false).ToList()
                .Select(p => new 
                {
                    Id = p.Id,
                    minename = p.mine.Name,
                    copname = p.Cops.Name,
                    Dimensions = p.length + "*" + p.width + "*" + p.Height,
                    Weight = p.Weight,
                    CopsCod = p.CopsCod,
                    Transfernumber = p.Transfernumber,
                    p.IdStore
                })
               .ToList();



            return Json(listrecordentry,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRecordEntry(long? StoreId, string productCode)
        {
            if (StoreId == null || StoreId == 0)
            {
                return new HttpStatusCodeResult(520);
            }
            if (productCode == null || productCode == "0")
            {
                return new HttpStatusCodeResult(521);
            }
            var listrecordentry = db.Record_the_entry.Where(p => p.StateDelete == 0 && p.ExitState == false && p.StateCopReserve == false && p.CopsCod == productCode && p.IdStore == StoreId).ToList()

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



            return Json(listrecordentry, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetModalRecordEntry(long? StoreId)
        {
            if (StoreId == null || StoreId == 0)
            {
                return new HttpStatusCodeResult(520);
            }

            var listrecordentry = db.Record_the_entry.Where(p => p.StateDelete == 0 && p.ExitState == false && p.StateCopReserve == false && p.IdStore == StoreId).ToList()

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



            return Json(listrecordentry, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> RegisterCopeReserve(CopsBooking cb, string date, ItemPropSelect prop)
        {
            if (cb.Id == 0)
            {
                cb.StateDelete = 0;
                if (date != null && date != "")
                {
                    DateTime tempdate = clsPersianDate.ShamsiToMiladi(date).Value;

                    cb.DateExpired = tempdate;
                }

                cb.IdState = 1;

                db.CopsBooking.Add(cb);
                await db.SaveChangesAsync();

                List<RecordEntryCopsBooking> _listprops = new List<RecordEntryCopsBooking>();
                RecordEntryCopsBooking _p = null;

                foreach (var item in prop.ListProps)
                {

                    _p = new RecordEntryCopsBooking();
                    _p.IdCopsBooking = cb.Id;
                    _p.IdRecordEntry = item.Id;
                    _p.StateExit = false;


                    if (_listprops.Where(p => p.IdRecordEntry == item.Id).Any())
                    {
                        continue;
                    }

                    _listprops.Add(_p);
                    Record_the_entry re = db.Record_the_entry.Find(item.Id);
                    re.StateCopReserve = true;
                    await db.SaveChangesAsync();
                }
                db.RecordEntryCopsBooking.AddRange(_listprops);
                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
            else
            {
                var deleteprops = db.RecordEntryCopsBooking.Where(p => p.IdCopsBooking == cb.Id);
                db.RecordEntryCopsBooking.RemoveRange(deleteprops);
                await db.SaveChangesAsync();

                CopsBooking ecb = db.CopsBooking.Find(cb.Id);

                ecb.StateDelete = 0;
                if (date != null && date != "")
                {
                    DateTime tempdate = clsPersianDate.ShamsiToMiladi(date).Value;

                    ecb.DateExpired = tempdate;
                }
                ecb.IdState = 1;

                ecb.IdStore = cb.IdStore;
                ecb.CustomerFullName = cb.CustomerFullName;

                await db.SaveChangesAsync();

                List<RecordEntryCopsBooking> _listprops = new List<RecordEntryCopsBooking>();
                RecordEntryCopsBooking _p = null;

                foreach (var item in prop.ListProps)
                {
                    Record_the_entry re = db.Record_the_entry.Find(item.Id);
                    re.StateCopReserve = true;
                    await db.SaveChangesAsync();

                    _p = new RecordEntryCopsBooking();
                    _p.IdCopsBooking = cb.Id;
                    _p.IdRecordEntry = item.Id;

                    _p.StateExit = false;

                    _listprops.Add(_p);
                }
                db.RecordEntryCopsBooking.AddRange(_listprops);
                await db.SaveChangesAsync();


                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
        }


    }
}