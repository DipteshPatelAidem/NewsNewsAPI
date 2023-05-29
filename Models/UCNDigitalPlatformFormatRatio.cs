using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class UCNDigitalPlatformFormatRatio
    {
        public int? PlatformFormatRatioID { get; set; }
        public int PlatformFormatID { get; set; }
        public int PlatformID { get; set; }
        public string PlatformCode { get; set; }
        public string Platform { get; set; }
        public int FormatID { get; set; }
        public string FormatCode { get; set; }
        public string Format { get; set; }

        public int RatioID { get; set; }
        public string Ratio { get; set; }
    }
}