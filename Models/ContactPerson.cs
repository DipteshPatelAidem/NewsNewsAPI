using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class ContactPerson
    {
        public Guid PersonCode { get; set; }
         
        public string PersonName { get; set; }
        // Yet to add other columns like company name from company master!
        public string CompanyName { get; set; }

        public string PersonName2 { get; set; }

        public string ContactFor { get; set; }

        public int ContactForCode { get; set; }
    }
}