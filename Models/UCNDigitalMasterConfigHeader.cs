using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class UCNDigitalMasterConfigHeader
    {
        public int? MasterConfigHeaderID { get; set; }
        public string MasterConfigHeaderName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Company { get; set; }
        public string EnteredByUserName { get; set; }
        public int CompanyID { get; set; }
        public int EnteredBy { get; set; }
        public DateTime EnteredOn { get; set; }

        public List<UCNDigitalMasterConfig> ucndigitalmasterconfig { get; set; }

    }
}