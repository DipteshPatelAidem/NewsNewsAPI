using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class Designation
    {
        public int DesignationCode { get; set; }

        public string DesignationID { get; set; }

        public string DesignationName { get; set; }

        public int UserLevel { get; set; }

        public int DesgnLevel { get; set; }
    }
}