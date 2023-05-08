using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class Brand
    {
        public int BrandCode { get; set; }
        public string BrandID { get; set; }
        public string BrandName { get; set; }
        public string AdvertiserName { get; set; }
        public int AdvertiserCode { get; set; }
    }
}