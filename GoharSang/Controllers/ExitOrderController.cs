using System;
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
    public class ExitOrderController : Controller
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
                        if (usr.Where(p=>p.IdRole ==5).Any())
                        {
                            var result = getRecordEntry();
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

        private object getRecordEntry()
        {
            var listrecordentry = db.Record_the_entry.Where(p => p.StateDelete == 0 && p.ExitState==false).ToList()

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



            return listrecordentry;
        }


        public async Task<ActionResult> ExitOrder(Exitorder exo, string date, ItemPropSelect prop)
        {
            if (exo.Id == 0)
            {
                exo.StateDelete = 0;
                if (date != null && date != "")
                {
                    DateTime tempdate = clsPersianDate.ShamsiToMiladi(date).Value;

                    exo.Uploaddate = tempdate;
                }
                exo.IdState = 1;

                db.Exitorder.Add(exo);
                await db.SaveChangesAsync();

                List<RecordEntryExitOrder> _listprops = new List<RecordEntryExitOrder>();
                RecordEntryExitOrder _p = null;

                foreach (var item in prop.ListProps)
                {
                    RecordEntryCopsBooking recb = db.RecordEntryCopsBooking.Where(p => p.IdRecordEntry == item.Id).FirstOrDefault();

                    if (recb!=null)
                    {
                        if (exo.CustomerFullName== recb.CopsBooking.CustomerFullName)
                        {
                            _p = new RecordEntryExitOrder();
                            _p.IdExitOrder = exo.Id;
                            _p.IdRecordEntry = item.Id;
                            _p.StateExit = false;
                            _listprops.Add(_p);

                        }
                        else
                        {
                            return new HttpStatusCodeResult(514);
                        }
                    }
                    else
                    {
                        _p = new RecordEntryExitOrder();
                        _p.IdExitOrder = exo.Id;
                        _p.IdRecordEntry = item.Id;
                        _p.StateExit = false;
                        _listprops.Add(_p);
                    }
                   

                }
                db.RecordEntryExitOrder.AddRange(_listprops);
                await db.SaveChangesAsync();

                foreach (var item2 in prop.ListProps)
                {
                    Record_the_entry re = db.Record_the_entry.Find(item2.Id);
                    re.ExitState = true;
                    await db.SaveChangesAsync();
                }


                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
            else
            {
                var deleteprops = db.RecordEntryExitOrder.Where(p => p.IdExitOrder == exo.Id);
                db.RecordEntryExitOrder.RemoveRange(deleteprops);
                await db.SaveChangesAsync();

                Exitorder ecb = db.Exitorder.Find(exo.Id);

                ecb.IdState = 1;


                ecb.StateDelete = 0;
                if (date != null && date != "")
                {
                    DateTime tempdate = clsPersianDate.ShamsiToMiladi(date).Value;

                    ecb.Uploaddate = tempdate;
                }

                ecb.IdStore = exo.IdStore;
                ecb.CustomerFullName = exo.CustomerFullName;

                await db.SaveChangesAsync();

                List<RecordEntryExitOrder> _listprops = new List<RecordEntryExitOrder>();
                RecordEntryExitOrder _p = null;

                foreach (var item in prop.ListProps)
                {
                    RecordEntryCopsBooking recb = db.RecordEntryCopsBooking.Where(p => p.IdRecordEntry == item.Id).FirstOrDefault();

                    if (recb != null)
                    {
                        if (exo.CustomerFullName == recb.CopsBooking.CustomerFullName)
                        {
                            _p = new RecordEntryExitOrder();
                            _p.IdExitOrder = exo.Id;
                            _p.IdRecordEntry = item.Id;
                            _p.StateExit = false;


                            _listprops.Add(_p);
                        }
                        else
                        {
                            return new HttpStatusCodeResult(514);

                        }
                    }
                    else
                    {
                        _p = new RecordEntryExitOrder();
                        _p.IdExitOrder = exo.Id;
                        _p.IdRecordEntry = item.Id;
                        _p.StateExit = false;


                        _listprops.Add(_p);
                    }
                }
                db.RecordEntryExitOrder.AddRange(_listprops);
                await db.SaveChangesAsync();


                foreach (var item2 in prop.ListProps)
                {
                    Record_the_entry re = db.Record_the_entry.Find(item2.Id);
                    re.ExitState = true;
                    await db.SaveChangesAsync();
                }

                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
        }
    }
}