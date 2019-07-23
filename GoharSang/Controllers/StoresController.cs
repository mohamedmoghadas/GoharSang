using GoharSang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class StoresController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public JsonResult GetData()
        {
            var Store = db.Store.Where(p => p.StateDelete == 0)
                .Select(p => new
                {
                    p.Id,
                    p.Name
                })
                .ToList();
            return Json(Store, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public async Task<ActionResult> Deletedata(long id)
        {


            Store _dmdata =db.Store.Find(id);
            _dmdata.StateDelete = 1;
            await db.SaveChangesAsync();
            return Json("Ok", JsonRequestBehavior.AllowGet);


        }


        [HttpPost]

        public async Task<ActionResult> mgndata(Store _mdata)
        {
            if (_mdata.Id == 0)
            {
                _mdata.StateDelete = 0;
                db.Store.Add(_mdata);
                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            else
            {

                Store _emdata = db.Store.Find(_mdata.Id);
                _emdata.Name = _mdata.Name;
                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
        }

    }
}