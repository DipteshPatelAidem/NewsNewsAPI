using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class UCNDigitalPlatformFormat
    {
        public int PlatformFormatID { get; set; }
        public int PlatformID { get; set; }
        public string PlatformCode { get; set; }
        public string Platform { get; set; }
        public int FormatID { get; set; }
        public string FormatCode { get; set; }
        public string Format { get; set; }
    }
}