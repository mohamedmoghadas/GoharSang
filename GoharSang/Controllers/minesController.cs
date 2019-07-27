using GoharSang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class minesController : Controller
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

        public JsonResult GetData()
        {
            var mine = db.mine.Where(p => p.StateDelete == 0)
                .Select(p => new
                {
                    p.Id,
                    p.Name
                })
                .ToList();
            return Json(mine, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public async Task<ActionResult> Deletedata(long id)
        {


            mine _dmdata = db.mine.Find(id);
            _dmdata.StateDelete = 1;
            await db.SaveChangesAsync();
            return Json("Ok", JsonRequestBehavior.AllowGet);


        }


        [HttpPost]

        public async Task<ActionResult> mgndata(mine _mdata)
        {

            bool uniq = db.mine.Where(p => p.Name == _mdata.Name).Any();
            if (uniq)
            {
                return new HttpStatusCodeResult(510);
            }

            if (_mdata.Id == 0)
            {
                _mdata.StateDelete = 0;
                db.mine.Add(_mdata);
                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            else
            {

                mine _emdata = db.mine.Find(_mdata.Id);
                _emdata.Name = _mdata.Name;
                await db.SaveChangesAsync();
                return Json("Ok", JsonRequestBehavior.AllowGet);

            }
        }


    }
}