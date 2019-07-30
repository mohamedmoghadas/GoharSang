using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoharSang.Models;

namespace GoharSang.Controllers
{
    public class RecordEntryController : Controller
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

        public ActionResult getMine()
        {
            var ListMine = db.mine.Where(p => p.StateDelete == 0)
                .Select(p => new
                {
                    p.Id,
                    Display=  p.Name
                }).ToList();
            return Json(ListMine, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public ActionResult getCops()
        {
            var ListMine = db.Cops.Where(p => p.StateDelete == 0)
                .Select(p => new
                {
                    p.Id,
                    Display = p.Name
                }).ToList();
            return Json(ListMine, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public ActionResult getStore()
        {
            var ListMine = db.Store.Where(p => p.StateDelete == 0)
                .Select(p => new
                {
                    p.Id,
                    Display = p.Name
                }).ToList();
            return Json(ListMine, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RecordEntry(Record_the_entry _re,string _Date)
        {

            if (_Date != null && _Date!="")
            {
                DateTime tempdate = clsPersianDate.ShamsiToMiladi(_Date).Value;

                _re.Date = tempdate;
            }

            _re.ExitState = false;
            _re.StateDelete = 0;
           db.Record_the_entry.Add(_re);
            await db.SaveChangesAsync();

            #region upload files

            Record_the_Entrry_Image reimg = new Record_the_Entrry_Image();

            var filename = "";

                int dsdfs = 0;

            if (Request.Files != null && Request.Files.Count > 0)
            {

                dsdfs = 1222222;
                #region

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase imgre = Request.Files[i];
                    if (imgre.ContentLength > 0)
                    {

                        //if (hpf.ContentLength > 5242880)
                        //{
                        //    return new HttpStatusCodeResult(532);
                        //}

                        if (!imgre.ContentType.Contains("image"))
                        {
                            return new HttpStatusCodeResult(530);
                        }

                        var number = new Random();
                        filename = _re.Id+""+number.Next()+""+ i + number.Next(1, 999999999).ToString() + ".jpg";
                        var path = Path.Combine(Server.MapPath("~/images"), filename);
                        imgre.SaveAs(path);
                        reimg.Image = filename;
                        reimg.IdRecordentry = _re.Id;
                        db.Record_the_Entrry_Image.Add(reimg);

                        await db.SaveChangesAsync();




                    }
                }


                #endregion

            }

            #endregion


            return Json(dsdfs, JsonRequestBehavior.AllowGet);
        }


    }
}