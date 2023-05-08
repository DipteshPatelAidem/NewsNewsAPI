using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FullStack.API.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/meetingtype")]

    public class MeetingTypeController : ApiController
    {
          string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetMeetingType")]

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public List<MeetingType> GetMeetingType()
        {
            
            List<MeetingType> meetingtypeList = new List<MeetingType>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select * from mstgeneric where generickey1 like '%MstSIROutCome%'", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var meetingtype = new MeetingType();


                    meetingtype.GenericID = (string)rdr["GenericID"];
                    meetingtype.GenericName = rdr["GenericName"].ToString();
                    meetingtypeList.Add(meetingtype);
                }
            }
            return meetingtypeList;
        }

    }
}
