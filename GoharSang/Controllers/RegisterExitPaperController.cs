using GoharSang.Models;
using GoharSang.Models.SelectedProps;
using GoharSang.Models.vmModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class RegisterExitPaperController : Controller
    {

        GoharSangEntities db = new GoharSangEntities();

       
        public ActionResult Index(long? id)
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
                        if (id != null && id != 0)
                        {
                            var result = GetExitOrder(id);
                            return View(result);
                        }
                        else
                        {
                            return RedirectToAction("Index", "exitRequests");
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

        private object GetExitOrder(long? id)
        {

          
            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         join re in db.Record_the_entry
                         on reo.IdRecordEntry equals re.Id
                         where exo.StateDelete == 0 && exo.Id==id && reo.StateExit==false && exo.IdState==4
                         select new { exo, reo, re }).ToList()
                         .Select(p => new listRecordEntryExitOrder
                         {
                             Id = p.reo.Id,
                             CustomerFullName = p.exo.CustomerFullName,
                             Uploaddate = clsPersianDate.MiladiToShamsi(p.exo.Uploaddate),
                             StoreName = p.exo.Store.Name,
                             minename=p.re.mine.Name,
                             copname=p.re.Cops.Name,
                             RecordEntryExitOrderCount = p.exo.RecordEntryExitOrder.Count,
                             stateName = p.exo.State.Name,
                             Weight = p.re.Weight,
                             Dimensions = p.re.length + "*" + p.re.width + "*" + p.re.Height
                         }).ToList();


            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;

            return _vmReportBargirt;
        }


        [HttpPost]

        public async Task<ActionResult> RegisterExitPaper(Driver _driver, ItemPropSelect prop)
        {
            db.Driver.Add(_driver);
            await db.SaveChangesAsync();

            List<DriverREO> _listprops = new List<DriverREO>();
            DriverREO _p = null;

            foreach (var item in prop.ListProps)
            {

                RecordEntryExitOrder reo = db.RecordEntryExitOrder.Find(item.Id);

                reo.StateExit = true;
                await db.SaveChangesAsync();

                _p = new DriverREO();
                _p.IdDriver = _driver.Id;
                _p.IdREO = item.Id;

                _listprops.Add(_p);
            }
            db.DriverREO.AddRange(_listprops);
            await db.SaveChangesAsync();

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }
    }
}