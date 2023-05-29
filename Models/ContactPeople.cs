using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class ContactPeople
    {
        public Guid? PersonCode { get; set; }
        public string PersonName { get; set; }
        public string CompanyName { get; set; }
        public string intContactTypeCode { get; set; }

        public int? intDesignationCode { get; set; }
        public string intTitlePersonCode { get; set; }
        public int? intDepartmentCode { get; set; }

        public int intContactForCode { get; set; }
        public string strContactFor { get; set; }

        public int intEnteredBy { get; set; }
        //  public int? intDelFlag { get; set; }

        public string ContactNumber { get; set; }
        public string eMail { get; set; }
    }
}