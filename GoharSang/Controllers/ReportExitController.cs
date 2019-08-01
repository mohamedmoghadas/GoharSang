using GoharSang.Models;
using GoharSang.Models.vmModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class ReportExitController : Controller
    {
        GoharSangEntities db = new GoharSangEntities();
        ExcelWorksheet workSheet = null;
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
                    UserRole usr = db.UserRole.Where(p => p.IdUser == admin.Id).FirstOrDefault();
                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (usr.IdRole == 2)
                        {
                            if (PageNumber == null)
                            {
                                var result = GetExitOrder(1);
                                TempData["data"] = result;
                                ViewBag.PageNumber = 1;
                                ViewBag.AllPage = getTotalList();
                                return View(result);
                            }
                            else
                            {
                                var result = GetExitOrder((int)PageNumber);
                                TempData["data"] = result;
                                ViewBag.PageNumber = (int)PageNumber;
                                ViewBag.AllPage = getTotalList();
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

        private dynamic getTotalList()
        {
            var lists = ((from exo in db.Exitorder
                          join reo in db.RecordEntryExitOrder
                          on exo.Id equals reo.IdExitOrder
                          join driverREO in db.DriverREO
                          on reo.Id equals driverREO.IdREO
                          join re in db.Record_the_entry
                          on reo.IdRecordEntry equals re.Id
                          where exo.StateDelete == 0
                          select new { exo, reo, re, driverREO }).Count() / 10) + 1;
            return lists;
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
                    UserRole usr = db.UserRole.Where(p => p.IdUser == admin.Id).FirstOrDefault();

                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (usr.IdRole == 2)
                        {
                            var result = SGetExitOrder(vmr);
                            TempData["data"] = result;

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
            long i = 0;
            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         join driverREO in db.DriverREO
                         on reo.Id equals driverREO.IdREO
                         join re in db.Record_the_entry
                         on reo.IdRecordEntry equals re.Id
                         where exo.StateDelete == 0
                         select new { exo, reo, re, driverREO }).ToList()
                         .Select(p => new listRecordEntryExitOrder
                         {
                             Id = p.exo.Id,
                             CustomerFullName = p.exo.CustomerFullName,
                             Uploaddate = clsPersianDate.MiladiToShamsi(p.exo.Uploaddate),
                             StoreName = p.exo.Store.Name,
                             RecordEntryExitOrderCount = p.exo.RecordEntryExitOrder.Count,
                             stateName = p.exo.State.Name,
                             Weight = p.re.Weight,
                             Countmandeh = p.reo.StateExit == false ? i++ : 0,
                             DriverName = p.driverREO.Driver.DriverName
                         }).ToList();



            if (vmr.Uploaddate!=null && vmr.Uploaddate != "")
            {
                lists = lists.Where(p => p.Uploaddate.Contains(vmr.Uploaddate)).ToList();
            }

            if (vmr.CustomerFullName != null)
            {
                lists = lists.Where(p => p.CustomerFullName.Contains(vmr.CustomerFullName)).ToList();
            }

            if (vmr.StoreName != null)
            {
                lists = lists.Where(p => p.StoreName.Contains(vmr.StoreName)).ToList();
            }

            if (vmr.RecordEntryExitOrderCount != 0)
            {
                lists = lists.Where(p => p.RecordEntryExitOrderCount == vmr.RecordEntryExitOrderCount).ToList();
            }

            if (vmr.stateName != null)
            {
                lists = lists.Where(p => p.stateName.Contains(vmr.stateName)).ToList();
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

            long i = 0;
            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         join driverREO in db.DriverREO
                         on reo.Id equals driverREO.IdREO
                         join re in db.Record_the_entry
                         on reo.IdRecordEntry equals re.Id
                         where exo.StateDelete == 0
                         select new { exo, reo, re,driverREO }).ToList()
                         .Select(p => new listRecordEntryExitOrder
                         {
                             Id = p.exo.Id,
                             CustomerFullName = p.exo.CustomerFullName,
                             Uploaddate = clsPersianDate.MiladiToShamsi(p.exo.Uploaddate),
                             StoreName = p.exo.Store.Name,
                             RecordEntryExitOrderCount = p.exo.RecordEntryExitOrder.Count,
                             stateName = p.exo.State.Name,
                             Weight = p.re.Weight,
                             Countmandeh = p.reo.StateExit == false ? i++ : 0,
                             DriverName=p.driverREO.Driver.DriverName
                         }).OrderBy(u => u.Id)
                .Skip(PageSkip)
                .Take(PageOffSet)
                .ToList();


            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;

            return _vmReportBargirt;
        }




        public ActionResult export()
        {

            var data = TempData["data"] as vmReportBargirt;
            //TempData["data"] = data;

            var uploadPath = Server.MapPath(@"~/File/ReportEntry.xlsx");
            FileInfo fileInfo = new FileInfo(uploadPath);
            bool state = fileInfo.Exists;
            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();


                // setValue("O", 2, DateTime.Now.ToString());

                if (data != null && data.list.Count() > 0)
                {
                    int row = 1;
                    int i = 7;


                    foreach (var item in data.list)
                    {
                        setValue("A", i, row.ToString());

                        setValue("B", i, item.minename);
                        mergeCells("B", i, "C", i);
                        workSheet.Cells["B" + (i).ToString()].Style.WrapText = true;

                        setValue("D", i, item.copname);
                        mergeCells("D", i, "E", i);
                        workSheet.Cells["D" + (i).ToString()].Style.WrapText = true;

                        setValue("F", i, item.StoreName);
                        mergeCells("F", i, "G", i);
                        workSheet.Cells["F" + (i).ToString()].Style.WrapText = true;

                        setValue("H", i, item.Weight);
                        mergeCells("H", i, "I", i);
                        workSheet.Cells["H" + (i).ToString()].Style.WrapText = true;

                        setValue("J", i, item.Dimensions);
                        mergeCells("J", i, "K", i);
                        workSheet.Cells["J" + (i).ToString()].Style.WrapText = true;


                        setValue("L", i, item.CopCode);
                        mergeCells("L", i, "M", i);
                        workSheet.Cells["L" + (i).ToString()].Style.WrapText = true;

                        setValue("N", i, item.Uploaddate);
                        mergeCells("N", i, "O", i);
                        workSheet.Cells["N" + (i).ToString()].Style.WrapText = true;

                        setValue("P", i, item.Transfernumber);
                        mergeCells("P", i, "Q", i);
                        workSheet.Cells["P" + (i).ToString()].Style.WrapText = true;

                        i++;
                        row++;

                    }
                }


                string FileName = "گزارش_خروج.xlsx";

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename= " + FileName);
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.End();
            }
            return null;
        }


        public void setValue(string col, int row, string value)
        {
            workSheet.Cells[col + row.ToString()].Value = value;
            workSheet.Cells[col + row.ToString()].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            workSheet.Cells[col + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[col + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
        public void mergeCells(string col1, int row1, string col2, int row2)
        {
            workSheet.Cells[col1 + row1.ToString() + ":" + col2 + row2.ToString()].Merge = true;
            workSheet.Cells[col1 + row1.ToString() + ":" + col2 + row2.ToString()].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[col1 + row1.ToString() + ":" + col2 + row2.ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[col1 + row1.ToString() + ":" + col2 + row2.ToString()].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[col1 + row1.ToString() + ":" + col2 + row2.ToString()].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }
    }
}