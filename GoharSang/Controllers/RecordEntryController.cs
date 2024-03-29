﻿using System;
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
                    List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (usr.Where(p => p.IdRole == 3).Any())
                        {
                            if (PageNumber == null)
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

        [HttpGet]
        public ActionResult SIndex(listRecordEntryExitOrder vmr)
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
                        if (usr.Where(p => p.IdRole == 3).Any())
                        {
                            var result = SGetExitOrder(vmr);
                            ViewBag.PageNumber = 1;
                            ViewBag.AllPage = 1;

                            ViewBag.minename = vmr.minename;
                            ViewBag.copname = vmr.copname;
                            ViewBag.Weight = vmr.Weight;
                            ViewBag.StoreName = vmr.StoreName;
                            ViewBag.Dimensions = vmr.Dimensions;
                            ViewBag.CopCode = vmr.CopCode;
                            ViewBag.Transfernumber = vmr.Transfernumber;
                            ViewBag.Uploaddate = vmr.Uploaddate;

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
                    Weight = p.Weight,
                   
                    SDimensions = p.length.Trim() + "*" + p.width.Trim() + "*" + p.Height.Trim(),
                    Dimensions = p.Height.Trim() + "*" + p.width.Trim() + "*" + p.length.Trim(),

                    Transfernumber = p.Transfernumber,
                    image = p.Record_the_Entrry_Image.ToList()
                }).ToList();



            if ( vmr.Uploaddate != null && vmr.Uploaddate != "")
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
                lists = lists.Where(p => p.Weight==vmr.Weight).ToList();

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
                lists = lists.Where(p => p.SDimensions.Contains(vmr.Dimensions)).ToList();

            }

            if (vmr.CopCode != null)
            {
                lists = lists.Where(p => p.CopCode.Contains(vmr.CopCode)).ToList();

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
                      Dimensions = p.Height.Trim() + "*" + p.width.Trim() + "*" + p.length.Trim(),
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
                    Display = p.Name
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
        public async Task<ActionResult> RecordEntry(Record_the_entry _re, string _Date, HttpPostedFileBase _file)
        {

            if (_re.Id == 0)
            {
                bool uniq = db.Record_the_entry.Where(p => p.CopsCod == _re.CopsCod && p.StateDelete == 0).Any();

                if (uniq)
                {
                    return new HttpStatusCodeResult(513);
                }
                else
                {
                    if (_Date != null && _Date != "")
                    {
                        DateTime tempdate = clsPersianDate.ShamsiToMiladi(_Date).Value.Date;

                        _re.Date = tempdate;
                    }
                    _re.Height=_re.Height.Trim();
                    _re.basculenumber = _re.basculenumber.Trim();
                    _re.CopsCod = _re.CopsCod.Trim();
                    _re.length = _re.length.Trim();
                    _re.Transfernumber = _re.Transfernumber.Trim();
                    _re.Trucknumber = _re.Trucknumber.Trim();
                    _re.Weight = _re.Weight.Trim();
                    _re.width = _re.width.Trim();

                    _re.ExitState = false;
                    _re.StateCopReserve = false;

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



            }
            else
            {
                Record_the_entry _ere = db.Record_the_entry.Find(_re.Id);


                bool uniq = db.Record_the_entry.Where(p => p.CopsCod == _re.CopsCod && p.Id != _ere.Id && p.StateDelete == 0).Any();

                if (uniq)
                {
                    return new HttpStatusCodeResult(513);
                }
                else
                {

                    if (_Date != null && _Date != "")
                    {
                        DateTime tempdate = clsPersianDate.ShamsiToMiladi(_Date).Value.Date;

                        _ere.Date = tempdate;
                    }

                    _ere.basculenumber = _re.basculenumber.Trim();
                    _ere.CopsCod = _re.CopsCod.Trim();

                    _ere.Height = _re.Height.Trim();
                    _ere.IdCops = _re.IdCops;
                    _ere.IdMine = _re.IdMine;
                    _ere.IdStore = _re.IdStore;
                    _ere.length = _re.length.Trim();
                    _ere.Time = _re.Time;
                    _ere.Transfernumber = _re.Transfernumber.Trim();
                    _ere.Trucknumber = _re.Trucknumber.Trim();
                    _ere.Weight = _re.Weight.Trim();
                    _ere.width = _re.width.Trim();

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


        }

        [HttpPost]
        public async Task<ActionResult> deletedata(long id)
        {
            Record_the_entry re = db.Record_the_entry.Find(id);
            //re.StateDelete = 1;
            db.Record_the_entry.Remove(re);
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

        [HttpGet]

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
                        List<mine> listmine = db.mine.Where(p => p.StateDelete == 0).ToList();
                        List<Cops> listCops = db.Cops.Where(p => p.StateDelete == 0).ToList();
                        List<Store> listStore = db.Store.Where(p => p.StateDelete == 0).ToList();

                        vmEditRecordEntry _vmEditRecordEntry = new vmEditRecordEntry();
                        _vmEditRecordEntry.re = re;
                        _vmEditRecordEntry.listmine = listmine;
                        _vmEditRecordEntry.listCops = listCops;
                        _vmEditRecordEntry.listStore = listStore;

                        return View(_vmEditRecordEntry);
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