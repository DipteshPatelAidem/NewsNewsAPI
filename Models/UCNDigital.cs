using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class UCNDigital
    {
        public int? UCNid { get; set; }
        public string UCNdigitalCode { get; set; }
        public int BrandID { get; set; }
        public string Caption { get; set; }
        public int DurationID { get; set; }
        public int LanguageID { get; set; }
        public int PlatformID { get; set; }
        public int FormatID { get; set; }
        public int? RatioID { get; set; }
        public dynamic config { get; set; }
        public int EnteredBy { get; set; }
     //   public DateTime EnteredOn { get; set; }
        public int? UpdatedBy { get; set; }
     //   public DateTime UpdatedOn { get; set; }
        public int CompanyID { get; set; }
    }
}