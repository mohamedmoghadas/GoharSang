using GoharSang.Models;
using GoharSang.Models.vmModel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GoharSang.Controllers
{
    public class ReportEntryController : Controller
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
                    List<UserRole> usr = db.UserRole.Where(p => p.IdUser == admin.Id).ToList();

                    if (admin == null)
                    {

                        return RedirectToAction("Index", "LogIn");
                    }
                    else
                    {
                        if (usr.Where(p=>p.IdRole ==2).Any())
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
            var lists = (db.Record_the_entry.Where(p => p.StateDelete == 0).Count()/10)+1;
            return lists;
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
                        if (usr.Where(p=>p.IdRole ==2).Any())
                        {
                            var result = SGetExitOrder(vmr);
                            TempData["data"] = result;
                            ViewBag.PageNumber = 1;
                            ViewBag.AllPage = 1;
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
                    Dimensions = p.length + "*" + p.width + "*" + p.Height,
                    Transfernumber = p.Transfernumber,
                    image = p.Record_the_Entrry_Image.ToList()
                }).ToList();



            if ( vmr.Uploaddate!=null && vmr.Uploaddate!="")
            {
                lists = lists.Where(p => p.Uploaddate == vmr.Uploaddate).ToList() ;
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
                })
               .OrderBy(u => u.Id)
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



                        if (item.image.Where(p => p.IdRecordentry == item.Id).ToList().Count>0)
                        {
                            foreach (var item2 in item.image.Where(p => p.IdRecordentry == item.Id).ToList())
                            {
                                mergeCells("R", i, "S", i);
                                AddImage(excelPackage, "~/images", item2.Image, "R", i);
                            }
                        }


                      



                         



                        i++;
                        row++;

                    }
                }


                string FileName = "گزارش_ورود.xlsx";

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

        private void AddImage(ExcelPackage package, string imagePath,string nameimage,string col, int row)
        {

            var cell = workSheet.Cells[col + row.ToString()];

            var image = Path.Combine(Server.MapPath(imagePath),nameimage);
            if (System.IO.File.Exists(image))
            {

                Image logo = Image.FromFile(image);

                var ws = package.Workbook.Worksheets.FirstOrDefault();

                for (int r = 7; r < 15; r++)
                {
                    ws.Row(r * 5).Height = 39.00D;
                }

                
                    Random rand = new Random();

                    var picture = ws.Drawings.AddPicture(nameimage, logo);


                picture.SetPosition(cell.Start.Row + row, 0, cell.Start.Column + row + 5, 0);


                picture.SetSize(Convert.ToInt32(logo.Width/2), Convert.ToInt32(logo.Height / 2));





                //Bitmap _image = new Bitmap(nameimage);

                //// Bitmap image = new Bitmap();
                //ExcelPicture excelImage = null;
                //if (image != null)
                //{

                //    excelImage = oSheet.Drawings.AddPicture(nameimage, _image);
                //    excelImage.From.Column = colIndex;
                //    excelImage.From.Row = rowIndex;

                //    excelImage.SetSize(160, 60);
                //    // 2x2 px space for better alignment
                //    excelImage.From.ColumnOff = Pixel2MTU(2);
                //    excelImage.From.RowOff = Pixel2MTU(2);
            }
        }
           

        [HttpPost]

        public ActionResult ShowPic(long id)
        {
            var listpic = db.Record_the_Entrry_Image.Where(p => p.IdRecordentry == id)
                .Select(p => p.Image).ToList();
            return Json(listpic, JsonRequestBehavior.AllowGet);
        }

    }

       

    }
