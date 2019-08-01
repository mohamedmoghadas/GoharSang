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
    public class ReportExitOrderController : Controller
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
                         where exo.StateDelete == 0
                         select new { exo, reo }).Count()/10)+1;
            return lists;
        }

        private object GetExitOrder(int PageNumber)
        {
            if (PageNumber <= 0)
            {
                PageNumber = 1;
            }
            int PageSkip = (PageNumber - 1) * PageOffSet;


            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         where exo.StateDelete == 0
                         select new { exo, reo }).ToList()
                         .Select(p => new listRecordEntryExitOrder
                         {
                             CustomerFullName = p.exo.CustomerFullName,
                             Uploaddate=clsPersianDate.MiladiToShamsi(p.exo.Uploaddate),
                             StoreName = p.exo.Store.Name,
                             RecordEntryExitOrderCount =  p.exo.RecordEntryExitOrder.Count,
                             stateName = p.exo.State.Name
                         }).OrderBy(u => u.Id)
                .Skip(PageSkip)
                .Take(PageOffSet)
                .ToList();


            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;

            return _vmReportBargirt;
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
            var lists = (from exo in db.Exitorder
                         join reo in db.RecordEntryExitOrder
                         on exo.Id equals reo.IdExitOrder
                         where exo.StateDelete == 0
                         select new { exo, reo }).ToList()
                        .Select(p => new listRecordEntryExitOrder
                        {
                            CustomerFullName = p.exo.CustomerFullName,
                            Uploaddate = clsPersianDate.MiladiToShamsi(p.exo.Uploaddate),
                            StoreName = p.exo.Store.Name,
                            RecordEntryExitOrderCount = p.exo.RecordEntryExitOrder.Count,
                            stateName = p.exo.State.Name
                        }).ToList();

            if (vmr.stateName!=null)
            {
                lists = lists.Where(p => p.stateName.Contains(vmr.stateName)).ToList();
            }
            if (vmr.Uploaddate != null && vmr.Uploaddate != "")
            {
                lists = lists.Where(p => p.Uploaddate.Contains(vmr.Uploaddate)).ToList();
            }


            vmReportBargirt _vmReportBargirt = new vmReportBargirt();
            _vmReportBargirt.list = lists;

            return _vmReportBargirt;
        }

        public ActionResult export()
        {

            var data = TempData["data"] as vmReportBargirt;
            //TempData["data"] = data;

            var uploadPath = Server.MapPath(@"~/File/ReportBargiry.xlsx");
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
                        setValue("A",i, row.ToString());
                       
                        setValue("B", i, item.CustomerFullName);
                        mergeCells("B", i, "C", i);
                        workSheet.Cells["B" + (i).ToString()].Style.WrapText = true;

                        setValue("D", i, item.Uploaddate);
                        mergeCells("D", i, "E", i);
                        workSheet.Cells["D" + (i).ToString()].Style.WrapText = true;

                        setValue("F", i, item.StoreName);
                        mergeCells("F", i, "G", i);
                        workSheet.Cells["F" + (i).ToString()].Style.WrapText = true;

                        setValue("H", i, item.RecordEntryExitOrderCount.ToString());
                        mergeCells("H", i, "I", i);
                        workSheet.Cells["H" + (i).ToString()].Style.WrapText = true;


                        setValue("J", i, item.stateName);
                        mergeCells("J", i, "K", i);
                        workSheet.Cells["J" + (i).ToString()].Style.WrapText = true;



                        i++;
                        row++;

                    }
                }


                string FileName = "گزارش_بارگیری.xlsx";

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