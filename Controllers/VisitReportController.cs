using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FullStack.API.Models;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/visitreport")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VisitReportController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetVisitReportList")]
        public List<VisitReport> GetVisitReportList(int uCode, DateTime StartDate, DateTime EndDate)
        {
            List<VisitReport> visitreportList = new List<VisitReport>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                // todo  use stored procedure
                // recheck the query 
                //       SqlCommand cmd = new SqlCommand("select top 100  VisitCode,UserCode,ChannelCode,AgencyCode,AdvertiserCode,convert(varchar(max), Remark) as Remark,convert(varchar(max),[Description]) as [Description],VisitDate,EntryDate,ChannelName,AdvertiserName,AgencyName,UserName,MeetingWith,SubjectCode,SubjectName,ReminderDate,ReminderRemark,RemarkFlag,OutComeofMeetingCode,OutComeofMeeting,RegionCode,Region,MeetingWithName,BrandName as BrandNames from vwVisitUpdate where brandname is not null order by 1 desc", con);
                //       SqlCommand cmd = new SqlCommand("select top 100  VisitCode,VisitDate,ChannelName,MeetingwithName,PeopleMeet,AdvertiserName,AgencyName,BrandName as BrandNames ,SubjectName,OutComeofMeetingCode,OutComeofMeeting,Remark ,Description,ProposalValue,ReminderDate,ReminderRemark ,RemarkFlag ,AccompaniedBy from vwVisitUpdate where brandname is not null order by 1 desc", con);

                string qry = "select  VisitCode,VisitDate,ChannelName,MeetingwithName,PeopleMeet,AdvertiserName,AgencyName,BrandName as BrandNames ,SubjectName,OutComeofMeetingCode,OutComeofMeeting,Remark ,Description,ProposalValue,ReminderDate,ReminderRemark ,RemarkFlag ,AccompaniedBy,AccompaniedByCodes,PersonCode,BrandCodes,SubjectCode,ChannelCode,AgencyCode,AdvertiserCode ,UserCode,EntryDate,UserName,Region,RegionCode from vwVisitUpdate where usercode=" + uCode.ToString() + " and ( VisitDate between '" + StartDate.ToString("dd-MMM-yyyy") + "' and '" + EndDate.ToString("dd-MMM-yyyy") + "' ) " + " order by 1 desc";

                SqlCommand cmd = new SqlCommand(qry, con);

                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var visitreport = new VisitReport();

                    visitreport.VisitCode = (int)rdr["visitcode"];
                    visitreport.VisitDate = (DateTime)rdr["VisitDate"];
                    visitreport.ChannelName = rdr["ChannelName"].ToString();
                    visitreport.MeetingWithName = rdr["MeetingWithName"].ToString();
                    visitreport.AdvertiserName = rdr["AdvertiserName"].ToString();
                    visitreport.AgencyName = rdr["AgencyName"].ToString();
                    visitreport.BrandNames = rdr["BrandNames"].ToString();
                    visitreport.PeopleMeet = rdr["PeopleMeet"].ToString();
                    visitreport.SubjectName = rdr["SubjectName"].ToString();


                    visitreport.Description = rdr["Description"].ToString();

                    visitreport.OutcomeOfMeetingName = rdr["OutComeofMeeting"].ToString();
                    visitreport.Remark = rdr["Remark"].ToString();

                    visitreport.MeetingFeedback = rdr["Remark"].ToString();

                    visitreport.OutcomeOfMeetingCode = (int)rdr["OutComeofMeetingCode"];
                    visitreport.ProposalValue = (decimal)rdr["ProposalValue"];

                    if (rdr["ReminderDate"] != DBNull.Value)
                    {
                        visitreport.ReminderDate = (DateTime)rdr["ReminderDate"];
                    }
                    visitreport.ReminderRemark = rdr["ReminderRemark"].ToString();
                    if (rdr["RemarkFlag"] != DBNull.Value)
                    {
                        visitreport.RemarkFlag = (int)rdr["RemarkFlag"];
                    }
                    visitreport.AccompaniedBy = rdr["AccompaniedBy"].ToString();

                    string accByc = rdr["AccompaniedByCodes"].ToString();
                    var res = accByc.Split(',').Where(x => { int tmp; return int.TryParse(x, out tmp); }).Select(x => int.Parse(x)).ToArray();

                    visitreport.AccompaniedByCodes = res;

                    string branCods = rdr["BrandCodes"].ToString();
                    var BrandRes = branCods.Split(',').Where(x => { int tmp; return int.TryParse(x, out tmp); }).Select(x => int.Parse(x)).ToArray();
                    visitreport.BrandCodes = BrandRes;

                    string PersCod = rdr["PersonCode"].ToString();
                    visitreport.PersonCode = PersCod;


                    visitreport.ChannelCode = (int)rdr["ChannelCode"];

            //        sqlreader[indexAge] as int? ?? default(int)
//                    visitreport.AdvertiserCode = (int)rdr["Advertisercode"];
                      visitreport.AdvertiserCode = rdr["Advertisercode"] as int? ?? default(int);
                    //    visitreport.AgencyCode = (int)rdr["AgencyCode"];
                    visitreport.AgencyCode = rdr["AgencyCode"] as int? ?? default(int);

                    visitreport.PurposeID = rdr["SubjectCode"].ToString();
                    visitreport.OutcomeOfMeetingName = rdr["Description"].ToString();
                    visitreport.UserCode = (int)rdr["Usercode"];
                    visitreport.EntryDate = (DateTime)rdr["EntryDate"];
                    visitreport.UserName = rdr["UserName"].ToString();

                    visitreport.InsertedUserCode = (int)rdr["Usercode"];
                    visitreport.Region = rdr["Region"].ToString();

                    visitreport.RegionCode = (int)rdr["Regioncode"];



                    visitreportList.Add(visitreport);
                }
            }
            return visitreportList;
        }

        public List<VisitReport> GetVisitReportByVisitCode(int VisitCode)
        {
            List<VisitReport> visitreportList = new List<VisitReport>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                // todo  use stored procedure
                // recheck the query 
                //       SqlCommand cmd = new SqlCommand("select top 100  VisitCode,UserCode,ChannelCode,AgencyCode,AdvertiserCode,convert(varchar(max), Remark) as Remark,convert(varchar(max),[Description]) as [Description],VisitDate,EntryDate,ChannelName,AdvertiserName,AgencyName,UserName,MeetingWith,SubjectCode,SubjectName,ReminderDate,ReminderRemark,RemarkFlag,OutComeofMeetingCode,OutComeofMeeting,RegionCode,Region,MeetingWithName,BrandName as BrandNames from vwVisitUpdate where brandname is not null order by 1 desc", con);
                     SqlCommand cmd = new SqlCommand("select top 1  VisitCode,VisitDate,ChannelName,MeetingwithName,PeopleMeet,AdvertiserName,AgencyName,BrandName as BrandNames ,SubjectName,OutComeofMeetingCode,OutComeofMeeting,Remark ,Description,ProposalValue,ReminderDate,ReminderRemark ,RemarkFlag ,AccompaniedBy,AccompaniedByCodes,PersonCode,BrandCodes,SubjectCode,ChannelCode,AgencyCode,AdvertiserCode ,UserCode,EntryDate,UserName,Region,RegionCode,MeetingWith from vwVisitUpdate where visitcode=" + VisitCode + " order by VisitCode desc", con);
                //SqlCommand cmd = new SqlCommand("GetVisitUpdate", con);

                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@strFilter", SqlDbType.Text);
                //cmd.Parameters["@strFilter"].Value = "where VisitCode="+ VisitCode.ToString();
                
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var visitreport = new VisitReport();

                    visitreport.VisitCode = (int)rdr["visitcode"];
                    visitreport.VisitDate = (DateTime)rdr["VisitDate"];
                    visitreport.ChannelName = rdr["ChannelName"].ToString();
                    visitreport.MeetingWithName = rdr["MeetingWithName"].ToString();
                    visitreport.AdvertiserName = rdr["AdvertiserName"].ToString();
                    visitreport.AgencyName = rdr["AgencyName"].ToString();
                    visitreport.BrandNames = rdr["BrandNames"].ToString();
                    visitreport.PeopleMeet = rdr["PeopleMeet"].ToString();
                    visitreport.SubjectName = rdr["SubjectName"].ToString();
                    visitreport.PurposeID = rdr["SubjectCode"].ToString();


                    visitreport.Description = rdr["Description"].ToString();
                    visitreport.ActionName = rdr["Description"].ToString();

//                    visitreport.OutcomeOfMeetingName = rdr["OutComeofMeeting"].ToString(); 
                    visitreport.Remark = rdr["Remark"].ToString();

                    visitreport.MeetingFeedback = rdr["Remark"].ToString();
                    visitreport.OutcomeOfMeetingCode = (int)rdr["OutComeofMeetingCode"];
                    visitreport.MeetingTypeID = visitreport.OutcomeOfMeetingCode.ToString();

                    visitreport.ProposalValue = (decimal)rdr["ProposalValue"];

                    if (rdr["ReminderDate"] != DBNull.Value)
                    {
                        visitreport.ReminderDate = (DateTime)rdr["ReminderDate"];
                    }
                    visitreport.ReminderRemark = rdr["ReminderRemark"].ToString();
                    if (rdr["RemarkFlag"] != DBNull.Value)
                    {
                        visitreport.RemarkFlag = (int)rdr["RemarkFlag"];
                    }
                    visitreport.AccompaniedBy = rdr["AccompaniedBy"].ToString();

                    string accByc = rdr["AccompaniedByCodes"].ToString();
                    var res = accByc.Split(',').Where(x => { int tmp; return int.TryParse(x, out tmp); }).Select(x => int.Parse(x)).ToArray();

                    visitreport.AccompaniedByCodes = res;

                    string branCods = rdr["BrandCodes"].ToString();
                    var BrandRes = branCods.Split(',').Where(x => { int tmp; return int.TryParse(x, out tmp); }).Select(x => int.Parse(x)).ToArray();

                    visitreport.BrandCodes = BrandRes;

                    string PersCod = rdr["PersonCode"].ToString();
                
                    visitreport.PersonCode = PersCod;

                    visitreport.ChannelCode = (int)rdr["ChannelCode"];
                    //visitreport.AdvertiserCode = (int)rdr["Advertisercode"];
                    visitreport.AdvertiserCode = rdr["Advertisercode"] as int? ?? default(int);

                    visitreport.MeetingWith = rdr["MeetingWithName"].ToString();

                    visitreport.AgencyCode = rdr["AgencyCode"] as int? ?? default(int);

                    visitreport.PurposeID = rdr["SubjectCode"].ToString();
                    visitreport.OutcomeOfMeetingName= rdr["Description"].ToString();
                    visitreport.UserCode = (int)rdr["Usercode"];
                    visitreport.EntryDate = (DateTime)rdr["EntryDate"];
                    visitreport.UserName = rdr["UserName"].ToString();

                    visitreport.InsertedUserCode = (int)rdr["Usercode"];
                    visitreport.Region = rdr["Region"].ToString();

                    visitreport.RegionCode = (int)rdr["Regioncode"];

                    visitreportList.Add(visitreport);
                }
            }
            return visitreportList;
        }

        [HttpPost]
        [Route("VisitReportPost")]
        public IHttpActionResult VisitReportInsertUpdate(VisitReport visitreport)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("VisitReportInsertUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@VisitDate", SqlDbType.DateTime);
                cmd.Parameters["@VisitDate"].Value = visitreport.VisitDate;

                cmd.Parameters.Add("@UserCode", SqlDbType.Int);//, 4, visitreport.UserCode);
                cmd.Parameters["@UserCode"].Value = visitreport.InsertedUserCode;
                cmd.Parameters.Add("@ChannelCode", SqlDbType.Int);//, 4, visitreport.ChannelCode);
                cmd.Parameters["@ChannelCode"].Value = visitreport.ChannelCode;
                
                cmd.Parameters.Add("@AgencyCode", SqlDbType.Int);//, 4, "AgencyCode")
                //       cmd.Parameters["@AgencyCode"].Value = visitreport.AgencyCode;
                cmd.Parameters["@AgencyCode"].Value = visitreport.AgencyCode as int? ?? default(int);
                cmd.Parameters.Add("@AdvertiserCode", SqlDbType.Int);//, 4, "AdvertiserCode")
                //cmd.Parameters["@AdvertiserCode"].Value = visitreport.AdvertiserCode;
                cmd.Parameters["@AdvertiserCode"].Value = visitreport.AdvertiserCode as int? ?? default(int);
                cmd.Parameters.Add("@ProposalValue", SqlDbType.Money);//, 4, "ProposalValue")
                cmd.Parameters["@ProposalValue"].Value = visitreport.ProposalValue;
                cmd.Parameters.Add("@Remark", SqlDbType.Text);//, 5000, "Remark")
                cmd.Parameters["@Remark"].Value = visitreport.MeetingFeedback;
                cmd.Parameters.Add("@Description", SqlDbType.Text);//, 5000, "Description")
            //  cmd.Parameters["@Description"].Value = visitreport.ActionName;
            //   cmd.Parameters["@Description"].Value = visitreport.OutcomeOfMeetingName;
                cmd.Parameters["@Description"].Value = visitreport.ActionName;

                cmd.Parameters.Add("@SubjectCode", SqlDbType.Int);//,4, visitreport.SubjectCode);
                cmd.Parameters["@SubjectCode"].Value = visitreport.PurposeID;



                //.Add("@ProgramCode", SqlDbType.Int, 4, "ProgramCode")
                //.Add("@TimeBandCode", SqlDbType.Int, 4, "TimeBandCode")



                //.Add("@DealValue", SqlDbType.Money, 4, "DealValue")


                //.Add("@FinalizedBy", SqlDbType.Int, 4, "FinalizedBy")
                //.Add("@DealFor", SqlDbType.Int, 4, "DealFor")
                //.Add("@DealEntryBy", SqlDbType.Int, 4, "DealEntryBy")
                //.Add("@BrandCode", SqlDbType.Int, 4, "BrandCode")
                //.Add("@CreativeAgencyCode", SqlDbType.Int, 4, "CreativeAgencyCode")
                // cmd.Parameters.Add("@ProspectCode", SqlDbType.Int);//, 4, "ProspectCode")


                //cmd.Parameters.Add("@SIRDate", SqlDbType.DateTime);//, 8, "SIRDate")
                //cmd.Parameters["@SIRDate"].Value = visitreport.VisitDate; // todo check visit date or getDate()

                // cmd.Parameters.Add("@ReminderDate", SqlDbType.DateTime);//, 8, "ReminderDate")
                // cmd.Parameters["@ReminderDate"].Value = visitreport.ReminderDate;

                //  cmd.Parameters.Add("@ReminderRemark", SqlDbType.Text);//, 500, "ReminderRemark")
                //  cmd.Parameters["@ReminderRemark"].Value = visitreport.ReminderRemark;

                cmd.Parameters.Add("@OutcomeOfMeetingCode", SqlDbType.Int);
                //                cmd.Parameters["@OutcomeOfMeetingCode"].Value = visitreport.OutcomeOfMeetingCode;
                cmd.Parameters["@OutcomeOfMeetingCode"].Value = visitreport.MeetingTypeID;
                cmd.Parameters.Add("@VisitCode", SqlDbType.Int);
                cmd.Parameters["@VisitCode"].Value = visitreport.VisitCode;
                cmd.Parameters.Add("@PersonCode", SqlDbType.VarChar);
                cmd.Parameters["@PersonCode"].Value = visitreport.PersonCode;
                cmd.Parameters.Add("@BrandCodes", SqlDbType.Text);
              //  cmd.Parameters["@BrandCodes"].Value = String.Join(",", visitreport.BrandCodes);
               if  (visitreport.BrandCodes != null)
                  {
                    cmd.Parameters["@BrandCodes"].Value = String.Join(",", visitreport.BrandCodes.Cast<int?>().ToArray());

                }
                else
                {
                    cmd.Parameters["@BrandCodes"].Value = "";
                }

                cmd.Parameters.Add("@AccompaniedByCodes", SqlDbType.Text);

                //cmd.Parameters["@AccompaniedByCodes"].Value = String.Join(",", visitreport.AccompaniedByCodes);
                if (visitreport.AccompaniedByCodes != null)
                {
                    cmd.Parameters["@AccompaniedByCodes"].Value = String.Join(",", visitreport.AccompaniedByCodes.Cast<int?>().ToArray());
                }
                else {
                    cmd.Parameters["@AccompaniedByCodes"].Value = "";
                }
                //.Add("@ActivityPlanner_Code", SqlDbType.Int, 4, "ActivityPlanner_Code")
                con.Open();

                cmd.ExecuteNonQuery();

            }

            return Ok();
        }

    }
}
