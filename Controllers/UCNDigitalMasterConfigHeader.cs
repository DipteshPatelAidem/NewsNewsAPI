using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class UCNDigitalMasterConfigHeader
    {  
        public int CompanyID { get; set; }
        public int EnteredBy { get; set; }
        public List<UCNDigitalMasterConfig> ucndigitalmasterconfig { get; set; }

    }
}