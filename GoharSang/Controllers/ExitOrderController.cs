using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoharSang.Models;

namespace GoharSang.Controllers
{
    public class ExitOrderController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ExitOrder(Exitorder _Exitorde,string DateLoad)
        {

            if (DateLoad != null && DateLoad != "")
            {
                DateTime tempdate = clsPersianDate.ShamsiToMiladi(DateLoad).Value;

                _Exitorde.Uploaddate = tempdate;
            }

            db.Exitorder.Add(_Exitorde);
           await db.SaveChangesAsync();

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }
    }
}