using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoharSang.Models;
using GoharSang.Models.vmModel;

namespace GoharSang.Controllers
{
    public class RecordEntryController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();

        int PageOffSet = 10;

        public ActionResult Index(int? PageNumber)
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
                        if (PageNumber==null)
                        {
                            var result = GetExitOrder(1);
                            ViewBag.PageNumber = 1;

                            return View(result);
                        }
                        else
                        {
                            var result = GetExitOrder((int)PageNumber);
                            ViewBag.PageNumber = (int)PageNumber;

                            return View(result);
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
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        var result = SGetExitOrder(vmr);
                       

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

        private object SGetExitOrder(listRecordEntryExitOrder vmr)
        {
            var lists = db.Record_the_entry.Where(p => p.StateDelete == 0)
                .ToList().Select(p => new listRecordEntryExitOrder
                {
                    Id = p.Id,



                    Uploaddate = clsPersianDate.MiladiToShamsi(p.Date),
                    StoreName = p.Store.Name,
                    copname = p.Cops.Name,
                    CopCode = p.CopsCod,
                    minename = p.mine.Name,
                    // RecordEntryExitOrderCount = p.RecordEntryExitOrder.Count,
                    //  stateName = p.State.Name,
                    Weight = p.Weight,
                    Dimensions = p.length + "*" + p.width + "*" + p.Height,
                    Transfernumber = p.Transfernumber,
                    image = p.Record_the_Entrry_Image.ToList()
                }).ToList();



            if (vmr.Uploaddate != null && vmr.Uploaddate != "")
            {
                lists = lists.Where(p => p.Uploaddate == vmr.Uploaddate).ToList();
            }
            if (vmr.minename != null)
            {
                lists = lists.Where(p => p.minename.Contains(vmr.minename)).ToList();

            }
            if (vmr.copname != null)
            {
                lists = lists.Where(p => p.copname.Contains(vmr.copname)).ToList();

            }

            if (vmr.Weight != null)
            {
                lists = lists.Where(p => p.Weight.Contains(vmr.Weight)).ToList();

            }
            if (vmr.StoreName != null)
            {
                lists = lists.Where(p => p.StoreName.Contains(vmr.StoreName)).ToList();

            }


            if (vmr.Transfernumber != null)
            {
                lists = lists.Where(p => p.Transfernumber.Contains(vmr.Transfernumber)).ToList();

            }


            if (vmr.Dimensions != null)
            {
                lists = lists.Where(p => p.Dimensions.Contains(vmr.Dimensions)).ToList();

            }





            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;
            return _vmReportBargirt;
        }

        private object GetExitOrder(int PageNumber)
        {

            if (PageNumber <= 0)
            {
                PageNumber = 1;
            }
            int PageSkip = (PageNumber - 1) * PageOffSet;

            var lists = db.Record_the_entry.Where(p => p.StateDelete == 0)
                  .ToList().Select(p => new listRecordEntryExitOrder
                  {
                      Id = p.Id,



                      Uploaddate = clsPersianDate.MiladiToShamsi(p.Date),
                      StoreName = p.Store.Name,
                      copname = p.Cops.Name,
                      CopCode = p.CopsCod,
                      minename = p.mine.Name,

                      Weight = p.Weight,
                      Dimensions = p.length + "*" + p.width + "*" + p.Height,
                      Transfernumber = p.Transfernumber,
                      image = p.Record_the_Entrry_Image.ToList()
                  }).OrderBy(u => u.Id)
                .Skip(PageSkip)
                .Take(PageOffSet)
                .ToList();





            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;
            _vmReportBargirt.AllPage = (db.Record_the_entry.Where(p => p.StateDelete == 0).Count() / 10) + 1;
            return _vmReportBargirt;
        }

        public ActionResult RegisterPage()
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
        public async Task<ActionResult> RecordEntry(Record_the_entry _re,string _Date, HttpPostedFileBase _file)
        {

            if (_re.Id==0)
            {
                if (_Date != null && _Date != "")
                {
                    DateTime tempdate = clsPersianDate.ShamsiToMiladi(_Date).Value.Date;

                    _re.Date = tempdate;
                }

                _re.ExitState = false;
                _re.StateDelete = 0;
                db.Record_the_entry.Add(_re);
                await db.SaveChangesAsync();






                Record_the_Entrry_Image reimg = new Record_the_Entrry_Image();

                var filename = "";



                if (Request.Files.Count != 0 && Request.Files != null)
                {

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase imgre = Request.Files[i];
                        if (imgre.ContentLength > 0)
                        {



                            if (!imgre.ContentType.Contains("image"))
                            {
                                return new HttpStatusCodeResult(530);
                            }

                            var number = new Random();
                            filename = _re.Id + "" + number.Next() + "" + i + number.Next(1, 999999999).ToString() + ".jpg";
                            var path = Path.Combine(Server.MapPath("~/images"), filename);
                            imgre.SaveAs(path);
                            reimg.Image = filename;
                            reimg.IdRecordentry = _re.Id;
                            db.Record_the_Entrry_Image.Add(reimg);

                            await db.SaveChangesAsync();




                        }
                    }

                }

                return Json("Ok", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Record_the_entry _ere = db.Record_the_entry.Find(_re.Id);

                if (_Date != null && _Date != "")
                {
                    DateTime tempdate = clsPersianDate.ShamsiToMiladi(_Date).Value.Date;

                    _ere.Date = tempdate;
                }

                _ere.basculenumber = _re.basculenumber;
                _ere.CopsCod = _re.CopsCod;

                _ere.Height = _re.Height;
                _ere.IdCops = _re.IdCops;
                _ere.IdMine = _re.IdMine;
                _ere.IdStore = _re.IdStore;
                _ere.length = _re.length;
                _ere.Time = _re.Time;
                _ere.Transfernumber = _re.Transfernumber;
                _ere.Trucknumber = _re.Trucknumber;
                _ere.Weight = _re.Weight;
                _ere.width = _re.width;

                await db.SaveChangesAsync();






                Record_the_Entrry_Image reimg = new Record_the_Entrry_Image();

                var filename = "";



                if (Request.Files.Count != 0 && Request.Files != null)
                {

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase imgre = Request.Files[i];
                        if (imgre.ContentLength > 0)
                        {



                            if (!imgre.ContentType.Contains("image"))
                            {
                                return new HttpStatusCodeResult(530);
                            }
                            Record_the_Entrry_Image ereimg = db.Record_the_Entrry_Image.Where(p => p.IdRecordentry == _re.Id).FirstOrDefault();

                            if (ereimg!=null)
                            {
                                var oldfile = Path.Combine(Server.MapPath("~/images"), ereimg.Image);
                                if (System.IO.File.Exists(oldfile))
                                {
                                    System.IO.File.Delete(oldfile);
                                    db.Record_the_Entrry_Image.Remove(ereimg);
                                    await db.SaveChangesAsync();

                                }
                            }
                                
                            
                            var number = new Random();
                            filename = _re.Id + "" + number.Next() + "" + i + number.Next(1, 999999999).ToString() + ".jpg";
                            var path = Path.Combine(Server.MapPath("~/images"), filename);
                            imgre.SaveAs(path);
                            reimg.Image = filename;
                            reimg.IdRecordentry = _ere.Id;
                            db.Record_the_Entrry_Image.Add(reimg);

                            await db.SaveChangesAsync();




                        }
                    }

                }

                return Json("Ok", JsonRequestBehavior.AllowGet);
            }

           
        }

        [HttpPost]
        public async Task<ActionResult> deletedata(long id)
        {
            Record_the_entry re = db.Record_the_entry.Find(id);
            re.StateDelete = 1;
            await db.SaveChangesAsync();

            Record_the_Entrry_Image ereimg = db.Record_the_Entrry_Image.Where(p => p.IdRecordentry == id).FirstOrDefault();

            if (ereimg != null)
            {
                var oldfile = Path.Combine(Server.MapPath("~/images"), ereimg.Image);
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                    db.Record_the_Entrry_Image.Remove(ereimg);
                    await db.SaveChangesAsync();

                }
            }

            return Json("Ok", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]

        public ActionResult ediddata(long id)
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
                        Record_the_entry re = db.Record_the_entry.Find(id);
                        return View(re);
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

    }
}