using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class UCNDigitalMasterConfig
    {
       // public int? MasterConfigID { get; set; }
        public string key { get; set; }
        public string limit { get; set; }
        public string prefix { get; set; }
        public int index { get; set; }
        public int CompanyID {get; set; }
        public int EnteredBy { get; set; }
        public int MasterConfigHeaderID { get; set; }
        //  public DateTime StartDate { get; set; }
        //  public DateTime? EndDate { get; set; }

        public DateTime EnteredOn { get; set; }
    }
}