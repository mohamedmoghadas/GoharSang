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
                        var result = getRecordEntry();
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

        private object getRecordEntry()
        {
            var listrecordentry = db.Record_the_entry.Where(p => p.StateDelete == 0).ToList()
                .Select(p => new vmListRecordEntry
                {
                    Id=p.Id,
                    minename = p.mine.Name,
                    copname = p.Cops.Name,
                    Dimensions = p.length + "*" + p.width + "*" + p.Height,
                    Weight= p.Weight,
                    CopsCod= p.CopsCod,
                    Transfernumber= p.Transfernumber,
                    image=db.Record_the_Entrry_Image.Where(q=>q.Id==p.Id).FirstOrDefault()==null?""
                    : db.Record_the_Entrry_Image.Where(q => q.Id == p.Id).FirstOrDefault().Image,
                }).ToList();

           

            return listrecordentry;
        }


        public async Task<ActionResult> RegisterCopeReserve(CopsBooking cb,string date,ItemPropSelect prop)
        {
            if (cb.Id==0)
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
                   
                    _listprops.Add(_p);
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
                    _p = new RecordEntryCopsBooking();
                    _p.IdCopsBooking = cb.Id;
                    _p.IdRecordEntry = item.Id;
                    

                    _listprops.Add(_p);
                }
                db.RecordEntryCopsBooking.AddRange(_listprops);
                await db.SaveChangesAsync();


                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
        } 

        
    }
}