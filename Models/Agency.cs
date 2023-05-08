using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class Agency
    {
        public int AgencyCode { get; set; }
        public string AgencyID { get; set; }
        public string AgencyName { get; set; }
        public int RegionCode { get; set; }

        public string Region { get; set; }
    }
}