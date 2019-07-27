using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoharSang.Models.vmModel
{
    public class vmReportBargirt
    {
        public List<listRecordEntryExitOrder> list { get; set; }
       
    }
    public class listRecordEntryExitOrder
    {
        public string CustomerFullName { get; set; }
        public string Uploaddate { get; set; }
        public string StoreName { get; set; }
        public long RecordEntryExitOrderCount { get; set; }

        public string stateName { get; set; }
    }
}