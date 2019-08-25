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
                    List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (usr.Where(p=>p.IdRole == 1).Any())
                        {
                            return View();

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
            // _dmdata.StateDelete = 1;
            db.mine.Remove(_dmdata);
            await db.SaveChangesAsync();
            return GetData();



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
                return GetData();
            }
            else
            {

                mine _emdata = db.mine.Find(_mdata.Id);
                _emdata.Name = _mdata.Name;
                await db.SaveChangesAsync();
                return GetData();

            }
        }


    }
}