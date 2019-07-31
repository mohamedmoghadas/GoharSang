using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoharSang.Models.vmModel
{
    public class vmReportBargirt
    {
        public List<listRecordEntryExitOrder> list { get; set; }
        public long AllPage { get; set; }


    }
    public class listRecordEntryExitOrder
    {
        public long Id { get; set; }
        
        public string CustomerFullName { get; set; }
        public string Uploaddate { get; set; }
        public string StoreName { get; set; }
        public long RecordEntryExitOrderCount { get; set; }

        public string stateName { get; set; }
        public string Weight { get; set; }
        public long Countmandeh { get; set; }
        public string Dimensions { get; set; }
        public string minename { get; set; }
        public string copname { get; set; }
        public string Transfernumber { get; set; }
        public string CopCode { get; set; }

        public string DriverName { get; set; }
        public long IdRecordEntryExitOrder { get; set; }
        public long IdRecord_the_entry { get; set; }

        public List<Record_the_Entrry_Image> image { get;set; }
    }
}