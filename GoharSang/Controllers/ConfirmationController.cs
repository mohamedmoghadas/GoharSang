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
                    //List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

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
            string UserIdcookie = "";

            UserIdcookie = Request.Cookies["UserId"].Value;
            string _Id = UserIdcookie;
            long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
            Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
            List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();
            UserStoreRole UserStoreRole = db.UserStoreRole.Where(p => p.IdUser == admin.Id).FirstOrDefault();

            if (usr.Where(p=>p.IdRole == 8).Any())
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
            else
            {
                var lists = db.Exitorder.Where(p => p.StateDelete == 0 && p.IdStore == UserStoreRole.IdStore)
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
                 //   List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();


                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                       
                            var result = SGetExitOrder(vmr);
                            return View(result);
                       
                            return RedirectToAction("AccessDenied", "Error");

                        

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
            string UserIdcookie = "";

            UserIdcookie = Request.Cookies["UserId"].Value;
            string _Id = UserIdcookie;
            long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
            Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
            List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();
            UserStoreRole UserStoreRole = db.UserStoreRole.Where(p => p.IdUser == admin.Id).FirstOrDefault();

            if (usr.Where(p=>p.IdRole== 8).Any())
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
            else
            {
                var lists = db.Exitorder.Where(p => p.StateDelete == 0 && p.IdStore == UserStoreRole.IdStore)
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

            }



        [HttpPost]
        public async Task<ActionResult> Confirm(long id)
        {
            string UserIdcookie = "";

            UserIdcookie = Request.Cookies["UserId"].Value;
            string _Id = UserIdcookie;
            long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
            Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
            List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

            if (usr.Where(p=>p.IdRole==6).Any())
            {
                Exitorder exo = db.Exitorder.Find(id);
                exo.IdState = 4;

                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(513);
            }
        }

        [HttpPost]
        public async Task<ActionResult> NotConfirm(long id)
        {
            string UserIdcookie = "";

            UserIdcookie = Request.Cookies["UserId"].Value;
            string _Id = UserIdcookie;
            long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
            Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
            List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

            if (usr.Where(p => p.IdRole == 6).Any())
            {

                Exitorder exo = db.Exitorder.Find(id);
                exo.IdState = 2;

                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(513);
            }
        }



        [HttpPost]
        public async Task<ActionResult> Checking(long id)
        {
            string UserIdcookie = "";

            UserIdcookie = Request.Cookies["UserId"].Value;
            string _Id = UserIdcookie;
            long Id = Convert.ToInt16(CreatHash.Decrypt(_Id));
            Users admin = db.Users.FirstOrDefault(p => p.Id == Id);
            List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

            if (usr.Where(p => p.IdRole == 6).Any())
            {
                Exitorder exo = db.Exitorder.Find(id);
                exo.IdState = 3;

                await db.SaveChangesAsync();
            return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(513);
            }
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