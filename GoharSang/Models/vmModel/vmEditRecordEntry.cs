using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoharSang.Models.vmModel
{
    public class vmEditRecordEntry
    {
     public Record_the_entry re { get; set; }
     public   List<mine> listmine { get; set; }
        public   List<Cops> listCops { get; set; }
        public List<Store> listStore { get; set; }
    }
}