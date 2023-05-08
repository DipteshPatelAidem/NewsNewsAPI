using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FullStack.API.Models
{
    public class VisitReport
    {
        public int VisitCode { get; set; }
        public DateTime VisitDate { get; set; }
        public int ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public string MeetingWithName { get; set; }
        public string PersonCode { get; set; }
        public string PeopleMeet { get; set; }
        public string AgencyName { get; set; }

        public string AdvertiserName { get; set; }

        public string PurposeID { get; set; }
        public string SubjectName { get; set; }

        public string BrandNames { get; set; }
        public string MeetingFeedback { get; set; } //        public string Remark { get; set; }
        public string Remark { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public string OutcomeOfMeetingName { get; set;}
        //    public int MeetingTypeID { get; set; } //   public int OutcomeOfMeetingCode { get; set; }
        public string MeetingTypeID { get; set; } //   public int OutcomeOfMeetingCode { get; set; }
        public int OutcomeOfMeetingCode { get; set; }
        public int UserCode { get; set; }
        public int? AgencyCode { get; set; }
        public int? AdvertiserCode { get; set; }
        public DateTime EntryDate { get; set; }        
        public string UserName { get; set; }
        public string MeetingWith { get; set; }
        public DateTime ReminderDate { get; set; }
        public string ReminderRemark { get; set; }               
        public int RegionCode { get; set; }
        public string Region { get; set; }
        public decimal ProposalValue { get; set; }
        public int RemarkFlag { get; set; }
        public string AccompaniedBy { get; set; }
        public int[] AccompaniedByCodes { get; set; }

        public int[] BrandCodes { get; set; }
        public int InsertedUserCode { get; set; }
    }
}