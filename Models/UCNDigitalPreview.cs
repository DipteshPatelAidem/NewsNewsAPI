using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class UCNDigitalPreview
    
        { 
        public string UCNdigitalCode { get; set; }
        public string brandname { get; set; }
        public string caption { get; set; }
        public decimal duration { get; set; }
        public string language { get; set; }
        public string platform { get; set; }
        public string format { get; set; }
        public string ratio { get; set; }         
    }
}