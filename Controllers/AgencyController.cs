using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FullStack.API.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http.Cors;

namespace FullStack.API.Controllers
{
    [RoutePrefix("api/agency")]

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AgencyController : ApiController
    {
       string CS = ConfigurationManager.ConnectionStrings["AidemAgEntities"].ConnectionString;

        [Route("GetAgency")]
        public List<Agency> GetAgency()
        {

            List<Agency> agencyList = new List<Agency>();

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select distinct AgencyCode,AgencyName,AgencyID,RegionCode,Region from vwAgency where isnull(RecordFlag,0)=1 order by AgencyName", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var agency = new Agency();

                    agency.AgencyCode = (int)rdr["AgencyCode"];
                    agency.AgencyID = (string)rdr["AgencyID"];
                    agency.AgencyName = rdr["AgencyName"].ToString();
                    agency.RegionCode = (int)rdr["RegionCode"];
                    agency.Region = rdr["Region"].ToString();
                    agencyList.Add(agency);
                }
            }
            return agencyList;
        }
    }
}
